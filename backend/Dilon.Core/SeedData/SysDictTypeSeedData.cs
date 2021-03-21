using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 系统字典类型种子数据
    /// </summary>
    public class SysDictTypeSeedData : IEntitySeedData<SysDictType>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysDictType> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysDictType{Id=1, Name="通用状态", Code="common_status", Sort=100, Remark="通用状态", Status=0 },
                new SysDictType{Id=2, Name="性别", Code="sex", Sort=100, Remark="性别字典", Status=0 },
                new SysDictType{Id=3, Name="常量的分类", Code="consts_type", Sort=100, Remark="常量的分类，用于区别一组配置", Status=0 },
                new SysDictType{Id=4, Name="是否", Code="yes_or_no", Sort=100, Remark="是否", Status=0 },
                new SysDictType{Id=5, Name="访问类型", Code="vis_type", Sort=100, Remark="访问类型", Status=0 },
                new SysDictType{Id=6, Name="菜单类型", Code="menu_type", Sort=100, Remark="菜单类型", Status=0 },
                new SysDictType{Id=7, Name="发送类型", Code="send_type", Sort=100, Remark="发送类型", Status=0 },
                new SysDictType{Id=8, Name="打开方式", Code="open_type", Sort=100, Remark="打开方式", Status=0 },
                new SysDictType{Id=9, Name="菜单权重", Code="menu_weight", Sort=100, Remark="菜单权重", Status=0 },
                new SysDictType{Id=10, Name="数据范围类型", Code="data_scope_type", Sort=100, Remark="数据范围类型", Status=0 },
                new SysDictType{Id=11, Name="短信发送来源", Code="sms_send_source", Sort=100, Remark="短信发送来源", Status=0 },
                new SysDictType{Id=12, Name="操作类型", Code="op_type", Sort=100, Remark="操作类型", Status=0 },
                new SysDictType{Id=13, Name="文件存储位置", Code="file_storage_location", Sort=100, Remark="文件存储位置", Status=0 },
                new SysDictType{Id=14, Name="运行状态", Code="run_status", Sort=100, Remark="定时任务运行状态", Status=0 },
                new SysDictType{Id=15, Name="通知公告类型", Code="notice_type", Sort=100, Remark="通知公告类型", Status=0 },
                new SysDictType{Id=16, Name="通知公告状态", Code="notice_status", Sort=100, Remark="通知公告状态", Status=0 },
                new SysDictType{Id=17, Name="是否boolean", Code="yes_true_false", Sort=100, Remark="是否boolean", Status=0 },
                new SysDictType{Id=18, Name="代码生成方式", Code="code_gen_create_type", Sort=100, Remark="代码生成方式", Status=0 },
                new SysDictType{Id=19, Name="请求方式", Code="request_type", Sort=100, Remark="请求方式", Status=0 }
            };
        }
    }
}
