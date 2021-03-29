using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 字典值服务
    /// </summary>
    [ApiDescriptionSettings(Name = "DictData", Order = 100)]
    public class SysDictDataService : ISysDictDataService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysDictData> _sysDictDataRep;  // 字典类型表仓储 

        public SysDictDataService(IRepository<SysDictData> sysDictDataRep)
        {
            _sysDictDataRep = sysDictDataRep;
        }

        /// <summary>
        /// 分页查询字典值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysDictData/page")]
        public async Task<dynamic> QueryDictDataPageList([FromQuery] DictDataInput input)
        {
            var code = !string.IsNullOrEmpty(input.Code?.Trim());
            var value = !string.IsNullOrEmpty(input.Value?.Trim());
            var dictDatas = await _sysDictDataRep.DetachedEntities
                                  .Where(u => u.TypeId == input.TypeId)
                                  .Where((code, u => EF.Functions.Like(u.Code, $"%{input.Code.Trim()}%")),
                                         (value, u => EF.Functions.Like(u.Value, $"%{input.Value.Trim()}%")))
                                  .Where(u => u.Status != CommonStatus.DELETED).OrderBy(u => u.Sort)
                                  .Select(u => u.Adapt<DictDataOutput>())
                                  .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<DictDataOutput>.PageResult(dictDatas);
        }

        /// <summary>
        /// 获取某个字典类型下字典值列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysDictData/list")]
        public async Task<dynamic> GetDictDataList([FromQuery] QueryDictDataListInput input)
        {
            return await _sysDictDataRep.DetachedEntities.Where(u => u.TypeId == input.TypeId).Where(u => u.Status != CommonStatus.DELETED).OrderBy(u => u.Sort).ToListAsync();
        }

        /// <summary>
        /// 增加字典值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysDictData/add")]
        public async Task AddDictData(AddDictDataInput input)
        {
            var isExist = await _sysDictDataRep.AnyAsync(u => (u.Code == input.Code || u.Value == input.Value) && u.TypeId == input.TypeId, false);
            if (isExist) throw Oops.Oh(ErrorCode.D3003);

            var dictData = input.Adapt<SysDictData>();
            await _sysDictDataRep.InsertAsync(dictData);
        }

        /// <summary>
        /// 删除字典值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysDictData/delete")]
        public async Task DeleteDictData(DeleteDictDataInput input)
        {
            var dictData = await _sysDictDataRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (dictData == null) throw Oops.Oh(ErrorCode.D3004);

            await dictData.DeleteAsync();
        }

        /// <summary>
        /// 更新字典值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysDictData/edit")]
        public async Task UpdateDictData(UpdateDictDataInput input)
        {
            var isExist = await _sysDictDataRep.AnyAsync(u => u.Id == input.Id, false);
            if (!isExist) throw Oops.Oh(ErrorCode.D3004);

            // 排除自己并且判断与其他是否相同
            isExist = await _sysDictDataRep.AnyAsync(u => (u.Value == input.Value || u.Code == input.Code) && u.TypeId == input.TypeId && u.Id != input.Id, false);
            if (isExist) throw Oops.Oh(ErrorCode.D3003);

            var dictData = input.Adapt<SysDictData>();
            await _sysDictDataRep.UpdateAsync(dictData, ignoreNullValues: true);
        }

        /// <summary>
        /// 字典值详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysDictData/detail")]
        public async Task<dynamic> GetDictData([FromQuery] QueryDictDataInput input)
        {
            return await _sysDictDataRep.FirstOrDefaultAsync(u => u.Id == input.Id, false);
        }

        /// <summary>
        /// 修改字典值状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysDictData/changeStatus")]
        public async Task ChangeDictDataStatus(UpdateDictDataInput input)
        {
            var dictData = await _sysDictDataRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (dictData == null) throw Oops.Oh(ErrorCode.D3004);

            if (!Enum.IsDefined(typeof(CommonStatus), input.Status))
                throw Oops.Oh(ErrorCode.D3005);
            dictData.Status = input.Status;
        }

        /// <summary>
        /// 根据字典类型Id获取字典值集合
        /// </summary>
        /// <param name="dictTypeId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<dynamic> GetDictDataListByDictTypeId(long dictTypeId)
        {
            return await _sysDictDataRep.DetachedEntities.Where(u => u.SysDictType.Id == dictTypeId)
                                                         .Where(u => u.Status != CommonStatus.DELETED).OrderBy(u => u.Sort)
                                                         .Select(u => new
                                                         {
                                                             u.Code,
                                                             u.Value
                                                         }).ToListAsync();
        }

        /// <summary>
        /// 删除字典下所有值
        /// </summary>
        /// <param name="dictTypeId"></param>
        [NonAction]
        public async Task DeleteByTypeId(long dictTypeId)
        {
            var dictDatas = await _sysDictDataRep.Where(u => u.TypeId == dictTypeId).ToListAsync();
            dictDatas.ForEach(u =>
            {
                u.Delete();
            });
        }
    }
}
