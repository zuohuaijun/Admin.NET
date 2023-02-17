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
        // 动态创建作业
        var builder = new AssemblyCSharpBuilder("Admin.NET.Core")
        {
            Domain = DomainManagement.Random()
        };

        builder.Add(script);

        return builder.GetAssembly().GetTypes().FirstOrDefault(u => typeof(IJob).IsAssignableFrom(u));
    }
}