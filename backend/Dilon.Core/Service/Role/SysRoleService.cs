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
    /// 角色服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Role", Order = 149)]
    public class SysRoleService : ISysRoleService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysRole> _sysRoleRep;  // 角色表仓储
        private readonly IRepository<SysUserRole> _sysUserRoleRep;  // 用户角色表仓储

        private readonly IUserManager _userManager;
        private readonly ISysRoleDataScopeService _sysRoleDataScopeService;
        private readonly ISysOrgService _sysOrgService;
        private readonly ISysRoleMenuService _sysRoleMenuService;
        private readonly ISysCacheService _sysCacheService;

        public SysRoleService(IRepository<SysRole> sysRoleRep,
                              IRepository<SysUserRole> sysUserRoleRep,
                              IUserManager userManager,
                              ISysRoleDataScopeService sysRoleDataScopeService,
                              ISysOrgService sysOrgService,
                              ISysRoleMenuService sysRoleMenuService,
                              ISysCacheService sysCacheService)
        {
            _sysRoleRep = sysRoleRep;
            _sysUserRoleRep = sysUserRoleRep;
            _userManager = userManager;
            _sysRoleDataScopeService = sysRoleDataScopeService;
            _sysOrgService = sysOrgService;
            _sysRoleMenuService = sysRoleMenuService;
            _sysCacheService = sysCacheService;
        }

        /// <summary>
        /// 获取用户角色相关信息（登录）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<RoleOutput>> GetUserRoleList(long userId)
        {
            return await _sysRoleRep.DetachedEntities.Join(_sysUserRoleRep.DetachedEntities, u => u.Id, e => e.SysRoleId, (u, e) => new { u, e })
                                    .Where(x => x.e.SysUserId == userId)
                                    .Select(x => x.u.Adapt<RoleOutput>()).ToListAsync();
        }

        /// <summary>
        /// 分页获取角色列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysRole/page")]
        public async Task<dynamic> QueryRolePageList([FromQuery] RolePageInput input)
        {
            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            var code = !string.IsNullOrEmpty(input.Code?.Trim());
            var roles = await _sysRoleRep.DetachedEntities
                                         .Where((name, u => EF.Functions.Like(u.Name, $"%{input.Name.Trim()}%")),
                                                (code, u => EF.Functions.Like(u.Code, $"%{input.Code.Trim()}%")))
                                         .Where(u => u.Status == CommonStatus.ENABLE).OrderBy(u => u.Sort)
                                         .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<SysRole>.PageResult(roles);
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<dynamic> GetRoleList([FromQuery] RoleInput input)
        {
            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            var code = !string.IsNullOrEmpty(input.Code?.Trim());
            return await _sysRoleRep.DetachedEntities
                                    .Where((name, u => EF.Functions.Like(u.Name, $"%{input.Name.Trim()}%")),
                                           (code, u => EF.Functions.Like(u.Code, $"%{input.Code.Trim()}%")))
                                    .Where(u => u.Status == CommonStatus.ENABLE).OrderBy(u => u.Sort)
                                    .Select(u => new
                                    {
                                        u.Id,
                                        Name = u.Name + "[" + u.Code + "]"
                                    }).ToListAsync();
        }

        /// <summary>
        /// 角色下拉（用于授权角色时选择）
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysRole/dropDown")]
        public async Task<dynamic> GetRoleDropDown()
        {
            // 如果不是超级管理员，则查询自己拥有的角色集合
            var roles = _userManager.SuperAdmin
                        ? await _sysUserRoleRep.Where(u => u.SysUserId == _userManager.UserId).Select(u => u.SysRoleId).ToListAsync()
                        : new List<long>();

            return await _sysRoleRep.DetachedEntities
                                    .Where(roles.Count > 0, u => roles.Contains(u.Id))
                                    .Where(u => u.Status == CommonStatus.ENABLE)
                                    .Select(u => new
                                    {
                                        u.Id,
                                        u.Code,
                                        u.Name
                                    }).ToListAsync();
        }

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysRole/add")]
        public async Task AddRole(AddRoleInput input)
        {
            var isExist = await _sysRoleRep.DetachedEntities.AnyAsync(u => u.Code == input.Code || u.Name == input.Name);
            if (isExist)
                throw Oops.Oh(ErrorCode.D1006);

            var role = input.Adapt<SysRole>();
            role.DataScopeType = DataScopeType.ALL; // 新角色默认全部数据范围
            await role.InsertAsync();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysRole/delete")]
        [UnitOfWork]
        public async Task DeleteRole(DeleteRoleInput input)
        {
            var sysRole = await _sysRoleRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            await sysRole.DeleteAsync();

            //级联删除该角色对应的角色-数据范围关联信息
            await _sysRoleDataScopeService.DeleteRoleDataScopeListByRoleId(sysRole.Id);

            ////级联删除该角色对应的用户-角色表关联信息
            //await _sysUserRoleService.DeleteUserRoleListByRoleId(sysRole.Id); // 避免循环引用，故用下面逻辑
            var userRoles = await _sysUserRoleRep.Where(u => u.SysRoleId == sysRole.Id).ToListAsync();
            userRoles.ForEach(u =>
            {
                u.Delete();
            });

            //级联删除该角色对应的角色-菜单表关联信息
            await _sysRoleMenuService.DeleteRoleMenuListByRoleId(sysRole.Id);
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysRole/edit")]
        public async Task UpdateRole(UpdateRoleInput input)
        {
            var isExist = await _sysRoleRep.DetachedEntities.AnyAsync(u => (u.Name == input.Name || u.Code == input.Code) && u.Id != input.Id);
            if (isExist)
                throw Oops.Oh(ErrorCode.D1006);

            var sysRole = input.Adapt<SysRole>();
            await sysRole.UpdateAsync();
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysRole/detail")]
        public async Task<SysRole> GetRoleInfo([FromQuery] QueryRoleInput input)
        {
            return await _sysRoleRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
        }

        /// <summary>
        /// 授权角色菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysRole/grantMenu")]
        public async Task GrantMenu(GrantRoleMenuInput input)
        {
            await _sysRoleMenuService.GrantMenu(input);
        }

        /// <summary>
        /// 授权角色数据范围
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysRole/grantData")]
        public async Task GrantData(GrantRoleDataInput input)
        {
            // 清除所有用户数据范围缓存
            await _sysCacheService.DelByPatternAsync(CommonConst.CACHE_KEY_DATASCOPE);

            var role = await _sysRoleRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            var dataScopeType = input.DataScopeType;
            if (!_userManager.SuperAdmin)
            {
                //如果授权的角色的数据范围类型为全部，则没权限，只有超级管理员有
                if ((int)DataScopeType.ALL == dataScopeType)
                    throw Oops.Oh(ErrorCode.D1016);

                //如果授权的角色数据范围类型为自定义，则要判断授权的数据范围是否在自己的数据范围内
                if ((int)DataScopeType.DEFINE == dataScopeType)
                {
                    var dataScopes = await _sysOrgService.GetUserDataScopeIdList();
                    var grantOrgIdList = input.GrantOrgIdList; //要授权的数据范围列表
                    if (grantOrgIdList.Count > 0)
                    {
                        if (dataScopes.Count < 1)
                            throw Oops.Oh(ErrorCode.D1016);
                        else if (!dataScopes.All(u => grantOrgIdList.Any(c => c == u)))
                            throw Oops.Oh(ErrorCode.D1016);
                    }
                }
            }
            role.DataScopeType = (DataScopeType)dataScopeType;
            await _sysRoleDataScopeService.GrantDataScope(input);
        }

        /// <summary>
        /// 根据角色Id集合获取数据范围Id集合
        /// </summary>
        /// <param name="roleIdList"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<long>> GetUserDataScopeIdList(List<long> roleIdList, long orgId)
        {
            // 定义角色中最大数据范围的类型，目前按最大范围策略来，如果你同时拥有ALL和SELF的权限，最后按ALL返回
            int strongerDataScopeType = (int)DataScopeType.SELF;

            var customDataScopeRoleIdList = new List<long>();
            if (roleIdList != null && roleIdList.Count > 0)
            {
                var roles = await _sysRoleRep.DetachedEntities.Where(u => roleIdList.Contains(u.Id)).ToListAsync();
                roles.ForEach(u =>
                {
                    if (u.DataScopeType == DataScopeType.DEFINE)
                        customDataScopeRoleIdList.Add(u.Id);
                    else if ((int)u.DataScopeType <= strongerDataScopeType)
                        strongerDataScopeType = (int)u.DataScopeType;
                });
            }

            // 自定义数据范围的角色对应的数据范围
            var roleDataScopeIdList = await _sysRoleDataScopeService.GetRoleDataScopeIdList(customDataScopeRoleIdList);

            // 角色中拥有最大数据范围类型的数据范围
            var dataScopeIdList = await _sysOrgService.GetDataScopeListByDataScopeType(strongerDataScopeType, orgId);

            return roleDataScopeIdList.Concat(dataScopeIdList).Distinct().ToList(); //并集
        }

        /// <summary>
        /// 根据角色Id获取角色名称
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<string> GetNameByRoleId(long roleId)
        {
            var role = await _sysRoleRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == roleId);
            if (role == null)
                throw Oops.Oh(ErrorCode.D1002);
            return role.Name;
        }

        /// <summary>
        /// 获取角色拥有菜单Id集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysRole/ownMenu")]
        public async Task<List<long>> OwnMenu([FromQuery] QueryRoleInput input)
        {
            return await _sysRoleMenuService.GetRoleMenuIdList(new List<long> { input.Id });
        }

        /// <summary>
        /// 获取角色拥有数据Id集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysRole/ownData")]
        public async Task<List<long>> OwnData([FromQuery] QueryRoleInput input)
        {
            return await _sysRoleDataScopeService.GetRoleDataScopeIdList(new List<long> { input.Id });
        }
    }
}