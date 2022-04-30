using System.Collections.Generic;

namespace Admin.NET.Core.Util
{
    /// <summary>
    /// 代码生成帮助类
    /// </summary>
    public static class CodeGenUtil
    {
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
}