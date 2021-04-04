using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<List<SysCodeGenConfig>> List(CodeGenConfigDto input)
        {
            return await _sysCodeGenConfigRep.DetachedEntities.Where(u => u.CodeGenId == input.CodeGenId).ToListAsync();
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [NonAction]
        public async Task Add(CodeGenConfigDto input)
        {
            var codeGenConfig = input.Adapt<SysCodeGenConfig>();
            await codeGenConfig.InsertAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [NonAction]
        public async Task Delete(CodeGenConfigDto input)
        {
            var codeGenConfigList = await _sysCodeGenConfigRep.Where(u => u.CodeGenId == input.CodeGenId).ToListAsync();
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
        public async Task Update(List<CodeGenConfigDto> inputList)
        {
            if (inputList == null || inputList.Count < 1) return;
            inputList.ForEach(u =>
            {
                var codeGenConfig = u.Adapt<SysCodeGenConfig>();
                codeGenConfig.Update(false);
            });
            await Task.CompletedTask;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysCodeGenerateConfig/detail")]
        public async Task<SysCodeGenConfig> Detail(CodeGenConfigDto input)
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
            foreach (var tableColumn in tableColumnOuputList)
            {
                var sysCodeGenerateConfig = new SysCodeGenConfig();

                var YesOrNo = YesOrNot.Y.ToString();
                if (string.IsNullOrEmpty(tableColumn.ColumnKey) && tableColumn.ColumnKey.Equals(Config.DB_TABLE_COM_KRY))
                {
                    YesOrNo = YesOrNot.N.ToString();
                }
                else
                {
                    sysCodeGenerateConfig.WhetherCommon = YesOrNot.N.ToString();
                }

                sysCodeGenerateConfig.CodeGenId = codeGenerate.Id;
                sysCodeGenerateConfig.ColumnName = tableColumn.ColumnName;
                sysCodeGenerateConfig.ColumnComment = tableColumn.ColumnComment;
                sysCodeGenerateConfig.JavaName = tableColumn.ColumnName + codeGenerate.TablePrefix;
                sysCodeGenerateConfig.JavaType = tableColumn.DataType; // JavaSqlTool.sqlToJava(tableColumn.DataType);
                sysCodeGenerateConfig.WhetherRetract = YesOrNot.N.ToString();

                sysCodeGenerateConfig.WhetherRequired = YesOrNo;
                sysCodeGenerateConfig.QueryWhether = YesOrNo;
                sysCodeGenerateConfig.WhetherAddUpdate = YesOrNo;
                sysCodeGenerateConfig.WhetherTable = YesOrNo;

                sysCodeGenerateConfig.ColumnKey = tableColumn.ColumnKey;

                // 设置get set方法使用的名称
                var columnName = sysCodeGenerateConfig.ColumnName;
                sysCodeGenerateConfig.ColumnKeyName = columnName.Substring(0, 1).ToUpper() + columnName.Substring(1, columnName.Length);

                sysCodeGenerateConfig.DataType = tableColumn.DataType;
                sysCodeGenerateConfig.EffectType = sysCodeGenerateConfig.JavaType;
                sysCodeGenerateConfig.QueryType = QueryTypeEnum.eq.ToString();

                sysCodeGenerateConfig.InsertAsync();
            }
        }
    }
}
