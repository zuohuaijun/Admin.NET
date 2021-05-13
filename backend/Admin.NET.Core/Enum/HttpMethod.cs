using System.ComponentModel;

namespace Admin.NET.Core
{
    /// <summary>
    /// 请求方式类型
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// GET
        /// </summary>
        [Description("GET")]
        GET = 0,

        /// <summary>
        /// POST
        /// </summary>
        [Description("POST")]
        POST = 1,

        /// <summary>
        /// PUT
        /// </summary>
        [Description("PUT")]
        PUT = 2,

        /// <summary>
        /// DELETE
        /// </summary>
        [Description("DELETE")]
        DELETE = 3,

        /// <summary>
        /// PATCH
        /// </summary>
        [Description("PATCH")]
        PATCH = 4,

        /// <summary>
        /// HEAD
        /// </summary>
        [Description("HEAD")]
        HEAD = 5,

        /// <summary>
        /// CONNECT
        /// </summary>
        [Description("CONNECT")]
        CONNECT = 6,

        /// <summary>
        /// TRACE
        /// </summary>
        [Description("TRACE")]
        TRACE = 7,

        /// <summary>
        /// OPTIONS
        /// </summary>
        [Description("OPTIONS")]
        OPTIONS = 8
    }
}