using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 员工服务
    /// </summary>
    public class SysEmpService : ISysEmpService, ITransient
    {
        private readonly IRepository<SysEmp> _sysEmpRep;  // 员工表仓储

        private readonly ISysEmpExtOrgPosService _sysEmpExtOrgPosService;
        private readonly ISysEmpPosService _sysEmpPosService;

        public SysEmpService(IRepository<SysEmp> sysEmpRep,
                             ISysEmpExtOrgPosService sysEmpExtOrgPosService,
                             ISysEmpPosService sysEmpPosService)
        {
            _sysEmpRep = sysEmpRep;
            _sysEmpExtOrgPosService = sysEmpExtOrgPosService;
            _sysEmpPosService = sysEmpPosService;
        }

        /// <summary>
        /// 获取用户员工相关信息（包括登录）
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<EmpOutput> GetEmpInfo(long empId)
        {
            var empInfoOutput = new EmpOutput();
            var sysEmp = await _sysEmpRep.FirstOrDefaultAsync(u => u.Id == empId, false);
            if (sysEmp == null) return empInfoOutput;

            empInfoOutput = sysEmp.Adapt<EmpOutput>();
            empInfoOutput.ExtOrgPos = await _sysEmpExtOrgPosService.GetEmpExtOrgPosList(empId);
            empInfoOutput.Positions = await _sysEmpPosService.GetEmpPosList(empId);
            return empInfoOutput;
        }

        /// <summary>
        /// 增加或编辑员工相关信息
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        public async Task AddOrUpdate(EmpOutput2 sysEmpParam)
        {
            // 先删除员工信息
            var emps = await _sysEmpRep.Where(u => u.Id == long.Parse(sysEmpParam.Id)).ToListAsync();
            await _sysEmpRep.DeleteAsync(emps);

            // 再新增新员工信息
            var emp = sysEmpParam.Adapt<SysEmp>();
            await _sysEmpRep.InsertAsync(emp);

            // 更新附属机构职位信息
            await _sysEmpExtOrgPosService.AddOrUpdate(emp.Id, sysEmpParam.ExtIds);

            // 更新职位信息
            await _sysEmpPosService.AddOrUpdate(emp.Id, sysEmpParam.PosIdList);
        }

        /// <summary>
        /// 修改员工相关机构信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="orgName"></param>
        /// <returns></returns>
        public async Task UpdateEmpOrgInfo(long orgId, string orgName)
        {
            var emps = await _sysEmpRep.Where(u => u.OrgId == orgId).ToListAsync();
            emps.ForEach(u =>
            {
                u.OrgName = orgName;
            });
        }

        /// <summary>
        /// 根据机构Id判断该机构下是否有员工
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<bool> HasOrgEmp(long orgId)
        {
            return await _sysEmpRep.DetachedEntities.AnyAsync(u => u.OrgId == orgId);
        }

        /// <summary>
        /// 根据员工Id删除对应的员工表信息
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task DeleteEmpInfoByUserId(long empId)
        {
            // 删除员工信息
            var emp = await _sysEmpRep.FirstOrDefaultAsync(u => u.Id == empId);
            await emp.DeleteAsync();

            // 级联删除对应的员工-附属信息
            await _sysEmpExtOrgPosService.DeleteEmpExtInfoByUserId(empId);

            // 级联删除对用的员工-职位信息
            await _sysEmpPosService.DeleteEmpPosInfoByUserId(empId);
        }

        /// <summary>
        /// 获取员工机构Id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<long> GetEmpOrgId(long empId)
        {
            return (await _sysEmpRep.FirstOrDefaultAsync(u => u.Id == empId, false)).OrgId;
        }

        /// <summary>
        /// 获取子机构用户
        /// </summary>
        /// <param name="orgIds"></param>
        /// <returns></returns>
        public async Task<List<SysEmp>> HasOrgEmp(List<long> orgIds)
        {
            return await _sysEmpRep.DetachedEntities.Where(u => orgIds.Contains(u.OrgId)).ToListAsync();
        }
    }
}