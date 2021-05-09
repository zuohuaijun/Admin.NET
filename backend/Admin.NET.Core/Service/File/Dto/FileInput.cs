namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 上传文件参数
    /// </summary>
    public class FilePageInput : PageInputBase
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
    }

    public class DeleteFileInfoInput : BaseId
    {
    }

    public class QueryFileInoInput : BaseId
    {
    }
}