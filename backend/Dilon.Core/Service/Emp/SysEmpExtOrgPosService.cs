using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 员工附属机构和职位服务
    /// </summary>
    public class SysEmpExtOrgPosService : ISysEmpExtOrgPosService, ITransient
    {
        private readonly IRepository<SysEmpExtOrgPos> _sysEmpExtOrgPosRep;  // 附属机构表仓储 

        public SysEmpExtOrgPosService(IRepository<SysEmpExtOrgPos> sysEmpExtOrgPosRep)
        {
            _sysEmpExtOrgPosRep = sysEmpExtOrgPosRep;
        }

        /// <summary>
        /// 保存或编辑附属机构相关信息
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        public async Task AddOrUpdate(long empId, List<EmpExtOrgPosOutput> extIdList)
        {
            // 先删除
            await DeleteEmpExtInfoByUserId(empId);

            var tasks = new List<Task>();
            extIdList.ForEach(u =>
            {
                tasks.Add(new SysEmpExtOrgPos
                {
                    SysEmpId = empId,
                    SysOrgId = u.OrgId,
                    SysPosId = u.PosId
                }.InsertAsync());
            });
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 获取附属机构和职位信息
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<List<EmpExtOrgPosOutput>> GetEmpExtOrgPosList(long empId)
        {
            return await _sysEmpExtOrgPosRep.DetachedEntities
                                            .Where(u => u.SysEmpId == empId)
                                            .Select(u => new EmpExtOrgPosOutput
                                            {
                                                OrgId = u.SysOrg.Id,
                                                OrgCode = u.SysOrg.Code,
                                                OrgName = u.SysOrg.Name,
                                                PosId = u.SysPos.Id,
                                                PosCode = u.SysPos.Code,
                                                PosName = u.SysPos.Name
                                            }).ToListAsync();
        }

        /// <summary>
        /// 根据机构Id判断该附属机构下是否有员工
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<bool> HasExtOrgEmp(long orgId)
        {
            return await _sysEmpExtOrgPosRep.DetachedEntities.AnyAsync(u => u.SysOrgId == orgId);
        }

        /// <summary>
        /// 根据职位Id判断该附属职位下是否有员工
        /// </summary>
        /// <param name="posId"></param>
        /// <returns></returns>
        public async Task<bool> HasExtPosEmp(long posId)
        {
            return await _sysEmpExtOrgPosRep.DetachedEntities.AnyAsync(u => u.SysPosId == posId);
        }

        /// <summary>
        /// 根据员工Id删除对应的员工-附属信息
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task DeleteEmpExtInfoByUserId(long empId)
        {
            var empExtOrgPos = await _sysEmpExtOrgPosRep.Where(u => u.SysEmpId == empId).ToListAsync();
            empExtOrgPos.ForEach(u =>
            {
                u.Delete();
            });
        }
    }
}
