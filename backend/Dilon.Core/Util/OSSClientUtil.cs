using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.IO;
using System.Net;

namespace Dilon.Core
{
    /// <summary>
    /// 阿里云oss文件上传工具类
    /// </summary>
    public class OSSClientUtil
    {
        private static readonly string _accessKeyId = "accessKeyId";
        private static readonly string _accessKeySecret = "accessKeySecret";
        //const string endpoint = "oss-cn-huhehaote-internal.aliyuncs.com";
        private static readonly string _internalEndpoint = "internalEndpoint"; //内网传输连接
        private const string _endpoint = "oss-cn-beijing.aliyuncs.com"; //"oss-cn-beijing.aliyuncs.com";// "oss-cn-huhehaote-internal.aliyuncs.com";//"oss-cn-huhehaote.aliyuncs.com" ;
        private static readonly string _bucketName = "bucketName";

        public static OssClient GetClient()
        {
            return new OssClient(_endpoint, _accessKeyId, _accessKeySecret);
        }

        public static OssClient GetClient_CND()
        {
            var conf = new ClientConfiguration
            {
                IsCname = true
            };
            return new OssClient("cdnmedia.aliyuncs.com", _accessKeyId, _accessKeySecret, conf);
        }

        public static OssClient GetClient_internal()
        {
            return new OssClient(_internalEndpoint, _accessKeyId, _accessKeySecret);
        }

