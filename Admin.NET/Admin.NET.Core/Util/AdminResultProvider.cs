// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core;

/// <summary>
/// 全局规范化结果
/// </summary>
[UnifyModel(typeof(AdminResult<>))]
public class AdminResultProvider : IUnifyResultProvider
{
    /// <summary>
    /// 异常返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnException(ExceptionContext context, ExceptionMetadata metadata)
    {
        return new JsonResult(RESTfulResult(metadata.StatusCode, data: metadata.Data, errors: metadata.Errors), UnifyContext.GetSerializerSettings(context));
    }

    /// <summary>
    /// 成功返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public IActionResult OnSucceeded(ActionExecutedContext context, object data)
    {
        return new JsonResult(RESTfulResult(StatusCodes.Status200OK, true, data), UnifyContext.GetSerializerSettings(context));
    }

    /// <summary>
    /// 验证失败返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnValidateFailed(ActionExecutingContext context, ValidationMetadata metadata)
    {
        return new JsonResult(RESTfulResult(metadata.StatusCode ?? StatusCodes.Status400BadRequest, data: metadata.Data, errors: metadata.ValidationResult), UnifyContext.GetSerializerSettings(context));
    }

    /// <summary>
    /// 特定状态码返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="statusCode"></param>
    /// <param name="unifyResultSettings"></param>
    /// <returns></returns>
    public async Task OnResponseStatusCodes(HttpContext context, int statusCode, UnifyResultSettingsOptions unifyResultSettings)
    {
        // 设置响应状态码
        UnifyContext.SetResponseStatusCodes(context, statusCode, unifyResultSettings);

        switch (statusCode)
        {
            // 处理 401 状态码
            case StatusCodes.Status401Unauthorized:
                var msg = "401 登录已过期，请重新登录";
                // 若存在身份验证失败消息，则返回消息内容
                if (context.Items.TryGetValue(SignatureAuthenticationDefaults.AuthenticateFailMsgKey, out var authFailMsg))
                    msg = authFailMsg + "";
                await context.Response.WriteAsJsonAsync(RESTfulResult(statusCode, errors: msg),
                    App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
                break;
            // 处理 403 状态码
            case StatusCodes.Status403Forbidden:
                await context.Response.WriteAsJsonAsync(RESTfulResult(statusCode, errors: "403 禁止访问，没有权限"),
                    App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
                break;

            default: break;
        }
    }

    /// <summary>
    /// 返回 RESTful 风格结果集
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="succeeded"></param>
    /// <param name="data"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    private static AdminResult<object> RESTfulResult(int statusCode, bool succeeded = default, object data = default, object errors = default)
    {
        //// 统一返回值脱敏处理
        //if (data?.GetType() == typeof(String))
        //{
        //    data = App.GetRequiredService<ISensitiveDetectionProvider>().ReplaceAsync(data.ToString(), '*').GetAwaiter().GetResult();
        //}
        //else if (data?.GetType() == typeof(JsonResult))
        //{
        //    data = App.GetRequiredService<ISensitiveDetectionProvider>().ReplaceAsync(JSON.Serialize(data), '*').GetAwaiter().GetResult();
        //}

        return new AdminResult<object>
        {
            Code = statusCode,
            Message = errors is null or string ? (errors + "") : JSON.Serialize(errors),
            Result = data,
            Type = succeeded ? "success" : "error",
            Extras = UnifyContext.Take(),
            Time = DateTime.Now
        };
    }
}

/// <summary>
/// 全局返回结果
/// </summary>
/// <typeparam name="T"></typeparam>
public class AdminResult<T>
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 类型success、warning、error
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public T Result { get; set; }

    /// <summary>
    /// 附加数据
    /// </summary>
    public object Extras { get; set; }

    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time { get; set; }
}