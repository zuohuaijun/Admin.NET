// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

/// <summary>
/// 代码生成详细配置参数
/// </summary>
public class CodeGenConfig
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 代码生成主表ID
    /// </summary>
    public long CodeGenId { get; set; }

    /// <summary>
    /// 数据库字段名
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// 实体属性名
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// 字段数据长度
    /// </summary>
    public int ColumnLength { get; set; }

    /// <summary>
    /// 数据库字段名(首字母小写)
    /// </summary>
    public string LowerPropertyName => string.IsNullOrWhiteSpace(PropertyName) ? null : PropertyName[..1].ToLower() + PropertyName[1..];

    /// <summary>
    /// 字段描述
    /// </summary>
    public string ColumnComment { get; set; }

    /// <summary>
    /// .NET类型
    /// </summary>
    public string NetType { get; set; }

    /// <summary>
    /// 作用类型（字典）
    /// </summary>
    public string EffectType { get; set; }

    /// <summary>
    /// 外键实体名称
    /// </summary>
    public string FkEntityName { get; set; }

    /// <summary>
    /// 外键表名称
    /// </summary>
    public string FkTableName { get; set; }

    /// <summary>
    /// 外键实体名称(首字母小写)
    /// </summary>
    public string LowerFkEntityName =>
        string.IsNullOrWhiteSpace(FkEntityName) ? null : FkEntityName[..1].ToLower() + FkEntityName[1..];

    /// <summary>
    /// 外键显示字段
    /// </summary>
    public string FkColumnName { get; set; }

    /// <summary>
    /// 外键显示字段(首字母小写)
    /// </summary>
    public string LowerFkColumnName =>
        string.IsNullOrWhiteSpace(FkColumnName) ? null : FkColumnName[..1].ToLower() + FkColumnName[1..];

    /// <summary>
    /// 外键显示字段.NET类型
    /// </summary>
    public string FkColumnNetType { get; set; }

    /// <summary>
    /// 字典code
    /// </summary>
    public string DictTypeCode { get; set; }

    /// <summary>
    /// 列表是否缩进（字典）
    /// </summary>
    public string WhetherRetract { get; set; }

    /// <summary>
    /// 是否必填（字典）
    /// </summary>
    public string WhetherRequired { get; set; }

    /// <summary>
    /// 是否是查询条件
    /// </summary>
    public string QueryWhether { get; set; }

    /// <summary>
    /// 查询方式
    /// </summary>
    public string QueryType { get; set; }

    /// <summary>
    /// 列表显示
    /// </summary>
    public string WhetherTable { get; set; }

    /// <summary>
    /// 增改
    /// </summary>
    public string WhetherAddUpdate { get; set; }

    /// <summary>
    /// 主外键
    /// </summary>
    public string ColumnKey { get; set; }

    /// <summary>
    /// 数据库中类型（物理类型）
    /// </summary>
    public string DataType { get; set; }

    /// <summary>
    /// 是否是通用字段
    /// </summary>
    public string WhetherCommon { get; set; }

    /// <summary>
    /// 表的别名 Table as XXX
    /// </summary>
    public string TableNickName
    {
        get
        {
            string str = "";
            if (EffectType == "fk")
            {
                str = LowerFkEntityName + "_FK_" + LowerFkColumnName;
            }
            else if (EffectType == "Upload")
            {
                str = "sysFile_FK_" + LowerPropertyName;
            }
            return str;
        }
    }

    /// <summary>
    /// 显示文本字段
    /// </summary>
    public string DisplayColumn { get; set; }

    /// <summary>
    /// 选中值字段
    /// </summary>
    public string ValueColumn { get; set; }

    /// <summary>
    /// 父级字段
    /// </summary>
    public string PidColumn { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int OrderNo { get; set; }
}