using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 员工职位服务
    /// </summary>
    public class SysEmpPosService : ISysEmpPosService, ITransient
    {
        private readonly IRepository<SysEmpPos> _sysEmpPosRep;  // 员工职位表仓储

        public SysEmpPosService(IRepository<SysEmpPos> sysEmpPosRep)
        {
            _sysEmpPosRep = sysEmpPosRep;
        }

        /// <summary>
        /// 增加或编辑员工职位相关信息
        /// </summary>
        /// <param name="empId">员工Id（用户Id）</param>
        /// <param name="posIdList">职位id集合</param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task AddOrUpdate(long empId, List<long> posIdList)
        {
            // 先删除
            await DeleteEmpPosInfoByUserId(empId);

            var empPos = posIdList.Select(u => new SysEmpPos
            {
                SysEmpId = empId,
                SysPosId = u
            }).ToList();
            await _sysEmpPosRep.InsertAsync(empPos);
        }

        /// <summary>
        /// 获取所属职位信息
        /// </summary>
        /// <param name="empId">员工Id（用户Id）</param>
        public async Task<List<EmpPosOutput>> GetEmpPosList(long empId)
        {
            return await _sysEmpPosRep.DetachedEntities
                                      .Where(u => u.SysEmpId == empId)
                                      .Select(u => new EmpPosOutput
                                      {
                                          PosId = u.SysPos.Id,
                                          PosCode = u.SysPos.Code,
                                          PosName = u.SysPos.Name
                                      }).ToListAsync();
        }

        /// <summary>
        /// 根据职位Id判断该职位下是否有员工
        /// </summary>
        /// <param name="posId"></param>
        /// <returns></returns>
        public async Task<bool> HasPosEmp(long posId)
        {
            return await _sysEmpPosRep.DetachedEntities.AnyAsync(u => u.SysPosId == posId);
        }

        /// <summary>
        /// 根据员工Id删除对用的员工-职位信息
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task DeleteEmpPosInfoByUserId(long empId)
        {
            var empPos = await _sysEmpPosRep.Where(u => u.SysEmpId == empId).ToListAsync();
            await _sysEmpPosRep.DeleteAsync(empPos);
        }
    }
}