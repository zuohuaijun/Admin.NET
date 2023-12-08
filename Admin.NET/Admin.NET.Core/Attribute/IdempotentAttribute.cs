
using System.Security.Claims;


namespace Admin.NET.Core
{

    public class IdempotentAttribute : Attribute, IAsyncActionFilter
    {
        /// <summary>
        /// 请求间隔时间
        /// </summary>
        public int Delay = 7;
        /// <summary>
        /// 错误提示内容
        /// </summary>
        public string Msg = "您的操作太快了，请稍稍慢一点！";
        /// <summary>
        /// 自定义缓存Key  缓存规则 Key + 请求路由 + 用户id + 请求参数
        /// </summary>
        public string Key = "";
        /// <summary>
        /// false 返回上次请求结果 true 直接抛出异常
        /// </summary>
        public bool Ops = false;
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
            var tenSeconds = TimeSpan.FromSeconds(Delay);

            var parameters = "";
            foreach ( var parameter in context.ActionDescriptor.Parameters) 
            {
                parameters += parameter.Name;
                parameters += context.ActionArguments[parameter.Name].ToJson();
            }

            var cacheKey = MD5Encryption.Encrypt(Key + path + userId + parameters);
            if (_sysCacheService.ExistKey(cacheKey))
            {
                if (Ops) throw Oops.Oh(Msg);

                try
                {
                    var cachedResult = _sysCacheService.Get<RequestData>(cacheKey);
                    context.Result = new ObjectResult(cachedResult.value);
                }
                catch (Exception)
                {

                    throw Oops.Oh(Msg);
                }
            }
            else
            {
                //先加入一个空的缓存，防止第一次请求结果没回来导致连续请求。
                _sysCacheService.Set(cacheKey, "", tenSeconds);
                var resultContext = await next();
                if (resultContext.Result is ObjectResult objectResult)
                {
                    var valueType = objectResult.Value.GetType();
                    var requestData = new RequestData
                    {
                        type= valueType.Name,
                        value = objectResult.Value
                    };
                    _sysCacheService.Set(cacheKey, requestData, tenSeconds);
                }
            }
        }
        /// <summary>
        /// 请求结果
        /// </summary>
        private class RequestData
        {
            /// <summary>
            /// 请求结果返回的数据
            /// </summary>
            public dynamic value { get; set; }
            /// <summary>
            /// 结果类型
            /// </summary>
            public string type { get; set; }
        }
    }
}
