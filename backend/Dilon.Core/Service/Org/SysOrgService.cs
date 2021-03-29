using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 组织机构服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Org", Order = 148)]
    public class SysOrgService : ISysOrgService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysOrg> _sysOrgRep;  // 组织机构表仓储 
        private readonly IUserManager _userManager;

        private readonly ISysEmpService _sysEmpService;
        private readonly ISysEmpExtOrgPosService _sysEmpExtOrgPosService;
        private readonly ISysRoleDataScopeService _sysRoleDataScopeService;
        private readonly ISysUserDataScopeService _sysUserDataScopeService;

        public SysOrgService(IRepository<SysOrg> sysOrgRep,
                             IUserManager userManager,
                             ISysEmpService sysEmpService,
                             ISysEmpExtOrgPosService sysEmpExtOrgPosService,
                             ISysRoleDataScopeService sysRoleDataScopeService,
                             ISysUserDataScopeService sysUserDataScopeService)
        {
            _sysOrgRep = sysOrgRep;
            _userManager = userManager;
            _sysEmpService = sysEmpService;
            _sysEmpExtOrgPosService = sysEmpExtOrgPosService;
            _sysRoleDataScopeService = sysRoleDataScopeService;
            _sysUserDataScopeService = sysUserDataScopeService;
        }

        /// <summary>
        /// 分页查询组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysOrg/page")]
        public async Task<dynamic> QueryOrgPageList([FromQuery] PageOrgInput input)
        {
            var dataScopeList = GetDataScopeList(await GetUserDataScopeIdList());

            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            var id = !string.IsNullOrEmpty(input.Id?.Trim());
            var pId = !string.IsNullOrEmpty(input.Pid?.Trim());
            var orgs = await _sysOrgRep.DetachedEntities
                                       .Where((name, u => EF.Functions.Like(u.Name, $"%{input.Name.Trim()}%")), // 根据机构名称模糊查询
                                              (id, u => u.Id == long.Parse(input.Id.Trim())), // 根据机构id查询
                                              (pId, u => EF.Functions.Like(u.Pids, $"%[{input.Pid.Trim()}]%")
                                                         || u.Id == long.Parse(input.Pid.Trim()))) // 根据父机构id查询
                                       .Where(dataScopeList.Count > 0, u => dataScopeList.Contains(u.Id)) // 非管理员范围限制
                                       .Where(u => u.Status != CommonStatus.DELETED).OrderBy(u => u.Sort)
                                       .Select(u => u.Adapt<OrgOutput>())
                                       .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<OrgOutput>.PageResult(orgs);
        }

        /// <summary>
        /// (非管理员)获取当前用户数据范围（机构Id）
        /// </summary>
        /// <param name="dataScopes"></param>
        /// <returns></returns>
        private List<long> GetDataScopeList(List<long> dataScopes)
        {
            var dataScopeList = new List<long>();
            // 如果是超级管理员则获取所有组织机构，否则只获取其数据范围的机构数据
            if (!_userManager.SuperAdmin)
            {
                if (dataScopes.Count < 1)
                    return dataScopeList;

                // 此处获取所有的上级节点，用于构造完整树
                dataScopes.ForEach(u =>
                {
                    var sysOrg = _sysOrgRep.DetachedEntities.FirstOrDefault(c => c.Id == u);
                    var parentAndChildIdListWithSelf = sysOrg.Pids.TrimEnd(',').Replace("[", "").Replace("]", "")
                                                                  .Split(",").Select(u => long.Parse(u)).ToList();
                    dataScopeList.AddRange(parentAndChildIdListWithSelf);
                });
            }
            return dataScopeList;
        }

        /// <summary>
        /// 获取组织机构列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysOrg/list")]
        public async Task<List<OrgOutput>> GetOrgList([FromQuery] OrgInput input)
        {
            var dataScopeList = GetDataScopeList(await GetUserDataScopeIdList());

            var pId = !string.IsNullOrEmpty(input.Pid?.Trim());
            var orgs = await _sysOrgRep.DetachedEntities
                                       .Where(pId, u => u.Pid == long.Parse(input.Pid))
                                       .Where(dataScopeList.Count > 0, u => dataScopeList.Contains(u.Id))
                                       .Where(u => u.Status != CommonStatus.DELETED).OrderBy(u => u.Sort).ToListAsync();
            return orgs.Adapt<List<OrgOutput>>();
        }

        /// <summary>
        /// 增加组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysOrg/add")]
        public async Task AddOrg(AddOrgInput input)
        {
            var isExist = await _sysOrgRep.DetachedEntities.AnyAsync(u => u.Name == input.Name || u.Code == input.Code);
            if (isExist)
                throw Oops.Oh(ErrorCode.D2002);

            if (!_userManager.SuperAdmin)
            {
                // 如果新增的机构父Id不是0，则进行数据权限校验
                if (input.Pid != "0" && !string.IsNullOrEmpty(input.Pid))
                {
                    // 新增组织机构的父机构不在自己的数据范围内
                    var dataScopes = await GetUserDataScopeIdList();
                    if (dataScopes.Count < 1 || !dataScopes.Contains(long.Parse(input.Pid)))
                        throw Oops.Oh(ErrorCode.D2003);
                }
                else
                    throw Oops.Oh(ErrorCode.D2003);
            }

            var sysOrg = input.Adapt<SysOrg>();
            await FillPids(sysOrg);
            await sysOrg.InsertNowAsync();
        }

        /// <summary>
        /// 填充父Ids字段
        /// </summary>
        /// <param name="sysOrg"></param>
        /// <returns></returns>
        private async Task FillPids(SysOrg sysOrg)
        {
            if (sysOrg.Pid == 0L)
            {
                sysOrg.Pids = "[" + 0 + "],";
            }
            else
            {
                var t = await _sysOrgRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == sysOrg.Pid);
                sysOrg.Pids = t.Pids + "[" + t.Id + "],";
            }
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysOrg/delete")]
        public async Task DeleteOrg(DeleteOrgInput input)
        {
            var sysOrg = await _sysOrgRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == long.Parse(input.Id));

            // 检测数据范围能不能操作这个机构
            var dataScopes = await GetUserDataScopeIdList();
            if (!_userManager.SuperAdmin && (dataScopes.Count < 1 || !dataScopes.Contains(sysOrg.Id)))
                throw Oops.Oh(ErrorCode.D2003);

            // 该机构下有员工，则不能删
            var hasOrgEmp = await _sysEmpService.HasOrgEmp(sysOrg.Id);
            if (hasOrgEmp)
                throw Oops.Oh(ErrorCode.D2004);

            // 该附属机构下若有员工，则不能删
            var hasExtOrgEmp = await _sysEmpExtOrgPosService.HasExtOrgEmp(sysOrg.Id);
            if (hasExtOrgEmp)
                throw Oops.Oh(ErrorCode.D2005);

            // 级联删除子节点
            var childIdList = await GetChildIdListWithSelfById(sysOrg.Id);
            var orgs = await _sysOrgRep.Where(u => childIdList.Contains(u.Id)).ToListAsync();
            orgs.ForEach(u =>
            {
                u.DeleteNow();
            });

            // 级联删除该机构及子机构对应的角色-数据范围关联信息
            await _sysRoleDataScopeService.DeleteRoleDataScopeListByOrgIdList(childIdList);

            // 级联删除该机构子机构对应的用户-数据范围关联信息
            await _sysUserDataScopeService.DeleteUserDataScopeListByOrgIdList(childIdList);
        }

        /// <summary>
        /// 更新组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysOrg/edit")]
        public async Task UpdateOrg(UpdateOrgInput input)
        {
            if (input.Pid != "0" && !string.IsNullOrEmpty(input.Pid))
            {
                var org = await _sysOrgRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == long.Parse(input.Pid));
                _ = org ?? throw Oops.Oh(ErrorCode.D2000);
            }
            if (input.Id == input.Pid)
                throw Oops.Oh(ErrorCode.D2001);

            var sysOrg = await _sysOrgRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == long.Parse(input.Id));

            // 检测数据范围能不能操作这个机构
            var dataScopes = await GetUserDataScopeIdList();
            if (!_userManager.SuperAdmin && (dataScopes.Count < 1 || !dataScopes.Contains(sysOrg.Id)))
                throw Oops.Oh(ErrorCode.D2003);

            var isExist = await _sysOrgRep.DetachedEntities.AnyAsync(u => (u.Name == input.Name || u.Code == input.Code) && u.Id != sysOrg.Id);
            if (isExist)
                throw Oops.Oh(ErrorCode.D2002);

            //如果名称有变化，则修改对应员工的机构相关信息
            if (!sysOrg.Name.Equals(input.Name))
                await _sysEmpService.UpdateEmpOrgInfo(sysOrg.Id, sysOrg.Name);

            sysOrg = input.Adapt<SysOrg>();
            await FillPids(sysOrg);
            await sysOrg.UpdateNowAsync(ignoreNullValues: true);
        }

        /// <summary>
        /// 获取组织机构信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysOrg/detail")]
        public async Task<SysOrg> GetOrg([FromQuery] QueryOrgInput input)
        {
            return await _sysOrgRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == long.Parse(input.Id));
        }

        /// <summary>
        /// 根据节点Id获取所有子节点Id集合，包含自己
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<long>> GetChildIdListWithSelfById(long id)
        {
            var childIdList = await _sysOrgRep.DetachedEntities
                                              .Where(u => EF.Functions.Like(u.Pids, $"%{id}%"))
                                              .Select(u => u.Id).ToListAsync();
            childIdList.Add(id);
            return childIdList;
        }

        /// <summary>
        /// 获取组织机构树       
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysOrg/tree")]
        public async Task<dynamic> GetOrgTree([FromQuery] OrgInput input)
        {
            var dataScopeList = new List<long>();
            if (!_userManager.SuperAdmin)
            {
                var dataScopes = await GetUserDataScopeIdList();
                if (dataScopes.Count < 1)
                    return dataScopeList;
                dataScopeList = GetDataScopeList(dataScopes);
            }
            var orgs = await _sysOrgRep.DetachedEntities.Where(dataScopeList.Count > 0, u => dataScopeList.Contains(u.Id))
                                                        .Where(u => u.Status == (int)CommonStatus.ENABLE).OrderBy(u => u.Sort)
                                                        .Select(u => new OrgTreeNode
                                                        {
                                                            Id = u.Id,
                                                            ParentId = u.Pid,
                                                            Title = u.Name,
                                                            Value = u.Id.ToString(),
                                                            Weight = u.Sort
                                                        }).ToListAsync();

            return new TreeBuildUtil<OrgTreeNode>().DoTreeBuild(orgs);
        }

        /// <summary>
        /// 根据数据范围类型获取当前用户的数据范围（机构Id）集合
        /// </summary>
        /// <param name="dataScopeType"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<long>> GetDataScopeListByDataScopeType(int dataScopeType, long orgId)
        {
            var orgIdList = new List<long>();
            if (orgId < 0)
                return orgIdList;

            // 如果是范围类型是全部数据，则获取当前所有的组织架构Id
            if (dataScopeType == (int)DataScopeType.ALL)
            {
                orgIdList = await _sysOrgRep.DetachedEntities.Where(u => u.Status != (int)CommonStatus.ENABLE).Select(u => u.Id).ToListAsync();
            }
            // 如果范围类型是本部门及以下部门，则查询本节点和子节点集合，包含本节点
            else if (dataScopeType == (int)DataScopeType.DEPT_WITH_CHILD)
            {
                orgIdList = await GetChildIdListWithSelfById(orgId);
            }
            // 如果数据范围是本部门，不含子节点，则直接返回本部门
            else if (dataScopeType == (int)DataScopeType.DEPT)
            {
                orgIdList.Add(orgId);
            }
            return orgIdList;
        }

        /// <summary>
        /// 获取用户数据范围（机构Id集合）
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public async Task<List<long>> GetUserDataScopeIdList()
        {
            return await App.GetService<ISysUserService>().GetUserDataScopeIdList();
        }
    }
}