        /// <summary>
        /// 上传本地文件(走阿里云内网传输)
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="localFilename"></param>
        /// <returns></returns>
        public static bool PushMedia_internal(string objectName, string localFilename)
        {
            var client = GetClient_internal();
            return client.PutObject(_bucketName, objectName, localFilename).HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// 上传一个图片
        /// </summary>
        /// <param name="base64Code">图片经过base64加密后的结果</param>
        /// <param name="fileName">文件名,例如:Emplyoee/dzzBack.jpg</param>
        public static bool PushImg(string base64Code, string fileName)
        {
            var client = GetClient();
            var stream = new MemoryStream(Convert.FromBase64String(base64Code));
            return client.PutObject(_bucketName, fileName, stream).HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public static bool PushMedia(Stream stream, string fileName)
        {
            var client = GetClient();
            return client.PutObject(_bucketName, fileName, stream).HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// 上传本地文件
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="localFilename"></param>
        /// 返回参数说明  1.本地文件不存在  2.文件oss上已存在  3.上传失败  4.上传成功
        /// <returns></returns>
        public static int PushMedia(string objectName, string localFilename)
        {
            if (!File.Exists(localFilename)) return 1;            
            if (DoesObjectExist(objectName)) return 2; // 存在文件
            try
            {
                var client = GetClient();
                //MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64Code));
                //var metadata = new ObjectMetadata();
                //metadata.ContentType = TypeTo(contentType);

                if (localFilename.Contains("http"))
                {
                    var webClient = new WebClient { Credentials = CredentialCache.DefaultCredentials };
                    var stream = webClient.DownloadData(localFilename);
                    var ms = new MemoryStream(stream);
                    var c = client.PutObject(_bucketName, objectName, ms);
                    if (c.HttpStatusCode == System.Net.HttpStatusCode.OK) { return 4; } else { return 3; }
                }
                else
                {
                    var c = client.PutObject(_bucketName, objectName, localFilename);
                    if (c.HttpStatusCode == System.Net.HttpStatusCode.OK) { return 4; } else { return 3; }
                }
            }
            catch 
            {
                return 3;
            }
        }

        /// <summary>
        /// 上传一个图片
        /// </summary>
        /// <param name="filebyte">图片字节 </param>
        /// <param name="fileName">文件名,例如:Emplyoee/dzzBack.jpg</param>
        /// <param name="md5"></param>
        public static bool PushImg(byte[] filebyte, string fileName, out string md5)
        {
            var client = GetClient();
            var stream = new MemoryStream(filebyte, 0, filebyte.Length);
            var result = client.PutObject(_bucketName, fileName, stream);
            md5 = result.ResponseMetadata["Content-MD5"];
            return result.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// 获取鉴权后的URL,URL有效日期默认一小时
        /// </summary>
        /// <param name="fileName">文件名,例如:Emplyoee/dzzBack.jpg</param>
        /// <returns></returns>
        public static string GetImg(string fileName)
        {
            var client = GetClient();
            var key = fileName;
            var req = new GeneratePresignedUriRequest(_bucketName, key, SignHttpMethod.Get)
            {
                Expiration = DateTime.Now.AddHours(1)
            };
            return client.GeneratePresignedUri(req).ToString();
        }

        /// <summary>
        /// 获取鉴权后的URL
        /// </summary>
        /// <param name="fileName">文件名,例如:Emplyoee/dzzBack.jpg</param>
        /// <param name="expiration">URL有效日期,例如:DateTime.Now.AddHours(1) </param>
        /// <returns></returns>
        public static string GetImg(string fileName, DateTime expiration)
        {
            var client = GetClient();
            var key = fileName;
            var req = new GeneratePresignedUriRequest(_bucketName, key, SignHttpMethod.Get)
            {
                Expiration = expiration
            };
            return client.GeneratePresignedUri(req).ToString();
        }

        /// <summary>
        /// 将文件转换成byte[] 数组
        /// </summary>
        /// <param name="fileUrl">文件路径文件名称</param>
        /// <returns>byte[]</returns>
        private byte[] AuthGetFileData(string fileUrl)
        {
            using (var fs = new FileStream(fileUrl, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                byte[] buffur = new byte[fs.Length];
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(buffur);
                    bw.Close();
                }
                return buffur;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileCode">文件id</param>
        /// <returns>文件url</returns>
        public static bool DeletefileCode(string fileCode)
        {
            if (string.IsNullOrEmpty(fileCode))
                return true;

            //检查fileCode磁盘中是否存在此文件
            if (File.Exists(fileCode))
            {
                File.Delete(fileCode);
                return true;
            }
            var client = GetClient();
            client.DeleteObject(_bucketName, fileCode);
            return true;
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="prefix">文件id</param>
        /// <returns>文件url</returns>
        public static bool DeleteFolder(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return true;

            var client = GetClient();
            var listObjectsRequest = new ListObjectsRequest(_bucketName);
            listObjectsRequest.Prefix = prefix;
            var result = client.ListObjects(listObjectsRequest);

            foreach (var summary in result.ObjectSummaries)
            {
                client.DeleteObject(_bucketName, summary.Key);
            }
            return false;
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool DoesObjectExist(string fileName)
        {
            var client = GetClient();
            // 判断文件是否存在。
            var exist = client.DoesObjectExist(_bucketName, fileName);
            return exist;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <returns></returns>
        public static string TypeTo(string type)
        {
            type = type switch
            {
                "mp4" => "video/mp4",
                "mp3" => "audio/mp3",
                _ => "image/" + type,
            };
            return type;
        }

        public static OssObject DownLoad(String fileName)
        {
            //var client = GetClient();
            //DateTime expiration = new DateTime().AddHours(1);
            //GeneratePresignedUriRequest request = new GeneratePresignedUriRequest(bucketName, fileName, SignHttpMethod.Get);
            //// 设置过期时间。
            //request.Expiration=expiration;
            //// 生成签名URL（HTTP GET请求）。
            //Uri signedUrl = client.GeneratePresignedUri(request);

            //// 添加GetObject请求头。
            ////customHeaders.put("Range", "bytes=100-1000");
            //OssObject result = client.GetObject(signedUrl);

            var client = GetClient();
            var key = fileName;
            _ = new GeneratePresignedUriRequest(_bucketName, key, SignHttpMethod.Get)
            {
                Expiration = DateTime.Now.AddHours(1)
            };
            return client.GetObject(_bucketName, key);
        }
    }
}
