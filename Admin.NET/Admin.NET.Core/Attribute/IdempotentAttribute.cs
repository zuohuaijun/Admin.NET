// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using System.Security.Claims;

namespace Admin.NET.Core;

/// <summary>
/// 防止重复请求过滤器特性
/// </summary>
public class IdempotentAttribute : Attribute, IAsyncActionFilter
{
    /// <summary>
    /// 请求间隔时间/秒
    /// </summary>
    public int IntervalTime { get; set; } = 5;

    /// <summary>
    /// 错误提示内容
    /// </summary>
    public string Message { get; set; } = "你操作频率过快，请稍后重试！";

    /// <summary>
    /// 缓存前缀: Key+请求路由+用户Id+请求参数
    /// </summary>
    public string CacheKey { get; set; }

    /// <summary>
    /// 是否直接抛出异常：Ture是，False返回上次请求结果
    /// </summary>
    public bool ThrowBah { get; set; }

    public IdempotentAttribute()
    {
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var path = httpContext.Request.Path.Value.ToString();
        var userId = httpContext.User?.FindFirstValue(ClaimConst.UserId);
        var cacheExpireTime = TimeSpan.FromSeconds(IntervalTime);

        var parameters = "";
        foreach (var parameter in context.ActionDescriptor.Parameters)
        {
            parameters += parameter.Name;
            parameters += context.ActionArguments[parameter.Name].ToJson();
        }

        var cacheKey = MD5Encryption.Encrypt($"{CacheKey}{path}{userId}{parameters}");
        var sysCacheService = App.GetService<SysCacheService>();
        if (sysCacheService.ExistKey(cacheKey))
        {
            if (ThrowBah) throw Oops.Oh(Message);

            try
            {
                var cachedResult = sysCacheService.Get<ResponseData>(cacheKey);
                context.Result = new ObjectResult(cachedResult.Value);
            }
            catch (Exception ex)
            {
                throw Oops.Oh($"{Message}-{ex}");
            }
        }
        else
        {
            // 先加入一个空缓存，防止第一次请求结果没回来导致连续请求
            sysCacheService.Set(cacheKey, "", cacheExpireTime);
            var resultContext = await next();
            if (resultContext.Result is ObjectResult objectResult)
            {
                var valueType = objectResult.Value.GetType();
                var responseData = new ResponseData
                {
                    Type = valueType.Name,
                    Value = objectResult.Value
                };
                sysCacheService.Set(cacheKey, responseData, cacheExpireTime);
            }
        }
    }

    /// <summary>
    /// 请求结果数据
    /// </summary>
    private class ResponseData
    {
        /// <summary>
        /// 结果类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 请求结果
        /// </summary>
        public dynamic Value { get; set; }
    }
}