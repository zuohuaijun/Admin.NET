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
                new SysDictData{Id=142307070902375, TypeId=142307070906484, Value="男", Code="1", Sort=100, Remark="男性", Status=0 },
                new SysDictData{Id=142307070902376, TypeId=142307070906484, Value="女", Code="2", Sort=100, Remark="女性", Status=0 },
                new SysDictData{Id=142307070902377, TypeId=142307070906484, Value="未知", Code="3", Sort=100, Remark="未知性别", Status=0 },
                new SysDictData{Id=142307070902378, TypeId=142307070906485, Value="默认常量", Code="DEFAULT", Sort=100, Remark="默认常量，都以XIAONUO_开头的", Status=0 },
                new SysDictData{Id=142307070902379, TypeId=142307070906485, Value="阿里云短信", Code="ALIYUN_SMS", Sort=100, Remark="阿里云短信配置", Status=0 },
                new SysDictData{Id=142307070902380, TypeId=142307070906485, Value="腾讯云短信", Code="TENCENT_SMS", Sort=100, Remark="腾讯云短信", Status=0 },
                new SysDictData{Id=142307070902381, TypeId=142307070906485, Value="邮件配置", Code="EMAIL", Sort=100, Remark="邮件配置", Status=0 },
                new SysDictData{Id=142307070902382, TypeId=142307070906485, Value="文件上传路径", Code="FILE_PATH", Sort=100, Remark="文件上传路径", Status=0 },
                new SysDictData{Id=142307070902383, TypeId=142307070906485, Value="Oauth配置", Code="OAUTH", Sort=100, Remark="Oauth配置", Status=0 },
                new SysDictData{Id=142307070902384, TypeId=142307070906483, Value="正常", Code="0", Sort=100, Remark="正常", Status=0 },
                new SysDictData{Id=142307070902385, TypeId=142307070906483, Value="停用", Code="1", Sort=100, Remark="停用", Status=0 },
                new SysDictData{Id=142307070902386, TypeId=142307070906483, Value="删除", Code="2", Sort=100, Remark="删除", Status=0 },
                new SysDictData{Id=142307070902387, TypeId=142307070906486, Value="否", Code="N", Sort=100, Remark="否", Status=0 },
                new SysDictData{Id=142307070902388, TypeId=142307070906486, Value="是", Code="Y", Sort=100, Remark="是", Status=0 },
                new SysDictData{Id=142307070902389, TypeId=142307070906487, Value="登录", Code="1", Sort=100, Remark="登录", Status=0 },
                new SysDictData{Id=142307070902390, TypeId=142307070906487, Value="登出", Code="2", Sort=100, Remark="登出", Status=0 },
                new SysDictData{Id=142307070902391, TypeId=142307070906488, Value="目录", Code="0", Sort=100, Remark="目录", Status=0 },
                new SysDictData{Id=142307070902392, TypeId=142307070906488, Value="菜单", Code="1", Sort=100, Remark="菜单", Status=0 },
                new SysDictData{Id=142307070902393, TypeId=142307070906488, Value="按钮", Code="2", Sort=100, Remark="按钮", Status=0 },
                new SysDictData{Id=142307070902394, TypeId=142307070906489, Value="未发送", Code="0", Sort=100, Remark="未发送", Status=0 },
                new SysDictData{Id=142307070902395, TypeId=142307070906489, Value="发送成功", Code="1", Sort=100, Remark="发送成功", Status=0 },
                new SysDictData{Id=142307070902396, TypeId=142307070906489, Value="发送失败", Code="2", Sort=100, Remark="发送失败", Status=0 },
                new SysDictData{Id=142307070902397, TypeId=142307070906489, Value="失效", Code="3", Sort=100, Remark="失效", Status=0 },
                new SysDictData{Id=142307070902398, TypeId=142307070906490, Value="无", Code="0", Sort=100, Remark="无", Status=0 },
                new SysDictData{Id=142307070902399, TypeId=142307070906490, Value="组件", Code="1", Sort=100, Remark="组件", Status=0 },
                new SysDictData{Id=142307070906437, TypeId=142307070906490, Value="内链", Code="2", Sort=100, Remark="内链", Status=0 },
                new SysDictData{Id=142307070906438, TypeId=142307070906490, Value="外链", Code="3", Sort=100, Remark="外链", Status=0 },
                new SysDictData{Id=142307070906439, TypeId=142307070906491, Value="系统权重", Code="1", Sort=100, Remark="系统权重", Status=0 },
                new SysDictData{Id=142307070906440, TypeId=142307070906491, Value="业务权重", Code="2", Sort=100, Remark="业务权重", Status=0 },
                new SysDictData{Id=142307070906441, TypeId=142307070906492, Value="全部数据", Code="1", Sort=100, Remark="全部数据", Status=0 },
                new SysDictData{Id=142307070906442, TypeId=142307070906492, Value="本部门及以下数据", Code="2", Sort=100, Remark="本部门及以下数据", Status=0 },
                new SysDictData{Id=142307070906443, TypeId=142307070906492, Value="本部门数据", Code="3", Sort=100, Remark="本部门数据", Status=0 },
                new SysDictData{Id=142307070906444, TypeId=142307070906492, Value="仅本人数据", Code="4", Sort=100, Remark="仅本人数据", Status=0 },
                new SysDictData{Id=142307070906445, TypeId=142307070906492, Value="自定义数据", Code="5", Sort=100, Remark="自定义数据", Status=0 },
                new SysDictData{Id=142307070906446, TypeId=142307070906493, Value="app", Code="1", Sort=100, Remark="app", Status=0 },
                new SysDictData{Id=142307070906447, TypeId=142307070906493, Value="pc", Code="2", Sort=100, Remark="pc", Status=0 },
                new SysDictData{Id=142307070906448, TypeId=142307070906493, Value="其他", Code="3", Sort=100, Remark="其他", Status=0 },
                new SysDictData{Id=142307070906449, TypeId=142307070906494, Value="其它", Code="0", Sort=100, Remark="其它", Status=0 },
                new SysDictData{Id=142307070906450, TypeId=142307070906494, Value="增加", Code="1", Sort=100, Remark="增加", Status=0 },
                new SysDictData{Id=142307070906451, TypeId=142307070906494, Value="删除", Code="2", Sort=100, Remark="删除", Status=0 },
                new SysDictData{Id=142307070906452, TypeId=142307070906494, Value="编辑", Code="3", Sort=100, Remark="编辑", Status=0 },
                new SysDictData{Id=142307070906453, TypeId=142307070906494, Value="更新", Code="4", Sort=100, Remark="更新", Status=0 },
                new SysDictData{Id=142307070906454, TypeId=142307070906494, Value="查询", Code="5", Sort=100, Remark="查询", Status=0 },
                new SysDictData{Id=142307070906455, TypeId=142307070906494, Value="详情", Code="6", Sort=100, Remark="详情", Status=0 },
                new SysDictData{Id=142307070906456, TypeId=142307070906494, Value="树", Code="7", Sort=100, Remark="树", Status=0 },
                new SysDictData{Id=142307070906457, TypeId=142307070906494, Value="导入", Code="8", Sort=100, Remark="导入", Status=0 },
                new SysDictData{Id=142307070906458, TypeId=142307070906494, Value="导出", Code="9", Sort=100, Remark="导出", Status=0 },
                new SysDictData{Id=142307070906459, TypeId=142307070906494, Value="授权", Code="10", Sort=100, Remark="授权", Status=0 },
                new SysDictData{Id=142307070906460, TypeId=142307070906494, Value="强退", Code="11", Sort=100, Remark="强退", Status=0 },
                new SysDictData{Id=142307070906461, TypeId=142307070906494, Value="清空", Code="12", Sort=100, Remark="清空", Status=0 },
                new SysDictData{Id=142307070906462, TypeId=142307070906494, Value="修改状态", Code="13", Sort=100, Remark="修改状态", Status=0 },
                new SysDictData{Id=142307070906463, TypeId=142307070906495, Value="阿里云", Code="1", Sort=100, Remark="阿里云", Status=0 },
                new SysDictData{Id=142307070906464, TypeId=142307070906495, Value="腾讯云", Code="2", Sort=100, Remark="腾讯云", Status=0 },
                new SysDictData{Id=142307070906465, TypeId=142307070906495, Value="minio", Code="3", Sort=100, Remark="minio", Status=0 },
                new SysDictData{Id=142307070906466, TypeId=142307070906495, Value="本地", Code="4", Sort=100, Remark="本地", Status=0 },
                new SysDictData{Id=142307070906467, TypeId=142307070910533, Value="运行", Code="1", Sort=100, Remark="运行", Status=0 },
                new SysDictData{Id=142307070906468, TypeId=142307070910533, Value="停止", Code="2", Sort=100, Remark="停止", Status=0 },
                new SysDictData{Id=142307070906469, TypeId=142307070910534, Value="通知", Code="1", Sort=100, Remark="通知", Status=0 },
                new SysDictData{Id=142307070906470, TypeId=142307070910534, Value="公告", Code="2", Sort=100, Remark="公告", Status=0 },
                new SysDictData{Id=142307070906471, TypeId=142307070910535, Value="草稿", Code="0", Sort=100, Remark="草稿", Status=0 },
                new SysDictData{Id=142307070906472, TypeId=142307070910535, Value="发布", Code="1", Sort=100, Remark="发布", Status=0 },
                new SysDictData{Id=142307070906473, TypeId=142307070910535, Value="撤回", Code="2", Sort=100, Remark="撤回", Status=0 },
                new SysDictData{Id=142307070906474, TypeId=142307070910535, Value="删除", Code="3", Sort=100, Remark="删除", Status=0 },
                new SysDictData{Id=142307070906475, TypeId=142307070910536, Value="是", Code="true", Sort=100, Remark="是", Status=0 },
                new SysDictData{Id=142307070906476, TypeId=142307070910536, Value="否", Code="false", Sort=100, Remark="否", Status=0 },
                new SysDictData{Id=142307070906477, TypeId=142307070910537, Value="下载压缩包", Code="1", Sort=100, Remark="下载压缩包", Status=0 },
                new SysDictData{Id=142307070906478, TypeId=142307070910537, Value="生成到本项目", Code="2", Sort=100, Remark="生成到本项目", Status=0 },
                new SysDictData{Id=142307070906479, TypeId=142307070910538, Value="GET", Code="1", Sort=100, Remark="GET", Status=0 },
                new SysDictData{Id=142307070906480, TypeId=142307070910538, Value="POST", Code="2", Sort=100, Remark="POST", Status=0 },
                new SysDictData{Id=142307070906481, TypeId=142307070910538, Value="PUT", Code="3", Sort=100, Remark="PUT", Status=0 },
                new SysDictData{Id=142307070906482, TypeId=142307070910538, Value="DELETE", Code="4", Sort=100, Remark="DELETE", Status=0 }
            };
        }
    }
}
