using Furion.DependencyInjection;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public class CommonService : ICommonService, IScoped
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommonService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取库表信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EntityInfo>> GetEntityInfos()
        {
            var entityInfos = new List<EntityInfo>();

            var type = typeof(SugarTable);
            var type1 = typeof(NotTableAttribute);
            var types = new List<Type>();
            foreach (var assemblyName in CommonConst.EntityAssemblyName)
            {
                Assembly asm = Assembly.Load(assemblyName);
                types.AddRange(asm.GetExportedTypes().ToList());
            }
            Func<Attribute[], bool> IsMyAttribute = o =>
            {
                if (o.Where(c => c.GetType() == type1).Count() > 0) return false;
                foreach (Attribute a in o)
                {
                    if (a.GetType() == type && a.GetType() != type1)
                        return true;
                }
                return false;
            };
            Type[] cosType = types.Where(o =>
            {
                return IsMyAttribute(Attribute.GetCustomAttributes(o, true));
            }
            ).ToArray();

            foreach (var c in cosType)
            {
                var sugarAttribute = c.GetCustomAttributes(type, true)?.FirstOrDefault();

                var des = c.GetCustomAttributes(typeof(DescriptionAttribute), true);
                var description = "";
                if (des.Length > 0)
                {
                    description = ((DescriptionAttribute)des[0]).Description;
                }
                entityInfos.Add(new EntityInfo()
                {
                    EntityName = c.Name,
                    DbTableName = sugarAttribute == null ? c.Name : ((SugarTable)sugarAttribute).TableName,
                    TableDescription = description
                });
            }
            return await Task.FromResult(entityInfos);
        }

        /// <summary>
        /// 获取Host
        /// </summary>
        /// <returns></returns>
        public string GetHost()
        {
            return $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}";
        }

        /// <summary>
        /// 获取文件URL
        /// </summary>
        /// <param name="sysFile"></param>
        /// <returns></returns>
        public string GetFileUrl(SysFile sysFile)
        {
            return $"{GetHost()}/{sysFile.FilePath}/{sysFile.Id + sysFile.Suffix}";
        }
    }
}