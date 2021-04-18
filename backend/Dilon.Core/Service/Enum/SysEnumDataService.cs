using System.Linq;
using System.Threading.Tasks;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 枚举值服务
    /// </summary>
    [ApiDescriptionSettings(Name = "EnumData")]
    public class SysEnumDataService : ISysEnumDataService, IDynamicApiController, ITransient
    {
        /// <summary>
        /// 通过枚举类型获取枚举值集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysEnumData/list")]
        public async Task<dynamic> GetEnumDataList([FromQuery] QueryEnumDataListInput input)
        {
            // 查找枚举
            var enumType = App.EffectiveTypes.FirstOrDefault(t => t.IsEnum && t.Name == input.EnumName);
            if (enumType == null)
                throw Oops.Oh(ErrorCode.D1502).StatusCode(405);

            // 获取枚举的key和描述
            return await Task.Run(() =>
                EnumExtensions.GetEnumDescDictionary(enumType)
                    .Select(x =>
                        new EnumDataOutput
                        {
                            Code = x.Key.ToString(),
                            Value = x.Value
                        }));
        }

        /// <summary>
        /// 通过实体字段类型获取相关集合（目前仅支持枚举类型）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysEnumData/listByFiled")]
        public async Task<dynamic> GetEnumDataListByField([FromQuery] QueryEnumDataListByFiledInput input)
        {
            // 获取实体类型属性
            var entityType = Db.GetDbContext().Model.GetEntityTypes()
                .FirstOrDefault(u => u.ClrType.Name == input.EntityName);
            if (entityType == null) throw Oops.Oh(ErrorCode.D1504);

            // 获取字段类型
            var fieldType = entityType.GetProperties().FirstOrDefault(p => p.Name == input.FieldName)?.ClrType;
            if (fieldType is not { IsEnum: true })
                throw Oops.Oh(ErrorCode.D1503);

            // 获取枚举的key和描述
            return await Task.Run(() =>
                EnumExtensions.GetEnumDescDictionary(fieldType)
                    .Select(x =>
                        new EnumDataOutput
                        {
                            Code = x.Key.ToString(),
                            Value = x.Value
                        }));
        }
    }
}