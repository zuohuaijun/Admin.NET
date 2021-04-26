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
                new SysDictType{Id=142307070906483, Name="通用状态", Code="common_status", Sort=100, Remark="通用状态", Status=0 },
                new SysDictType{Id=142307070906484, Name="性别", Code="sex", Sort=100, Remark="性别字典", Status=0 },
                new SysDictType{Id=142307070906485, Name="常量的分类", Code="consts_type", Sort=100, Remark="常量的分类，用于区别一组配置", Status=0 },
                new SysDictType{Id=142307070906486, Name="是否", Code="yes_or_no", Sort=100, Remark="是否", Status=0 },
                new SysDictType{Id=142307070906487, Name="访问类型", Code="vis_type", Sort=100, Remark="访问类型", Status=0 },
                new SysDictType{Id=142307070906488, Name="菜单类型", Code="menu_type", Sort=100, Remark="菜单类型", Status=0 },
                new SysDictType{Id=142307070906489, Name="发送类型", Code="send_type", Sort=100, Remark="发送类型", Status=0 },
                new SysDictType{Id=142307070906490, Name="打开方式", Code="open_type", Sort=100, Remark="打开方式", Status=0 },
                new SysDictType{Id=142307070906491, Name="菜单权重", Code="menu_weight", Sort=100, Remark="菜单权重", Status=0 },
                new SysDictType{Id=142307070906492, Name="数据范围类型", Code="data_scope_type", Sort=100, Remark="数据范围类型", Status=0 },
                new SysDictType{Id=142307070906493, Name="短信发送来源", Code="sms_send_source", Sort=100, Remark="短信发送来源", Status=0 },
                new SysDictType{Id=142307070906494, Name="操作类型", Code="op_type", Sort=100, Remark="操作类型", Status=0 },
                new SysDictType{Id=142307070906495, Name="文件存储位置", Code="file_storage_location", Sort=100, Remark="文件存储位置", Status=0 },
                new SysDictType{Id=142307070910533, Name="运行状态", Code="run_status", Sort=100, Remark="定时任务运行状态", Status=0 },
                new SysDictType{Id=142307070910534, Name="通知公告类型", Code="notice_type", Sort=100, Remark="通知公告类型", Status=0 },
                new SysDictType{Id=142307070910535, Name="通知公告状态", Code="notice_status", Sort=100, Remark="通知公告状态", Status=0 },
                new SysDictType{Id=142307070910536, Name="是否boolean", Code="yes_true_false", Sort=100, Remark="是否boolean", Status=0 },
                new SysDictType{Id=142307070910537, Name="代码生成方式", Code="code_gen_create_type", Sort=100, Remark="代码生成方式", Status=0 },
                new SysDictType{Id=142307070910538, Name="请求方式", Code="request_type", Sort=100, Remark="请求方式", Status=0 },
                new SysDictType{Id=142307070922827, Name="代码生成作用类型", Code="code_gen_effect_type", Sort=100, Remark="代码生成作用类型", Status=0 },
                new SysDictType{Id=142307070922828, Name="代码生成查询类型", Code="code_gen_query_type", Sort=100, Remark="代码生成查询类型", Status=0 },
                new SysDictType{Id=142307070922829, Name="代码生成.NET类型", Code="code_gen_net_type", Sort=100, Remark="代码生成.NET类型", Status=0 },
            };
        }
    }
}