using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core.Entity.System
{
    /// <summary>
    /// 文件信息表
    /// </summary>
    [Table("sys_file")]
    public class SysFile : DEntityBase
    {
        /// <summary>
        /// 文件存储位置（1:阿里云，2:腾讯云，3:minio，4:本地）
        /// </summary>
        public int FileLocation { get; set; }

        /// <summary>
        /// 文件仓库
        /// </summary>
        public string FileBucket { get; set; }

        /// <summary>
        /// 文件名称（上传时候的文件名）
        /// </summary>
        public string FileOriginName { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        public string FileSuffix { get; set; }

        /// <summary>
        /// 文件大小kb
        /// </summary>
        public long FileSizeKb { get; set; }

        /// <summary>
        /// 文件大小信息，计算后的
        /// </summary>
        public string FileSizeInfo { get; set; }

        /// <summary>
        /// 存储到bucket的名称（文件唯一标识id）
        /// </summary>
        public string FileObjectName { get; set; }

        /// <summary>
        /// 存储路径
        /// </summary>
        public string FilePath { get; set; }
    }
}
