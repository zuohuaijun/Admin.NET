using System.ComponentModel;

namespace Admin.NET.Core
{
    /// <summary>
    /// HTTP状态码
    /// </summary>
    public enum HttpStatusCode
    {
        /// <summary>
        /// 客户端可能继续其请求
        /// </summary>
        [Description("继续")]
        Continue = 100,

        /// <summary>
        /// 正在更改协议版本或协议
        /// </summary>
        [Description("交换协议")]
        SwitchingProtocols = 101,

        /// <summary>
        /// 请求成功，且请求的信息包含在响应中
        /// </summary>
        [Description("OK")]
        OK = 200,

        /// <summary>
        /// 请求导致在响应被发送前创建新资源
        /// </summary>
        [Description("已创建")]
        Created = 201,

        /// <summary>
        /// 请求已被接受做进一步处理
        /// </summary>
        [Description("接收")]
        Accepted = 202,

        /// <summary>
        /// 返回的元信息来自缓存副本而不是原始服务器，因此可能不正确
        /// </summary>
        [Description("非认证信息")]
        NonAuthoritativeInformation = 203,

        /// <summary>
        /// 已成功处理请求并且响应已被设定为无内容
        /// </summary>
        [Description("无内容")]
        NoContent = 204,

        /// <summary>
        /// 客户端应重置（或重新加载）当前资源
        /// </summary>
        [Description("重置内容")]
        ResetContent = 205,

        /// <summary>
        /// 响应是包括字节范围的 GET请求所请求的部分响应
        /// </summary>
        [Description("部分内容")]
        PartialContent = 206,

        /// <summary>
        /// 请求的信息有多种表示形式，默认操作是将此状态视为重定向
        /// </summary>
        [Description("多路选择")]
        MultipleChoices = 300,

        /// <summary>
        /// 请求的信息已移到 Location头中指定的 URI 处
        /// </summary>
        [Description("永久转移")]
        MovedPermanently = 301,

        /// <summary>
        /// 请求的信息位于 Location 头中指定的 URI 处
        /// </summary>
        [Description("暂时转移")]
        Found = 302,

        /// <summary>
        /// 将客户端自动重定向到 Location 头中指定的 URI
        /// </summary>
        [Description("参见其它")]
        SeeOther = 303,

        /// <summary>
        /// 客户端的缓存副本是最新的
        /// </summary>
        [Description("未修改")]
        NotModified = 304,

        /// <summary>
        /// 请求应使用位于 Location 头中指定的 URI 的代理服务器
        /// </summary>
        [Description("使用代理")]
        UseProxy = 305,

        /// <summary>
        /// 服务器未能识别请求
        /// </summary>
        [Description("错误请求")]
        BadRequest = 400,

        /// <summary>
        /// 请求的资源要求身份验证
        /// </summary>
        [Description("未认证")]
        Unauthorized = 401,

        /// <summary>
        /// 需要付费
        /// </summary>
        [Description("需要付费")]
        PaymentRequired = 402,

        /// <summary>
        /// 服务器拒绝满足请求
        /// </summary>
        [Description("禁止")]
        Forbidden = 403,

        /// <summary>
        /// 请求的资源不在服务器上
        /// </summary>
        [Description("未找到")]
        NotFound = 404,

        /// <summary>
        /// 请求的资源上不允许请求方法（POST或 GET）
        /// </summary>
        [Description("请求方法不允许")]
        MethodNotAllowed = 405,

        /// <summary>
        /// 客户端已用 Accept 头指示将不接受资源的任何可用表示形式
        /// </summary>
        [Description("不接受")]
        NotAcceptable = 406,

        /// <summary>
        /// 请求的代理要求身份验证
        /// Proxy-authenticate 头包含如何执行身份验证的详细信息
        /// </summary>
        [Description("需要代理认证")]
        ProxyAuthenticationRequired = 407,

        /// <summary>
        /// 客户端没有在服务器期望请求的时间内发送请求
        /// </summary>
        [Description("请求超时")]
        RequestTimeout = 408,

        /// <summary>
        /// 由于服务器上的冲突而未能执行请求
        /// </summary>
        [Description("冲突")]
        Conflict = 409,

        /// <summary>
        /// 请求的资源不再可用
        /// </summary>
        [Description("失败")]
        Gone = 410,

        /// <summary>
        /// 缺少必需的 Content-length
        /// </summary>
        [Description("缺少Content-length头")]
        LengthRequired = 411,

        /// <summary>
        /// 为此请求设置的条件失败，且无法执行此请求
        /// 条件是用条件请求标头（如 If-Match、If-None-Match 或 If-Unmodified-Since）设置的。
        /// </summary>
        [Description("条件失败")]
        PreconditionFailed = 412,

        /// <summary>
        /// 请求太大，服务器无法处理
        /// </summary>
        [Description("请求实体太大")]
        RequestEntityTooLarge = 413,

        /// <summary>
        /// URI 太长
        /// </summary>
        [Description("请求URI太长")]
        RequestUriTooLong = 414,

        /// <summary>
        /// 请求是不支持的类型
        /// </summary>
        [Description("不支持的媒体类型")]
        UnsupportedMediaType = 415,

        /// <summary>
        /// 无法返回从资源请求的数据范围，因为范围的开头在资源的开头之前，或因为范围的结尾在资源的结尾之后
        /// </summary>
        [Description("数据范围不匹配")]
        RequestedRangeNotSatisfiable = 416,

        /// <summary>
        /// 服务器未能符合Expect头中给定的预期值
        /// </summary>
        [Description("服务器与Expect头不匹配")]
        ExpectationFailed = 417,

        /// <summary>
        /// 服务器拒绝处理客户端使用当前协议发送的请求，但是可以接受其使用升级后的协议发送的请求
        /// </summary>
        [Description("当前协议不受支持")]
        UpgradeRequired = 426,

        /// <summary>
        /// 服务器上发生了一般错误
        /// </summary>
        [Description("服务器内部错误")]
        InternalServerError = 500,

        /// <summary>
        /// 服务器不支持请求的函数
        /// </summary>
        [Description("未实现")]
        NotImplemented = 501,

        /// <summary>
        /// 中间代理服务器从另一代理或原始服务器接收到错误响应
        /// </summary>
        [Description("网关失败")]
        BadGateway = 502,

        /// <summary>
        /// 服务器暂时不可用，通常是由于过多加载或维护
        /// </summary>
        [Description("服务器维护")]
        ServiceUnavailable = 503,

        /// <summary>
        /// 中间代理服务器在等待来自另一个代理或原始服务器的响应时已超时
        /// </summary>
        [Description("网关超时")]
        GatewayTimeout = 504,

        /// <summary>
        /// 服务器不支持请求的HTTP版本
        /// </summary>
        [Description("HTTP版本不支持")]
        HttpVersionNotSupported = 505
    }
}