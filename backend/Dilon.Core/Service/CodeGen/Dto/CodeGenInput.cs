using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 代码生成参数类
    /// </summary>
    public class CodeGenPageInput : PageInputBase
    {        
        /// <summary>
        /// 数据库表名
        /// </summary>
        public string TableName { get; set; }
    }

    public class AddCodeGenInput
    {
        /// <summary>
        /// 数据库表名
        /// </summary>
        [Required(ErrorMessage = "数据库表名不能为空")]
        public string TableName { get; set; }

        /// <summary>
        /// 业务名（业务代码包名称）
        /// </summary>
        [Required(ErrorMessage = "业务名不能为空")]
        public string BusName { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        [Required(ErrorMessage = "命名空间不能为空")]
        public string NameSpace { get; set; }

        /// <summary>
        /// 作者姓名
        /// </summary>
        [Required(ErrorMessage = "作者姓名不能为空")]
        public string AuthorName { get; set; }

        /// <summary>
        /// 生成方式
        /// </summary>
        [Required(ErrorMessage = "生成方式不能为空")]
        public string GenerateType { get; set; }

        /// <summary>
        /// 菜单应用分类（应用编码）
        /// </summary>
        [Required(ErrorMessage = "菜单应用分类不能为空")]
        public string MenuApplication { get; set; }

        /// <summary>
        /// 菜单父级
        /// </summary>
        [Required(ErrorMessage = "菜单父级不能为空")]
        public long MenuPid { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 是否移除表前缀
        /// </summary>
        public string TablePrefix { get; set; }

        /// <summary>
        /// 功能名（数据库表名称）
        /// </summary>
        public string TableComment { get; set; }
    }

    public class DeleteCodeGenInput : BaseId
    {
    }

    public class UpdateCodeGenInput
    {
        /// <summary>
        /// 代码生成器Id
        /// </summary>
        [Required(ErrorMessage = "代码生成器Id不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 数据库表名
        /// </summary>
        [Required(ErrorMessage = "数据库表名不能为空")]
        public string TableName { get; set; }

        /// <summary>
        /// 业务名（业务代码包名称）
        /// </summary>
        [Required(ErrorMessage = "业务名不能为空")]
        public string BusName { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        [Required(ErrorMessage = "命名空间不能为空")]
        public string NameSpace { get; set; }

        /// <summary>
        /// 作者姓名
        /// </summary>
        [Required(ErrorMessage = "作者姓名不能为空")]
        public string AuthorName { get; set; }

        /// <summary>
        /// 生成方式
        /// </summary>
        [Required(ErrorMessage = "生成方式不能为空")]
        public string GenerateType { get; set; }

        /// <summary>
        /// 菜单应用分类（应用编码）
        /// </summary>
        [Required(ErrorMessage = "菜单应用分类不能为空")]
        public string MenuApplication { get; set; }

        /// <summary>
        /// 菜单父级
        /// </summary>
        [Required(ErrorMessage = "菜单父级不能为空")]
        public long MenuPid { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 是否移除表前缀
        /// </summary>
        public string TablePrefix { get; set; }

        /// <summary>
        /// 功能名（数据库表名称）
        /// </summary>
        public string TableComment { get; set; }
    }

    public class QueryCodeGenInput : BaseId
    {
    }
}