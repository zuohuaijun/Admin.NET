// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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
        return await _db.Queryable<SysCodeGenConfig>()
            .Where(u => u.CodeGenId == input.CodeGenId)
            .Select<CodeGenConfig>()
            .Mapper(u =>
            {
                u.NetType = (u.EffectType == "EnumSelector" || u.EffectType == "ConstSelector" ? u.DictTypeCode : u.NetType);
            })
            .OrderBy(u => u.OrderNo)
            .ToListAsync();
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
        await _db.Updateable(inputList.Adapt<List<SysCodeGenConfig>>())
            .IgnoreColumns(u => new { u.ColumnLength, u.ColumnName, u.PropertyName })
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除代码生成配置
    /// </summary>
    /// <param name="codeGenId"></param>
    /// <returns></returns>
    [NonAction]
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
    /// <param name="tableColumnOutputList"></param>
    /// <param name="codeGenerate"></param>
    [NonAction]
    public void AddList(List<ColumnOuput> tableColumnOutputList, SysCodeGen codeGenerate)
    {
        if (tableColumnOutputList == null) return;

        var codeGenConfigs = new List<SysCodeGenConfig>();
        var orderNo = 100;
        foreach (var tableColumn in tableColumnOutputList)
        {
            var codeGenConfig = new SysCodeGenConfig();

            var YesOrNo = YesNoEnum.Y.ToString();
            if (Convert.ToBoolean(tableColumn.ColumnKey))
            {
                YesOrNo = YesNoEnum.N.ToString();
            }

            if (CodeGenUtil.IsCommonColumn(tableColumn.PropertyName))
            {
                codeGenConfig.WhetherCommon = YesNoEnum.Y.ToString();
                YesOrNo = YesNoEnum.N.ToString();
            }
            else
            {
                codeGenConfig.WhetherCommon = YesNoEnum.N.ToString();
            }

            codeGenConfig.CodeGenId = codeGenerate.Id;
            codeGenConfig.ColumnName = tableColumn.ColumnName; // 字段名
            codeGenConfig.PropertyName = tableColumn.PropertyName;// 实体属性名
            codeGenConfig.ColumnLength = tableColumn.ColumnLength;// 长度
            codeGenConfig.ColumnComment = tableColumn.ColumnComment;
            codeGenConfig.NetType = tableColumn.DataType;
            codeGenConfig.WhetherRetract = YesNoEnum.N.ToString();

            // 生成代码时，主键并不是必要输入项，故一定要排除主键字段
            codeGenConfig.WhetherRequired = (tableColumn.IsNullable || tableColumn.IsPrimarykey) ? YesNoEnum.N.ToString() : YesNoEnum.Y.ToString();
            codeGenConfig.QueryWhether = YesOrNo;
            codeGenConfig.WhetherAddUpdate = YesOrNo;
            codeGenConfig.WhetherTable = YesOrNo;

            codeGenConfig.ColumnKey = tableColumn.ColumnKey;

            codeGenConfig.DataType = tableColumn.DataType;
            codeGenConfig.EffectType = CodeGenUtil.DataTypeToEff(codeGenConfig.NetType);
            codeGenConfig.QueryType = GetDefaultQueryType(codeGenConfig); // QueryTypeEnum.eq.ToString();
            codeGenConfig.OrderNo = orderNo;
            codeGenConfigs.Add(codeGenConfig);

            orderNo += 10; // 每个配置排序间隔10
        }
        // 多库代码生成---这里要切回主库
        var provider = _db.AsTenant().GetConnectionScope(SqlSugarConst.MainConfigId);
        provider.Insertable(codeGenConfigs).ExecuteCommand();
    }

    /// <summary>
    /// 默认查询类型
    /// </summary>
    /// <param name="codeGenConfig"></param>
    /// <returns></returns>
    private string GetDefaultQueryType(SysCodeGenConfig codeGenConfig)
    {
        return (codeGenConfig.NetType?.TrimEnd('?')) switch
        {
            "string" => "like",
            "DateTime" => "~",
            _ => "==",
        };
    }
}