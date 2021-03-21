using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 系统字典值种子数据
    /// </summary>
    public class SysDictDataSeedData : IEntitySeedData<SysDictData>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysDictData> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysDictData{Id=1, TypeId=2, Value="男", Code="1", Sort=100, Remark="男性", Status=0 },
                new SysDictData{Id=2, TypeId=2, Value="女", Code="2", Sort=100, Remark="女性", Status=0 },
                new SysDictData{Id=3, TypeId=2, Value="未知", Code="3", Sort=100, Remark="未知性别", Status=0 },
                new SysDictData{Id=4, TypeId=3, Value="默认常量", Code="DEFAULT", Sort=100, Remark="默认常量，都以XIAONUO_开头的", Status=0 },
                new SysDictData{Id=5, TypeId=3, Value="阿里云短信", Code="ALIYUN_SMS", Sort=100, Remark="阿里云短信配置", Status=0 },
                new SysDictData{Id=6, TypeId=3, Value="腾讯云短信", Code="TENCENT_SMS", Sort=100, Remark="腾讯云短信", Status=0 },
                new SysDictData{Id=7, TypeId=3, Value="邮件配置", Code="EMAIL", Sort=100, Remark="邮件配置", Status=0 },
                new SysDictData{Id=8, TypeId=3, Value="文件上传路径", Code="FILE_PATH", Sort=100, Remark="文件上传路径", Status=0 },
                new SysDictData{Id=9, TypeId=3, Value="Oauth配置", Code="OAUTH", Sort=100, Remark="Oauth配置", Status=0 },
                new SysDictData{Id=10, TypeId=1, Value="正常", Code="0", Sort=100, Remark="正常", Status=0 },
                new SysDictData{Id=11, TypeId=1, Value="停用", Code="1", Sort=100, Remark="停用", Status=0 },
                new SysDictData{Id=12, TypeId=1, Value="删除", Code="2", Sort=100, Remark="删除", Status=0 },
                new SysDictData{Id=13, TypeId=4, Value="否", Code="N", Sort=100, Remark="否", Status=0 },
                new SysDictData{Id=14, TypeId=4, Value="是", Code="Y", Sort=100, Remark="是", Status=0 },
                new SysDictData{Id=15, TypeId=5, Value="登录", Code="1", Sort=100, Remark="登录", Status=0 },
                new SysDictData{Id=16, TypeId=5, Value="登出", Code="2", Sort=100, Remark="登出", Status=0 },
                new SysDictData{Id=17, TypeId=6, Value="目录", Code="0", Sort=100, Remark="目录", Status=0 },
                new SysDictData{Id=18, TypeId=6, Value="菜单", Code="1", Sort=100, Remark="菜单", Status=0 },
                new SysDictData{Id=19, TypeId=6, Value="按钮", Code="2", Sort=100, Remark="按钮", Status=0 },
                new SysDictData{Id=20, TypeId=7, Value="未发送", Code="0", Sort=100, Remark="未发送", Status=0 },
                new SysDictData{Id=21, TypeId=7, Value="发送成功", Code="1", Sort=100, Remark="发送成功", Status=0 },
                new SysDictData{Id=22, TypeId=7, Value="发送失败", Code="2", Sort=100, Remark="发送失败", Status=0 },
                new SysDictData{Id=23, TypeId=7, Value="失效", Code="3", Sort=100, Remark="失效", Status=0 },
                new SysDictData{Id=24, TypeId=8, Value="无", Code="0", Sort=100, Remark="无", Status=0 },
                new SysDictData{Id=25, TypeId=8, Value="组件", Code="1", Sort=100, Remark="组件", Status=0 },
                new SysDictData{Id=26, TypeId=8, Value="内链", Code="2", Sort=100, Remark="内链", Status=0 },
                new SysDictData{Id=27, TypeId=8, Value="外链", Code="3", Sort=100, Remark="外链", Status=0 },
                new SysDictData{Id=28, TypeId=9, Value="系统权重", Code="1", Sort=100, Remark="系统权重", Status=0 },
                new SysDictData{Id=29, TypeId=9, Value="业务权重", Code="2", Sort=100, Remark="业务权重", Status=0 },
                new SysDictData{Id=30, TypeId=10, Value="全部数据", Code="1", Sort=100, Remark="全部数据", Status=0 },
                new SysDictData{Id=31, TypeId=10, Value="本部门及以下数据", Code="2", Sort=100, Remark="本部门及以下数据", Status=0 },
                new SysDictData{Id=32, TypeId=10, Value="本部门数据", Code="3", Sort=100, Remark="本部门数据", Status=0 },
                new SysDictData{Id=33, TypeId=10, Value="仅本人数据", Code="4", Sort=100, Remark="仅本人数据", Status=0 },
                new SysDictData{Id=34, TypeId=10, Value="自定义数据", Code="5", Sort=100, Remark="自定义数据", Status=0 },
                new SysDictData{Id=35, TypeId=11, Value="app", Code="1", Sort=100, Remark="app", Status=0 },
                new SysDictData{Id=36, TypeId=11, Value="pc", Code="2", Sort=100, Remark="pc", Status=0 },
                new SysDictData{Id=37, TypeId=11, Value="其他", Code="3", Sort=100, Remark="其他", Status=0 },
                new SysDictData{Id=38, TypeId=12, Value="其它", Code="0", Sort=100, Remark="其它", Status=0 },
                new SysDictData{Id=39, TypeId=12, Value="增加", Code="1", Sort=100, Remark="增加", Status=0 },
                new SysDictData{Id=40, TypeId=12, Value="删除", Code="2", Sort=100, Remark="删除", Status=0 },
                new SysDictData{Id=41, TypeId=12, Value="编辑", Code="3", Sort=100, Remark="编辑", Status=0 },
                new SysDictData{Id=42, TypeId=12, Value="更新", Code="4", Sort=100, Remark="更新", Status=0 },
                new SysDictData{Id=43, TypeId=12, Value="查询", Code="5", Sort=100, Remark="查询", Status=0 },
                new SysDictData{Id=44, TypeId=12, Value="详情", Code="6", Sort=100, Remark="详情", Status=0 },
                new SysDictData{Id=45, TypeId=12, Value="树", Code="7", Sort=100, Remark="树", Status=0 },
                new SysDictData{Id=46, TypeId=12, Value="导入", Code="8", Sort=100, Remark="导入", Status=0 },
                new SysDictData{Id=47, TypeId=12, Value="导出", Code="9", Sort=100, Remark="导出", Status=0 },
                new SysDictData{Id=48, TypeId=12, Value="授权", Code="10", Sort=100, Remark="授权", Status=0 },
                new SysDictData{Id=49, TypeId=12, Value="强退", Code="11", Sort=100, Remark="强退", Status=0 },
                new SysDictData{Id=50, TypeId=12, Value="清空", Code="12", Sort=100, Remark="清空", Status=0 },
                new SysDictData{Id=51, TypeId=12, Value="修改状态", Code="13", Sort=100, Remark="修改状态", Status=0 },
                new SysDictData{Id=52, TypeId=13, Value="阿里云", Code="1", Sort=100, Remark="阿里云", Status=0 },
                new SysDictData{Id=53, TypeId=13, Value="腾讯云", Code="2", Sort=100, Remark="腾讯云", Status=0 },
                new SysDictData{Id=54, TypeId=13, Value="minio", Code="3", Sort=100, Remark="minio", Status=0 },
                new SysDictData{Id=55, TypeId=13, Value="本地", Code="4", Sort=100, Remark="本地", Status=0 },
                new SysDictData{Id=56, TypeId=14, Value="运行", Code="1", Sort=100, Remark="运行", Status=0 },
                new SysDictData{Id=57, TypeId=14, Value="停止", Code="2", Sort=100, Remark="停止", Status=0 },
                new SysDictData{Id=58, TypeId=15, Value="通知", Code="1", Sort=100, Remark="通知", Status=0 },
                new SysDictData{Id=59, TypeId=15, Value="公告", Code="2", Sort=100, Remark="公告", Status=0 },
                new SysDictData{Id=60, TypeId=16, Value="草稿", Code="0", Sort=100, Remark="草稿", Status=0 },
                new SysDictData{Id=61, TypeId=16, Value="发布", Code="1", Sort=100, Remark="发布", Status=0 },
                new SysDictData{Id=62, TypeId=16, Value="撤回", Code="2", Sort=100, Remark="撤回", Status=0 },
                new SysDictData{Id=63, TypeId=16, Value="删除", Code="3", Sort=100, Remark="删除", Status=0 },
                new SysDictData{Id=64, TypeId=17, Value="是", Code="true", Sort=100, Remark="是", Status=0 },
                new SysDictData{Id=65, TypeId=17, Value="否", Code="false", Sort=100, Remark="否", Status=0 },
                new SysDictData{Id=66, TypeId=18, Value="下载压缩包", Code="1", Sort=100, Remark="下载压缩包", Status=0 },
                new SysDictData{Id=67, TypeId=18, Value="生成到本项目", Code="2", Sort=100, Remark="生成到本项目", Status=0 },
                new SysDictData{Id=68, TypeId=19, Value="GET", Code="1", Sort=100, Remark="GET", Status=0 },
                new SysDictData{Id=69, TypeId=19, Value="POST", Code="2", Sort=100, Remark="POST", Status=0 },
                new SysDictData{Id=70, TypeId=19, Value="PUT", Code="3", Sort=100, Remark="PUT", Status=0 },
                new SysDictData{Id=71, TypeId=19, Value="DELETE", Code="4", Sort=100, Remark="DELETE", Status=0 }
            };
        }
    }
}
