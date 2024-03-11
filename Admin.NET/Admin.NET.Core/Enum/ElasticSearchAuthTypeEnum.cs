// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// ES认证类型枚举
/// <para>https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/_options_on_elasticsearchclientsettings.html</para>
/// </summary>
[Description("ES认证类型枚举")]
public enum ElasticSearchAuthTypeEnum
{
    /// <summary>
    /// BasicAuthentication
    /// </summary>
    [Description("BasicAuthentication")]
    Basic = 1,

    /// <summary>
    /// ApiKey
    /// </summary>
    [Description("ApiKey")]
    ApiKey = 2,

    /// <summary>
    /// Base64ApiKey
    /// </summary>
    [Description("Base64ApiKey")]
    Base64ApiKey = 3
}