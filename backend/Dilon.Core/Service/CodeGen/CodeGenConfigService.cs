using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 代码生成详细配置服务
    /// </summary>
    public class CodeGenConfigService : IDynamicApiController, ITransient
    {
        private readonly IRepository<SysCodeGenConfig> _sysCodeGenConfigRep;    // 代码生成详细配置仓储

        public CodeGenConfigService(IRepository<SysCodeGenConfig> sysCodeGenConfigRep)
        {
            _sysCodeGenConfigRep = sysCodeGenConfigRep;
        }

        /// <summary>
        /// 代码生成详细配置列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpGet("/sysCodeGenerateConfig/list")]
        public async Task<List<CodeGenConfig>> List([FromQuery] CodeGenConfig input)
        {
            return await _sysCodeGenConfigRep.DetachedEntities.Where(u => u.CodeGenId == input.CodeGenId && u.WhetherCommon != YesOrNot.Y.ToString())
                                                              .Select(u=>u.Adapt<CodeGenConfig>()).ToListAsync();
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [NonAction]
        public async Task Add(CodeGenConfig input)
        {
            var codeGenConfig = input.Adapt<SysCodeGenConfig>();
            await codeGenConfig.InsertAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="codeGenId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task Delete(long codeGenId)
        {
            var codeGenConfigList = await _sysCodeGenConfigRep.Where(u => u.CodeGenId == codeGenId).ToListAsync();
            codeGenConfigList.ForEach(u =>
            {
                u.Delete();
            });
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
            inputList.ForEach(u =>
            {
                var codeGenConfig = u.Adapt<SysCodeGenConfig>();
                codeGenConfig.Update(true);
            });
            await Task.CompletedTask;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysCodeGenerateConfig/detail")]
        public async Task<SysCodeGenConfig> Detail(CodeGenConfig input)
        {
            return await _sysCodeGenConfigRep.FirstOrDefaultAsync(u => u.Id == input.Id);
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

            foreach (var tableColumn in tableColumnOuputList)
            {
                var codeGenConfig = new SysCodeGenConfig();

                var YesOrNo = YesOrNot.Y.ToString();
                if (Convert.ToBoolean(tableColumn.ColumnKey))
                {
                    YesOrNo = YesOrNot.N.ToString();
                }
                
                if(IsCommonColumn(tableColumn.ColumnName))
                {
                    codeGenConfig.WhetherCommon = YesOrNot.Y.ToString();
                    YesOrNo = YesOrNot.N.ToString();
                }
                else
                {
                    codeGenConfig.WhetherCommon = YesOrNot.N.ToString();
                }

                codeGenConfig.CodeGenId = codeGenerate.Id;
                codeGenConfig.ColumnName = tableColumn.ColumnName;
                codeGenConfig.ColumnComment = tableColumn.ColumnComment;
                codeGenConfig.NetType = ConvertDataType(tableColumn.DataType);
                codeGenConfig.WhetherRetract = YesOrNot.N.ToString();

                codeGenConfig.WhetherRequired = YesOrNot.N.ToString();
                codeGenConfig.QueryWhether = YesOrNo;
                codeGenConfig.WhetherAddUpdate = YesOrNo;
                codeGenConfig.WhetherTable = YesOrNo;

                codeGenConfig.ColumnKey = tableColumn.ColumnKey;

                codeGenConfig.DataType = tableColumn.DataType;
                codeGenConfig.EffectType = DataTypeToEff(codeGenConfig.NetType);
                codeGenConfig.QueryType = QueryTypeEnum.eq.ToString();

                codeGenConfig.InsertAsync();
            }
        }

        /// <summary>
        /// 数据类型转显示类型
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private static string DataTypeToEff(string dataType)
        {
            if (string.IsNullOrEmpty(dataType)) return "";
            return dataType switch
            {
                "string" => "input",
                "int" => "inputnumber",
                "long" => "input",
                "float" => "input",
                "double" => "input",
                "decimal" => "input",
                "bool" => "switch",
                "Guid" => "input",
                "DateTime" => "datepicker",
                "DateTimeOffset" => "datepicker",
                _ => "input",
            };
        }

        // 转换.NET数据类型
        [NonAction]
        public string ConvertDataType(string dataType)
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

        // 是否通用字段
        private static bool IsCommonColumn(string columnName)
        {
            var columnList = new List<string>() { "CreatedTime", "UpdatedTime", "CreatedUserId", "CreatedUserName", "UpdatedUserId", "UpdatedUserName", "IsDeleted"};
            return columnList.Contains(columnName);
        }
    }
}
