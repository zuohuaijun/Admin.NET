// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 动态作业编译
/// </summary>
public class DynamicJobCompiler : ISingleton
{
    /// <summary>
    /// 编译代码并返回其中实现 IJob 的类型
    /// </summary>
    /// <param name="script">动态编译的作业代码</param>
    /// <returns></returns>
    public Type BuildJob(string script)
    {
        var jobAssembly = Schedular.CompileCSharpClassCode(script);
        var jobType = jobAssembly.GetTypes().FirstOrDefault(u => typeof(IJob).IsAssignableFrom(u));
        return jobType;
    }
}