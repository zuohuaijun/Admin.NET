// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

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
    ///  自定义缓存: Key+请求路由+用户Id+请求参数
    /// </summary>
    public string CacheKey { get; set; } = "";

    /// <summary>
    /// 直接抛出异常Ture，返回上次请求结果False
    /// </summary>
    public bool ThrowBah { get; set; } = false;

    private SysCacheService _sysCacheService { get; set; }

    public IdempotentAttribute()
    {
        _sysCacheService = App.GetService<SysCacheService>();
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

        var cacheKey = MD5Encryption.Encrypt(CacheKey + path + userId + parameters);
        if (_sysCacheService.ExistKey(cacheKey))
        {
            if (ThrowBah) throw Oops.Oh(Message);

            try
            {
                var cachedResult = _sysCacheService.Get<ResponseData>(cacheKey);
                context.Result = new ObjectResult(cachedResult.Value);
            }
            catch (Exception)
            {
                throw Oops.Oh(Message);
            }
        }
        else
        {
            // 先加入一个空缓存，防止第一次请求结果没回来导致连续请求
            _sysCacheService.Set(cacheKey, "", cacheExpireTime);
            var resultContext = await next();
            if (resultContext.Result is ObjectResult objectResult)
            {
                var valueType = objectResult.Value.GetType();
                var responseData = new ResponseData
                {
                    Type = valueType.Name,
                    Value = objectResult.Value
                };
                _sysCacheService.Set(cacheKey, responseData, cacheExpireTime);
            }
        }
    }

    /// <summary>
    /// 请求结果
    /// </summary>
    private class ResponseData
    {
        /// <summary>
        /// 结果类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 请求结果返回的数据
        /// </summary>
        public dynamic Value { get; set; }
    }
}