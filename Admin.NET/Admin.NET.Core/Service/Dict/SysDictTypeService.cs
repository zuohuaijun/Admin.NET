using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 系统字典类型服务
    /// </summary>
    [ApiDescriptionSettings(Name = "系统字典", Order = 192)]
    [AllowAnonymous]
    public class SysDictTypeService : IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysDictType> _sysDictTypeRep;
        private readonly SysDictDataService _sysDictDataService;

        public SysDictTypeService(SqlSugarRepository<SysDictType> sysDictTypeRep,
            SysDictDataService sysDictDataService)
        {
            _sysDictTypeRep = sysDictTypeRep;
            _sysDictDataService = sysDictDataService;
        }

        /// <summary>
        /// 获取字典类型分页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysDictType/pageList")]
        public async Task<SqlSugarPagedList<SysDictType>> GetDictTypePageList([FromQuery] PageDictTypeInput input)
        {
            var code = !string.IsNullOrEmpty(input.Code?.Trim());
            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            return await _sysDictTypeRep.AsQueryable()
                .WhereIF(code, u => u.Code.Contains(input.Code))
                .WhereIF(name, u => u.Name.Contains(input.Name))
                .OrderBy(u => u.Order)
                .ToPagedListAsync(input.Page, input.PageSize);
        }

        /// <summary>
        /// 获取字典类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysDictType/list")]
        public async Task<List<SysDictType>> GetDictTypeList()
        {
            return await _sysDictTypeRep.AsQueryable().OrderBy(u => u.Order).ToListAsync();
        }

        /// <summary>
        /// 获取字典类型-值列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("/sysDictType/dataList")]
        public async Task<List<SysDictData>> GetDictTypeDataList([FromQuery] GetDataDictTypeInput input)
        {
            var dictType = await _sysDictTypeRep.GetFirstAsync(u => u.Code == input.Code);
            if (dictType == null)
                throw Oops.Oh(ErrorCodeEnum.D3000);
            return await _sysDictDataService.GetDictDataListByDictTypeId(dictType.Id);
        }

        /// <summary>
        /// 添加字典类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysDictType/add")]
        public async Task AddDictType(AddDictTypeInput input)
        {
            var isExist = await _sysDictTypeRep.IsAnyAsync(u => u.Name == input.Name || u.Code == input.Code);
            if (isExist)
                throw Oops.Oh(ErrorCodeEnum.D3001);

            var dictType = input.Adapt<SysDictType>();
            await _sysDictTypeRep.InsertAsync(dictType);
        }

        /// <summary>
        /// 更新字典类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysDictType/update"),]
        public async Task UpdateDictType(UpdateDictTypeInput input)
        {
            var isExist = await _sysDictTypeRep.IsAnyAsync(u => u.Id == input.Id);
            if (!isExist)
                throw Oops.Oh(ErrorCodeEnum.D3000);

            isExist = await _sysDictTypeRep.IsAnyAsync(u => (u.Name == input.Name || u.Code == input.Code) && u.Id != input.Id);
            if (isExist)
                throw Oops.Oh(ErrorCodeEnum.D3001);

            var dictType = input.Adapt<SysDictType>();
            await _sysDictTypeRep.UpdateAsync(dictType);
        }

        /// <summary>
        /// 获取字典类型详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysDictType/detail")]
        public async Task<SysDictType> GetDictType([FromQuery] DictTypeInput input)
        {
            return await _sysDictTypeRep.GetFirstAsync(u => u.Id == input.Id);
        }

        /// <summary>
        /// 删除字典类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysDictType/delete")]
        public async Task DeleteDictType(DeleteDictTypeInput input)
        {
            var dictType = await _sysDictTypeRep.GetFirstAsync(u => u.Id == input.Id);
            if (dictType == null)
                throw Oops.Oh(ErrorCodeEnum.D3000);

            await _sysDictTypeRep.DeleteAsync(dictType);
        }

        /// <summary>
        /// 修改字典类型状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysDictType/changeStatus")]
        public async Task ChangeDictTypeStatus(ChangeStatusDictTypeInput input)
        {
            var dictType = await _sysDictTypeRep.GetFirstAsync(u => u.Id == input.Id);
            if (dictType == null)
                throw Oops.Oh(ErrorCodeEnum.D3000);

            if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
                throw Oops.Oh(ErrorCodeEnum.D3005);

            dictType.Status = (StatusEnum)input.Status;
            await _sysDictTypeRep.UpdateAsync(dictType);
        }
    }
}