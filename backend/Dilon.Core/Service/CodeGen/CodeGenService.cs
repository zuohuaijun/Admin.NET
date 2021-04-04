using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dilon.Core.Service.CodeGen
{
    /// <summary>
    /// 代码生成器服务
    /// </summary>
    [ApiDescriptionSettings(Name = "CodeGen", Order = 100)]
    public class CodeGenService : IDynamicApiController, ITransient
    {
        private static string TEMP_SUFFIX = ".vm"; // 模板后缀
        private static string ENCODED = "UTF-8"; // 转换的编码

        //转换模板名称所需变量
        private static string ADD_FORM_PAGE_NAME = "addForm.vue";
        private static string EDIT_FORM_PAGE_NAME = "editForm.vue";
        private static string INDEX_PAGE_NAME = "index.vue";
        private static string MANAGE_JS_NAME = "Manage.js";
        private static string SQL_NAME = ".sql";
        private static string JAVA_SUFFIX = ".java";
        private static string TEMP_ENTITY_NAME = "entity";

        private readonly IRepository<SysCodeGen> _sysCodeGenRep;    // 代码生成器仓储

        public CodeGenService(IRepository<SysCodeGen> sysCodeGenRep)
        {
            _sysCodeGenRep = sysCodeGenRep;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/codeGenerate/page")]
        public async Task<dynamic> QueryCodeGenPageList([FromQuery] CodeGenInput input)
        {
            var tableName = !string.IsNullOrEmpty(input.TableName?.Trim());
            var codeGens = await _sysCodeGenRep.DetachedEntities
                                               .Where((tableName, u => EF.Functions.Like(u.TableName, $"%{input.TableName.Trim()}%")))
                                               .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<SysCodeGen>.PageResult(codeGens);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/codeGenerate/add")]
        public async Task AddCodeGen(AddCodeGenInput input)
        {
            var isExist = await _sysCodeGenRep.DetachedEntities.AnyAsync(u => u.TableName == input.TableName);
            if (isExist)
                throw Oops.Oh(ErrorCode.D1400);

            var codeGen = input.Adapt<SysCodeGen>();
            await codeGen.InsertAsync();

            //// 加入配置表中
            //codeGenerateParam.setId(codeGenerate.getId());
            //this.sysCodeGenerateConfigService.addList(this.getInforMationColumnsResultList(codeGenerateParam), codeGenerate);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/codeGenerate/delete")]
        public async Task DeleteCodeGen(DeleteCodeGenInput input)
        {
            await _sysCodeGenRep.DeleteAsync(input.Id);

            //// 删除配置表中
            //codeGenerateParam.setId(codeGenerate.getId());
            //this.sysCodeGenerateConfigService.addList(this.getInforMationColumnsResultList(codeGenerateParam), codeGenerate);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/codeGenerate/edit")]
        public async Task UpdateCodeGen(UpdateCodeGenInput input)
        {
            var isExist = await _sysCodeGenRep.DetachedEntities.AnyAsync(u => u.TableName == input.TableName && u.Id != input.Id);
            if (isExist)
                throw Oops.Oh(ErrorCode.D1400);

            var codeGen = input.Adapt<SysCodeGen>();
            await codeGen.UpdateAsync();
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysCodeGen/detail")]
        public async Task<SysCodeGen> GetCodeGen([FromQuery] QueryCodeGenInput input)
        {
            return await _sysCodeGenRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
        }

        /// <summary>
        /// 获取数据库表集合
        /// </summary>
        /// <returns></returns>

        [HttpGet("/codeGenerate/InformationList")]
        public static List<TableOutput> GetTableList()
        {
            var tableNames = new List<TableOutput>();

            var entityTypes = Db.GetDbContext().Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
                var tableOutput = new TableOutput
                {
                    TableName = entityType.ClrType.Name
                };
                tableNames.Add(tableOutput);
            }
            return tableNames;
        }

        /// <summary>
        /// 代码生成_本地项目
        /// </summary>
        /// <returns></returns>
        [HttpGet("/codeGenerate/runLocal")]
        public void RunLocal(CodeGenInput input)
        {
            XnCodeGenOutput xnCodeGenParam = input.Adapt<XnCodeGenOutput>();
            xnCodeGenParam.FunctionName = input.TableComment;
            xnCodeGenParam.ConfigList = null;
            xnCodeGenParam.CreateTimestring = DateTimeOffset.Now.ToString();
        }
    }
}
