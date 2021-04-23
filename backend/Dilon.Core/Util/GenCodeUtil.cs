using System.Text.RegularExpressions;

namespace Dilon.Core
{
    /// <summary>
    /// 代码生成帮助类
    /// </summary>
    public static class GenCodeUtil
    {
        public static string ConvertDataType(string dataType)
        {
            if (string.IsNullOrEmpty(dataType)) return "";
            if (dataType.StartsWith("System.Nullable"))
                dataType = new Regex(@"(?i)(?<=\[)(.*)(?=\])").Match(dataType).Value; // 中括号[]里面值 

            switch (dataType)
            {
                case "System.Guid": return "Guid";
                case "System.String": return "string";
                case "System.Int32": return "int";
                case "System.Int64": return "long";
                case "System.Single": return "float";
                case "System.Double": return "double";
                case "System.Decimal": return "decimal";
                case "System.Boolean": return "bool";
                case "System.DateTime": return "DateTime";
                case "System.DateTimeOffset": return "DateTimeOffset";
                case "System.Byte": return "byte";
                case "System.Byte[]": return "byte[]";
                default:
                    break;
            }
            return dataType;
        }
    }
}