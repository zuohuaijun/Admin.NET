using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;

namespace Dilon.Core.Service.CodeGen
{
    /// <summary>
    /// 代码生成器服务
    /// </summary>
    [ApiDescriptionSettings(Name = "CodeGen", Order = 100)]
    public class CodeGenService : IDynamicApiController, ITransient
    {
        public CodeGenService()
        {

        }
    }
}
