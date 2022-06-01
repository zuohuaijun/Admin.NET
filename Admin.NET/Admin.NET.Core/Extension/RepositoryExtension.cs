using Furion;
using Furion.FriendlyException;
using Mapster;
using MapsterMapper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Extension
{
    public static class RepositoryExtension
    {
        /// <summary>
        /// 实体假删除 _rep.Context.Updateable(entity).FakeDelete().ExecuteCommandAsync();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateable"></param>
        /// <returns></returns>
        public static IUpdateable<T> FakeDelete<T>(this IUpdateable<T> updateable) where T : EntityBase, new()
        {
            return updateable.ReSetValue(x => { x.IsDelete = true; })
                .IgnoreColumns(ignoreAllNullColumns: true)
                .EnableDiffLogEvent()   // 记录差异日志
                .UpdateColumns(x => new { x.IsDelete, x.UpdateTime, x.UpdateUserId });  // 允许更新的字段-AOP拦截自动设置UpdateTime、UpdateUserId
        }

        /// <summary>
        /// 排序方式(默认降序)
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="pageInput"> </param>
        /// <param name="defualtSortField"> 默认排序字段 </param>
        /// <param name="descSort"> 是否降序 </param>
        /// <returns> </returns>
        public static ISugarQueryable<T> OrderBuilder<T>(this ISugarQueryable<T> queryable, BasePageInput pageInput, string defualtSortField = "Id", bool descSort = true)
        {
            var orderStr = "";
            // 约定默认每张表都有Id排序
            if (!string.IsNullOrWhiteSpace(defualtSortField))
            {
                orderStr = descSort ? defualtSortField + " Desc" : defualtSortField + " Asc";
            }
            TypeAdapterConfig config = new();
            config.ForType<T, BasePageInput>().IgnoreNullValues(true);
            Mapper mapper = new(config); // 务必将mapper设为单实例
            var nowPagerInput = mapper.Map<BasePageInput>(pageInput);
            // 排序是否可用-排序字段和排序顺序都为非空才启用排序
            if (!string.IsNullOrEmpty(nowPagerInput.Field) && !string.IsNullOrEmpty(nowPagerInput.Order))
            {
                orderStr = $"{nowPagerInput.Field} {(nowPagerInput.Order == nowPagerInput.DescStr ? "Desc" : "Asc")}";
            }
            return queryable.OrderByIF(!string.IsNullOrWhiteSpace(orderStr), orderStr);
        }
    }
}
