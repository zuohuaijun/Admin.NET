using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dilon.Core
{
    /// <summary>
    /// 阿里云oss文件上传工具类  by--jck
    /// </summary>
    public class OSSClientHelper
    {

        public static string accessKeyId = "accessKeyId";
        public static string accessKeySecret = "accessKeySecret";
        //const string endpoint = "oss-cn-huhehaote-internal.aliyuncs.com";
        private static string internalEndpoint = "internalEndpoint"; //内网传输连接
        const string endpoint = "oss-cn-beijing.aliyuncs.com"; //"oss-cn-beijing.aliyuncs.com";// "oss-cn-huhehaote-internal.aliyuncs.com";//"oss-cn-huhehaote.aliyuncs.com" ;
        public static string bucketName = "bucketName";

        public static OssClient GetClient()
        {

            return new OssClient(endpoint, accessKeyId, accessKeySecret);
        }
        public static OssClient GetClient_CND()
        {

            var conf = new ClientConfiguration();
            conf.IsCname = true;
            return new OssClient("cdnmedia.aliyuncs.com", accessKeyId, accessKeySecret, conf);
        }
        public static OssClient GetClient_internal()
        {
            return new OssClient(internalEndpoint, accessKeyId, accessKeySecret);
        }

        #region 文件上传OSS，走阿里云内网传输
        /// <summary>
        /// 上传本地文件
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="bucket"></param>
        /// <param name="contentType"></param>
        /// <param name="localFilename"></param>
        /// <returns></returns>
        public static bool PushMedia_internal(string objectName, string localFilename)
        {
            try
            {
                var client = GetClient_internal();
                return client.PutObject(bucketName, objectName, localFilename).HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            { }
            return false;
        }
        #endregion

        /// <summary>
        /// 上传一个图片
        /// </summary>
        /// <param name="base64Code">图片经过base64加密后的结果</param>
        /// <param name="fileName">文件名,例如:Emplyoee/dzzBack.jpg</param>
        public static bool PushImg(string base64Code, string fileName)
        {
            try
            {
                var client = GetClient();
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64Code));
                return client.PutObject(bucketName, fileName, stream).HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            { }
            return false;
        }
        public static bool PushMedia(Stream stream, string fileName)
        {

            try
            {
                var client = GetClient();
                //MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64Code));
                //var metadata = new ObjectMetadata();
                //metadata.ContentType = TypeTo(contentType);

                return client.PutObject(bucketName, fileName, stream).HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            { }
            return false;
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

            if (System.IO.File.Exists(localFilename))
            {
                //存在文件
                if (!DoesObjectExist(objectName))
                {

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
                            MemoryStream ms = new MemoryStream(stream);
                            var c = client.PutObject(bucketName, objectName, ms);

                            if (c.HttpStatusCode == System.Net.HttpStatusCode.OK) { return 4; } else { return 3; }


                        }
                        else
                        {
                            var c = client.PutObject(bucketName, objectName, localFilename);

                            if (c.HttpStatusCode == System.Net.HttpStatusCode.OK) { return 4; } else { return 3; }

                        }
                    }
                    catch (Exception ex)
                    { return 3; }
                }
                else
                {
                    return 2;
                }

            }
            else
            {
                return 1;
            }
        }
        /// <summary>
        /// 上传一个图片
        /// </summary>
        /// <param name="filebyte">图片字节 </param>
        /// <param name="fileName">文件名,例如:Emplyoee/dzzBack.jpg</param>
        public static bool PushImg(byte[] filebyte, string fileName, out string md5)
        {
            md5 = string.Empty;
            try
            {
                var client = GetClient();
                MemoryStream stream = new MemoryStream(filebyte, 0, filebyte.Length);
                var result = client.PutObject(bucketName, fileName, stream);
                md5 = result.ResponseMetadata["Content-MD5"];
                return result.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception)
            { }
            return false;
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
            var req = new GeneratePresignedUriRequest(bucketName, key, SignHttpMethod.Get)
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
            var req = new GeneratePresignedUriRequest(bucketName, key, SignHttpMethod.Get)
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
        protected byte[] AuthGetFileData(string fileUrl)
        {
            using (FileStream fs = new FileStream(fileUrl, FileMode.OpenOrCreate, FileAccess.ReadWrite))
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
        public static bool deletefileCode(string fileCode)
        {
            if (string.IsNullOrEmpty(fileCode))
            {
                return true;
            }
            //检查fileCode磁盘中是否存在此文件
            if (File.Exists(fileCode))
            {
                File.Delete(fileCode);
                return true;
            }
            try
            {
                var client = GetClient();
                client.DeleteObject(bucketName, fileCode);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="fileCode">文件id</param>
        /// <returns>文件url</returns>
        public static bool DeleteFolder(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return true;
            }
            var client = GetClient();

            var listObjectsRequest = new ListObjectsRequest(bucketName);
            listObjectsRequest.Prefix = prefix;
            var result = client.ListObjects(listObjectsRequest);

            foreach (var summary in result.ObjectSummaries)
            {
                client.DeleteObject(bucketName, summary.Key);
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
            try
            {
                var client = GetClient();
                // 判断文件是否存在。
                var exist = client.DoesObjectExist(bucketName, fileName);
                return exist;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <returns></returns>
        public static string TypeTo(string type)
        {
            switch (type)
            {
                case "mp4":
                    type = "video/mp4";
                    break;
                case "mp3":
                    type = "audio/mp3";
                    break;
                default:
                    type = "image/" + type;
                    break;
            }
            return type;
        }
        public static OssObject downLoad(String fileName)
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
            var req = new GeneratePresignedUriRequest(bucketName, key, SignHttpMethod.Get)
            {
                Expiration = DateTime.Now.AddHours(1)
            };

            return client.GetObject(bucketName, key);

        }

        #region 阿里云CDN API刷新缓存

        #endregion

    }
}
