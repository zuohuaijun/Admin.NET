using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 系统行政区域服务
    /// </summary>
    [ApiDescriptionSettings(Name = "行政区域", Order = 201)]
    public class SysDistrictService : IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysDistrict> _districtRep;

        public SysDistrictService(SqlSugarRepository<SysDistrict> districtRep,
            ISysCacheService sysCacheService)
        {
            _districtRep = districtRep;
        }

        /// <summary>
        /// 获取行政区域列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysDistrict/list")]
        public async Task<List<SysDistrict>> GetDistrictList([FromQuery] DistrictInput input)
        {
            var idList = input.Id > 0 ? await GetChildIdListWithSelfById(input.Id) : new List<long>();

            var iSugarQueryable = _districtRep.AsQueryable().OrderBy(u => u.Order)
                .WhereIF(idList.Count > 0, u => idList.Contains(u.Id)); // 非超级管理员限制

            if (!string.IsNullOrWhiteSpace(input.Name) || !string.IsNullOrWhiteSpace(input.Code) || !string.IsNullOrWhiteSpace(input.Value) || input.Id > 0)
            {
                return await iSugarQueryable
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Value), u => u.Value.Contains(input.Value))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code.Contains(input.Code))
                    .ToListAsync();
            }
            return await iSugarQueryable.ToTreeAsync(u => u.Children, u => u.Pid, input.Id > 0 ? input.Id : 0);
        }

        /// <summary>
        /// 增加行政区域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysDistrict/add")]
        public async Task<long> AddDistrict(AddDistrictInput input)
        {
            var isExist = await _districtRep.IsAnyAsync(u => u.Code == input.Code && u.Name == input.Name);
            if (isExist)
                throw Oops.Oh(ErrorCodeEnum.D1602);

            var sysDistrict = input.Adapt<SysDistrict>();
            var newDistrict = await _districtRep.AsInsertable(sysDistrict).ExecuteReturnEntityAsync();
            return newDistrict.Id;
        }

        /// <summary>
        /// 更新行政区域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysDistrict/update")]
        [SqlSugarUnitOfWork]
        public async Task UpdateDistrict(UpdateDistrictInput input)
        {
            if (input.Pid != 0)
            {
                var pDistrict = await _districtRep.GetFirstAsync(u => u.Id == input.Pid);
                _ = pDistrict ?? throw Oops.Oh(ErrorCodeEnum.D1600);
            }
            if (input.Id == input.Pid)
                throw Oops.Oh(ErrorCodeEnum.D1601);

            var sysDistrict = await _districtRep.GetFirstAsync(u => u.Id == input.Id);
            var isExist = await _districtRep.IsAnyAsync(u => (u.Name == input.Name && u.Code == input.Code) && u.Id != sysDistrict.Id);
            if (isExist)
                throw Oops.Oh(ErrorCodeEnum.D1602);

            // 父Id不能为自己的子节点
            var childIdList = await GetChildIdListWithSelfById(input.Id);
            if (childIdList.Contains(input.Pid))
                throw Oops.Oh(ErrorCodeEnum.D1601);

            var district = input.Adapt<SysDistrict>();
            await _districtRep.AsUpdateable(district).IgnoreColumns(true).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除行政区域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysDistrict/delete")]
        public async Task DeleteDistrict(DeleteDistrictInput input)
        {
            // 获取本节点对应所有子节点id列表
            var treeList = await _districtRep.AsQueryable().ToChildListAsync(u => u.Pid, input.Id);
            var idList = treeList.Select(u => u.Id).ToList();

            // 级联删除行政区域子节点
            await _districtRep.DeleteAsync(u => idList.Contains(u.Id));
        }

        /// <summary>
        /// 根据节点Id获取子节点Id集合(包含自己)
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<long>> GetChildIdListWithSelfById(long pid)
        {
            var treeList = await _districtRep.AsQueryable().ToChildListAsync(u => u.Pid, pid);
            return treeList.Select(u => u.Id).ToList();
        }
    }
}
