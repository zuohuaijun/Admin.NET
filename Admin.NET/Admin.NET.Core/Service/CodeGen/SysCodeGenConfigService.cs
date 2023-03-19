namespace Admin.NET.Core.Service;

/// <summary>
/// 系统代码生成配置服务
/// </summary>
[ApiDescriptionSettings(Order = 260)]
public class SysCodeGenConfigService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;

    public SysCodeGenConfigService(ISqlSugarClient db)
    {
        _db = db;
    }

    /// <summary>
    /// 获取代码生成配置列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取代码生成配置列表")]
    public async Task<List<CodeGenConfig>> GetList([FromQuery] CodeGenConfig input)
    {
        var whetherCommon = YesNoEnum.Y.ToString();
        return await _db.Queryable<SysCodeGenConfig>()
            .Where(u => u.CodeGenId == input.CodeGenId && u.WhetherCommon != whetherCommon)
            .Select<CodeGenConfig>().ToListAsync();
    }

    /// <summary>
    /// 更新代码生成配置
    /// </summary>
    /// <param name="inputList"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新代码生成配置")]
    public async Task UpdateCodeGenConfig(List<CodeGenConfig> inputList)
    {
        if (inputList == null || inputList.Count < 1) return;
        await _db.Updateable(inputList.Adapt<List<SysCodeGenConfig>>()).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除代码生成配置
    /// </summary>
    /// <param name="codeGenId"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(false)]
    public async Task DeleteCodeGenConfig(long codeGenId)
    {
        await _db.Deleteable<SysCodeGenConfig>().Where(u => u.CodeGenId == codeGenId).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取代码生成配置详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取代码生成配置详情")]
    public async Task<SysCodeGenConfig> GetDetail([FromQuery] CodeGenConfig input)
    {
        return await _db.Queryable<SysCodeGenConfig>().FirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 批量增加代码生成配置
    /// </summary>
    /// <param name="tableColumnOuputList"></param>
    /// <param name="codeGenerate"></param>
    [ApiDescriptionSettings(false)]
    public void AddList(List<ColumnOuput> tableColumnOuputList, SysCodeGen codeGenerate)
    {
        if (tableColumnOuputList == null) return;

        var codeGenConfigs = new List<SysCodeGenConfig>();

        foreach (var tableColumn in tableColumnOuputList)
        {
            var codeGenConfig = new SysCodeGenConfig();

            var YesOrNo = YesNoEnum.Y.ToString();
            if (Convert.ToBoolean(tableColumn.ColumnKey))
            {
                YesOrNo = YesNoEnum.N.ToString();
            }

            if (CodeGenUtil.IsCommonColumn(tableColumn.ColumnName))
            {
                codeGenConfig.WhetherCommon = YesNoEnum.Y.ToString();
                YesOrNo = YesNoEnum.N.ToString();
            }
            else
            {
                codeGenConfig.WhetherCommon = YesNoEnum.N.ToString();
            }

            codeGenConfig.CodeGenId = codeGenerate.Id;
            codeGenConfig.ColumnName = tableColumn.ColumnName;
            codeGenConfig.ColumnComment = tableColumn.ColumnComment;
            codeGenConfig.NetType = tableColumn.DataType;
            codeGenConfig.WhetherRetract = YesNoEnum.N.ToString();

            codeGenConfig.WhetherRequired = YesNoEnum.N.ToString();
            codeGenConfig.QueryWhether = YesOrNo;
            codeGenConfig.WhetherAddUpdate = YesOrNo;
            codeGenConfig.WhetherTable = YesOrNo;

            codeGenConfig.ColumnKey = tableColumn.ColumnKey;

            codeGenConfig.DataType = tableColumn.DataType;
            codeGenConfig.EffectType = CodeGenUtil.DataTypeToEff(codeGenConfig.NetType);
            codeGenConfig.QueryType = GetDefaultQueryType(codeGenConfig); // QueryTypeEnum.eq.ToString();
            codeGenConfigs.Add(codeGenConfig);
        }
        // 多库代码生成---这里要切回主库
        var provider = _db.AsTenant().GetConnectionScope(SqlSugarConst.ConfigId);
        provider.Insertable(codeGenConfigs).ExecuteCommand();
    }

    /// <summary>
    /// 默认查询类型
    /// </summary>
    /// <param name="codeGenConfig"></param>
    /// <returns></returns>
    private string GetDefaultQueryType(SysCodeGenConfig codeGenConfig)
    {
        switch (codeGenConfig.NetType?.TrimEnd('?'))
        {
            case "string":
                return "like";

            case "DateTime":
                return "~";

            default:
                return "==";
        }
    }
}