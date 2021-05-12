namespace Admin.NET.Core.Service
{
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
        /// 数据库字段名(首字母小写)
        /// </summary>
        public string LowerColumnName => string.IsNullOrWhiteSpace(ColumnName)
                                      ? null
                                      : ColumnName.Substring(0, 1).ToLower() + ColumnName[1..];

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
        /// 外键实体名称(首字母小写)
        /// </summary>
        public string LowerFkEntityName => string.IsNullOrWhiteSpace(FkEntityName)
                                        ? null
                                        : FkEntityName.Substring(0, 1).ToLower() + FkEntityName[1..];

        /// <summary>
        /// 外键显示字段
        /// </summary>
        public string FkColumnName { get; set; }

        /// <summary>
        /// 外键显示字段(首字母小写)
        /// </summary>
        public string LowerFkColumnName => string.IsNullOrWhiteSpace(FkColumnName)
                                        ? null
                                        : (FkColumnName.Substring(0, 1).ToLower() + FkColumnName[1..]);

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
        /// 列表排序显示
        /// </summary>
        public string WhetherOrderBy { get; set; }

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
    }
}