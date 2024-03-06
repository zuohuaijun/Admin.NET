// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.NET.Web.Entry.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        //private readonly ISystemService _systemService;

        //public HomeController(ISystemService systemService)
        //{
        //    _systemService = systemService;
        //}

        public IActionResult Index()
        {
            //ViewBag.Description = _systemService.GetDescription();

            return View();
        }
    }
}