using Furion;
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
        /// 根据已有实体假删除 _rep.Context.Updateable(entity).FakeDelete().ExecuteCommandAsync();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateable"></param>
        /// <returns></returns>
        public static IUpdateable<T> FakeDelete<T>(this IUpdateable<T> updateable) where T : EntityBase, new()
        {
            return updateable.ReSetValue(x => { x.IsDelete = true; })
                .IgnoreColumns(ignoreAllNullColumns: true)
                .EnableDiffLogEvent()   //记录差异日志
                .UpdateColumns(x => new { x.IsDelete, x.UpdateTime, x.UpdateUserId });  //允许更新的字段, AOP拦截会自动设置UpdateTime，UpdateUserId
        }
        
    }
}
