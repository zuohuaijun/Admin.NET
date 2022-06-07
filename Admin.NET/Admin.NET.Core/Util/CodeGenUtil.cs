namespace Admin.NET.Core.Util;

/// <summary>
/// 代码生成帮助类
/// </summary>
public static class CodeGenUtil
{
    //根据数据库类型来处理对应的数据字段类型
    public static string ConvertDataType(string dataType, string DbType)
    {
        switch (DbType)
        {
            case "PostgreSQL":
                return ConvertDataType_PostgreSQL(dataType);
            default:
                return ConvertDataType(dataType);
        }
    }
    //PostgreSQL数据类型对应的字段类型
    public static string ConvertDataType_PostgreSQL(string dataType)
    {
        switch (dataType)
        {
            case "int2":
            case "smallint":
                return "Int16";
            case "int4":
            case "integer":
                return "int";
            case "int8":
            case "bigint":
                return "long";
            case "float4":
            case "real":
                return "float";
            case "float8":
            case "double precision":
                return "double";
            case "numeric":
            case "decimal":
            case "path":
            case "point":
            case "polygon":
            case "interval":
            case "lseg":
            case "macaddr":
            case "money":
                return "decimal";
            case "boolean":
            case "bool":
            case "box":
            case "bytea":
                return "bool";
            case "varchar":
            case "character varying":
            case "geometry":
            case "name":
            case "text":
            case "char":
            case "character":
            case "cidr":
            case "circle":
            case "tsquery":
            case "tsvector":
            case "txid_snapshot":
            case "xml":
            case "json":
                return "string";
            case "uuid":
                return "Guid";
            case "timestamp":
            case "timestamp with time zone":
            case "timestamptz":
            case "timestamp without time zone":
            case "date":
            case "time":
            case "time with time zone":
            case "timetz":
            case "time without time zone":
                return "DateTime";
            case "bit":
            case "bit varying":
                return "byte[]";
            case "varbit":
                return "byte";
            case "public.geometry":
            case "inet":
                return "object";
            default:
                return "object";
        }
    }

    public static string ConvertDataType(string dataType)
    {
        switch (dataType)
        {
            case "text":
            case "varchar":
            case "char":
            case "nvarchar":
            case "nchar":
            case "timestamp":
                return "string";

            case "int":
                return "int";

            case "smallint":
                return "Int16";

            case "tinyint":
                return "byte";

            case "bigint":
            case "integer": // sqlite数据库
                return "long";

            case "bit":
                return "bool";

            case "money":
            case "smallmoney":
            case "numeric":
            case "decimal":
                return "decimal";

            case "real":
                return "Single";

            case "datetime":
            case "smalldatetime":
                return "DateTime";

            case "float":
                return "double";

            case "image":
            case "binary":
            case "varbinary":
                return "byte[]";

            case "uniqueidentifier":
                return "Guid";

            default:
                return "object";
        }
    }

    /// <summary>
    /// 数据类型转显示类型
    /// </summary>
    /// <param name="dataType"></param>
    /// <returns></returns>
    public static string DataTypeToEff(string dataType)
    {
        if (string.IsNullOrEmpty(dataType)) return "";
        return dataType switch
        {
            "string" => "Input",
            "int" => "InputNumber",
            "long" => "Input",
            "float" => "Input",
            "double" => "Input",
            "decimal" => "Input",
            "bool" => "Switch",
            "Guid" => "Input",
            "DateTime" => "DatePicker",
            _ => "Input",
        };
    }

    // 是否通用字段
    public static bool IsCommonColumn(string columnName)
    {
        var columnList = new List<string>()
        {
            "CreateTime", "UpdateTime", "CreateUserId", "UpdateUserId", "IsDelete"
        };
        return columnList.Contains(columnName);
    }
}