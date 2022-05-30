using SqlSugar;

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

    }
}
