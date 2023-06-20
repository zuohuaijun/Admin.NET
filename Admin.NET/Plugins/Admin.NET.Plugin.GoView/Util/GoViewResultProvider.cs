namespace Admin.NET.Plugin.GoView.Util;

/// <summary>
/// GoView 规范化结果
/// </summary>
[UnifyModel(typeof(GoViewResult<>))]
public class GoViewResultProvider : IUnifyResultProvider
{
    /// <summary>
    /// 异常返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnException(ExceptionContext context, ExceptionMetadata metadata)
    {
        return new JsonResult(RESTfulResult(metadata.StatusCode, data: metadata.Data, errors: metadata.Errors));
    }

    /// <summary>
    /// 成功返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public IActionResult OnSucceeded(ActionExecutedContext context, object data)
    {
        return new JsonResult(RESTfulResult(StatusCodes.Status200OK, true, data));
    }

    /// <summary>
    /// 验证失败返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnValidateFailed(ActionExecutingContext context, ValidationMetadata metadata)
    {
        return new JsonResult(RESTfulResult(metadata.StatusCode ?? StatusCodes.Status400BadRequest, data: metadata.Data, errors: metadata.ValidationResult));
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
                await context.Response.WriteAsJsonAsync(RESTfulResult(886, errors: "401 登录已过期，请重新登录"),
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
    private static GoViewResult RESTfulResult(int statusCode, bool succeeded = default, object data = default, object errors = default)
    {
        if (data == null)
        {
            return new GoViewResult
            {
                Code = statusCode,
                Msg = errors is null or string ? (errors + "") : JSON.Serialize(errors),
            };
        }
        else
        {
            var parseCount = int.TryParse(UnifyContext.Take() + "", out var count);
            return new GoViewResult<object>
            {
                Code = statusCode,
                Msg = errors is null or string ? (errors + "") : JSON.Serialize(errors),
                Data = data,
                Count = parseCount ? count : null,
            };
        }
    }
}

/// <summary>
/// GoView 返回结果
/// </summary>
public class GoViewResult
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 信息
    /// </summary>
    public string Msg { get; set; }
}

/// <summary>
/// GoView 返回结果
/// </summary>
/// <typeparam name="T"></typeparam>
public class GoViewResult<T> : GoViewResult
{
    /// <summary>
    /// 数据
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// 总数
    /// </summary>
    public int? Count { get; set; }
}