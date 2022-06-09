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