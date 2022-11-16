namespace Admin.NET.Core.Service;

/// <summary>
/// 系统代码生成配置服务
/// </summary>
[ApiDescriptionSettings(Order = 149)]
public class SysCodeGenConfigService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;

    public SysCodeGenConfigService(ISqlSugarClient db)
    {
        _db = db;
    }

    /// <summary>
    /// 代码生成详细配置列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysCodeGenerateConfig/list")]
    public async Task<List<CodeGenConfig>> List([FromQuery] CodeGenConfig input)
    {
        return await _db.Queryable<SysCodeGenConfig>()
            .Where(u => u.CodeGenId == input.CodeGenId && u.WhetherCommon != YesNoEnum.Y.ToString())
            .Select<CodeGenConfig>().ToListAsync();


    }

    /// <summary>
    /// 增加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task Add(CodeGenConfig input)
    {
        var codeGenConfig = input.Adapt<SysCodeGenConfig>();
        await _db.Insertable(codeGenConfig).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="codeGenId"></param>
    /// <returns></returns>
    [NonAction]
    public async Task Delete(long codeGenId)
    {
        await _db.Deleteable<SysCodeGenConfig>().Where(u => u.CodeGenId == codeGenId).ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="inputList"></param>
    /// <returns></returns>
    [HttpPost("/sysCodeGenerateConfig/edit")]
    public async Task Update(List<CodeGenConfig> inputList)
    {
        if (inputList == null || inputList.Count < 1) return;
        await _db.Updateable(inputList.Adapt<List<SysCodeGenConfig>>()).ExecuteCommandAsync();
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysCodeGenerateConfig/detail")]
    public async Task<SysCodeGenConfig> Detail([FromQuery] CodeGenConfig input)
    {
        return await _db.Queryable<SysCodeGenConfig>().FirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 批量增加
    /// </summary>
    /// <param name="tableColumnOuputList"></param>
    /// <param name="codeGenerate"></param>
    [NonAction]
    public void AddList(List<TableColumnOuput> tableColumnOuputList, SysCodeGen codeGenerate)
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
            codeGenConfig.NetType = CodeGenUtil.ConvertDataType(tableColumn.DataType);
            codeGenConfig.WhetherRetract = YesNoEnum.N.ToString();

            codeGenConfig.WhetherRequired = YesNoEnum.N.ToString();
            codeGenConfig.QueryWhether = YesOrNo;
            codeGenConfig.WhetherAddUpdate = YesOrNo;
            codeGenConfig.WhetherTable = YesOrNo;

            codeGenConfig.ColumnKey = tableColumn.ColumnKey;

            codeGenConfig.DataType = tableColumn.DataType;
            codeGenConfig.EffectType = CodeGenUtil.DataTypeToEff(codeGenConfig.NetType);
            codeGenConfig.QueryType = "=="; // QueryTypeEnum.eq.ToString();
            codeGenConfigs.Add(codeGenConfig);
        }
        // 多库代码生成---这里要切回主库
        var provider = _db.AsTenant().GetConnectionScope(SqlSugarConst.ConfigId);
        provider.Insertable(codeGenConfigs).ExecuteCommand();
    }
}