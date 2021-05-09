using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 文件信息表
    /// </summary>
    [Table("sys_file")]
    [Comment("文件信息表")]
    public class SysFile : DEntityBase
    {
        /// <summary>
        /// 文件存储位置（1:阿里云，2:腾讯云，3:minio，4:本地）
        /// </summary>
        [Comment("文件存储位置")]
        public int FileLocation { get; set; }

        /// <summary>
        /// 文件仓库
        /// </summary>
        [Comment("文件仓库")]
        [MaxLength(100)]
        public string FileBucket { get; set; }

        /// <summary>
        /// 文件名称（上传时候的文件名）
        /// </summary>
        [Comment("文件名称")]
        [MaxLength(100)]
        public string FileOriginName { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        [Comment("文件后缀")]
        [MaxLength(50)]
        public string FileSuffix { get; set; }

        /// <summary>
        /// 文件大小kb
        /// </summary>
        [Comment("文件大小kb")]
        [MaxLength(10)]
        public string FileSizeKb { get; set; }

        /// <summary>
        /// 文件大小信息，计算后的
        /// </summary>
        [Comment("文件大小信息")]
        [MaxLength(50)]
        public string FileSizeInfo { get; set; }

        /// <summary>
        /// 存储到bucket的名称（文件唯一标识id）
        /// </summary>
        [Comment("存储到bucket的名称")]
        [MaxLength(100)]
        public string FileObjectName { get; set; }

        /// <summary>
        /// 存储路径
        /// </summary>
        [Comment("存储路径")]
        [MaxLength(100)]
        public string FilePath { get; set; }
    }
}