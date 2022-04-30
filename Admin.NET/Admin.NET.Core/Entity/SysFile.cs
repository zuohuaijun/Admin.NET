using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统文件表
    /// </summary>
    [SugarTable("sys_file", "系统文件表")]
    [SqlSugarEntity]
    public class SysFile : EntityBase
    {
        /// <summary>
        /// 仓储名称
        /// </summary>
        [SugarColumn(ColumnDescription = "仓储名称", Length = 100)]
        [MaxLength(100)]
        public string BucketName { get; set; }

        /// <summary>
        /// 文件名称（上传时名称）
        /// </summary>文件名称
        [SugarColumn(ColumnDescription = "文件名称", Length = 100)]
        [MaxLength(100)]
        public string FileName { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        [SugarColumn(ColumnDescription = "文件后缀", Length = 20)]
        [MaxLength(20)]
        public string Suffix { get; set; }

        /// <summary>
        /// 存储路径
        /// </summary>
        [SugarColumn(ColumnDescription = "存储路径", Length = 100)]
        [MaxLength(100)]
        public string FilePath { get; set; }

        /// <summary>
        /// 文件大小KB
        /// </summary>
        [SugarColumn(ColumnDescription = "文件大小KB", Length = 10)]
        [MaxLength(10)]
        public string SizeKb { get; set; }

        /// <summary>
        /// 文件大小信息-计算后的
        /// </summary>
        [SugarColumn(ColumnDescription = "文件大小信息", Length = 50)]
        [MaxLength(50)]
        public string SizeInfo { get; set; }
    }
}