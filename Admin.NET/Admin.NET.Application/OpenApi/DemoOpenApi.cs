// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Application;

///// <summary>
///// 示例开放接口
///// </summary>
//[ApiDescriptionSettings("开放接口", Name = "Demo", Order = 100)]
//[Authorize(AuthenticationSchemes = SignatureAuthenticationDefaults.AuthenticationScheme)]
//public class DemoOpenApi : IDynamicApiController
//{
//    private readonly UserManager _userManager;

//    public DemoOpenApi(UserManager userManager)
//    {
//        _userManager = userManager;
//    }

//    [HttpGet("helloWord")]
//    public Task<string> HelloWord()
//    {
//        return Task.FromResult($"Hello word. {_userManager.Account}");
//    }
//}