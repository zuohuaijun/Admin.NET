// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

/// <summary>
/// 代码生成参数类
/// </summary>
public class CodeGenInput : BasePageInput
{
    /// <summary>
    /// 作者姓名
    /// </summary>
    public virtual string AuthorName { get; set; }

    /// <summary>
    /// 类名
    /// </summary>
    public virtual string ClassName { get; set; }

    /// <summary>
    /// 是否移除表前缀
    /// </summary>
    public virtual string TablePrefix { get; set; }

    /// <summary>
    /// 库定位器名
    /// </summary>
    public virtual string ConfigId { get; set; }

    /// <summary>
    /// 数据库名(保留字段)
    /// </summary>
    public virtual string DbName { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public virtual string DbType { get; set; }

    /// <summary>
    /// 数据库链接
    /// </summary>
    public virtual string ConnectionString { get; set; }

    /// <summary>
    /// 生成方式
    /// </summary>
    public virtual string GenerateType { get; set; }

    /// <summary>
    /// 数据库表名
    /// </summary>
    public virtual string TableName { get; set; }

    /// <summary>
    /// 命名空间
    /// </summary>
    public virtual string NameSpace { get; set; }

    /// <summary>
    /// 业务名（业务代码包名称）
    /// </summary>
    public virtual string BusName { get; set; }

    /// <summary>
    /// 功能名（数据库表名称）
    /// </summary>
    public virtual string TableComment { get; set; }

    /// <summary>
    /// 菜单应用分类（应用编码）
    /// </summary>
    public virtual string MenuApplication { get; set; }

    /// <summary>
    /// 菜单父级
    /// </summary>
    public virtual long MenuPid { get; set; }
}

public class AddCodeGenInput : CodeGenInput
{
    /// <summary>
    /// 数据库表名
    /// </summary>
    [Required(ErrorMessage = "数据库表名不能为空")]
    public override string TableName { get; set; }

    /// <summary>
    /// 业务名（业务代码包名称）
    /// </summary>
    [Required(ErrorMessage = "业务名不能为空")]
    public override string BusName { get; set; }

    /// <summary>
    /// 命名空间
    /// </summary>
    [Required(ErrorMessage = "命名空间不能为空")]
    public override string NameSpace { get; set; }

    /// <summary>
    /// 作者姓名
    /// </summary>
    [Required(ErrorMessage = "作者姓名不能为空")]
    public override string AuthorName { get; set; }

    ///// <summary>
    ///// 类名
    ///// </summary>
    //[Required(ErrorMessage = "类名不能为空")]
    //public override string ClassName { get; set; }

    ///// <summary>
    ///// 是否移除表前缀
    ///// </summary>
    //[Required(ErrorMessage = "是否移除表前缀不能为空")]
    //public override string TablePrefix { get; set; }

    /// <summary>
    /// 生成方式
    /// </summary>
    [Required(ErrorMessage = "生成方式不能为空")]
    public override string GenerateType { get; set; }

    ///// <summary>
    ///// 功能名（数据库表名称）
    ///// </summary>
    //[Required(ErrorMessage = "数据库表名不能为空")]
    //public override string TableComment { get; set; }

    /// <summary>
    /// 菜单父级
    /// </summary>
    [Required(ErrorMessage = "菜单父级不能为空")]
    public override long MenuPid { get; set; }
}

public class DeleteCodeGenInput
{
    /// <summary>
    /// 代码生成器Id
    /// </summary>
    [Required(ErrorMessage = "代码生成器Id不能为空")]
    public long Id { get; set; }
}

public class UpdateCodeGenInput : CodeGenInput
{
    /// <summary>
    /// 代码生成器Id
    /// </summary>
    [Required(ErrorMessage = "代码生成器Id不能为空")]
    public long Id { get; set; }
}

public class QueryCodeGenInput : DeleteCodeGenInput
{
}