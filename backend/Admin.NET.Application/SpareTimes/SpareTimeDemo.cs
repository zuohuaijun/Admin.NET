using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.TaskScheduler;
using System;

namespace Admin.NET.Application
{
    /// <summary>
    /// 定时任务demo
    /// </summary>
    public class SpareTimeDemo : ISpareTimeWorker
    {
        /// <summary>
        /// 3秒后出勤统计
        /// </summary>
        /// <param name="timer">参数</param>
        /// <param name="count">次数</param>
        [SpareTime(3000, "执行Sql", DoOnce = true, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
        public void ExecSql(SpareTimer timer, long count)
        {
            //创建作用域
            Scoped.Create((factory, scope) =>
            {
                var start = DateTime.Now;
                Console.WriteLine(start.ToString("yyyy-MM-dd HH:mm:ss") + ":任务开始-----------");
                var services = scope.ServiceProvider;
                // 数据库操作
                var db = Db.GetSqlRepository(services);
                if (!string.IsNullOrEmpty(timer.Description)) //假设 后台任务 中把 sql 语句写到了备注里面
                    db.SqlNonQuery(timer.Description);
                var end = DateTime.Now;
                Console.WriteLine(end.ToString("yyyy-MM-dd HH:mm:ss") + ":任务结束-----------");
                Console.WriteLine($"SQL执行了：{count} 次,耗时：{(end - start).TotalMilliseconds}ms");
            });
        }
    }
}
