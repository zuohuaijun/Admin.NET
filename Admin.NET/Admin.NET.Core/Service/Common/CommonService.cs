namespace Admin.NET.Core.Service;

public class CommonService : ICommonService, IScoped
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CodeGenOptions _codeGenOptions;

    public CommonService(IHttpContextAccessor httpContextAccessor,
        IOptions<CodeGenOptions> codeGenOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _codeGenOptions = codeGenOptions.Value;
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
        if (_codeGenOptions.EntityAssemblyNames != null)
        {
            foreach (var assemblyName in _codeGenOptions.EntityAssemblyNames)
            {
                Assembly asm = Assembly.Load(assemblyName);
                types.AddRange(asm.GetExportedTypes().ToList());
            }
        }
        Func<Attribute[], bool> IsMyAttribute = o =>
        {
            if (o.Where(c => c.GetType() == type1).Any()) return false;
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