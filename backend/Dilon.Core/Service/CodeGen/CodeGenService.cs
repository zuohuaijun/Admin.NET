using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dilon.Core.Service.CodeGen
{
    /// <summary>
    /// 代码生成器服务
    /// </summary>
    [ApiDescriptionSettings(Name = "CodeGen", Order = 100)]
    public class CodeGenService : IDynamicApiController, ITransient
    {
        private readonly IRepository<SysCodeGen> _sysCodeGenRep;    // 代码生成器仓储
        private readonly CodeGenConfigService _codeGenConfigService;

        public CodeGenService(IRepository<SysCodeGen> sysCodeGenRep, CodeGenConfigService codeGenConfigService)
        {
            _sysCodeGenRep = sysCodeGenRep;
            _codeGenConfigService = codeGenConfigService;
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
            var newCodeGen = await codeGen.InsertNowAsync();

            // 加入配置表中
            _codeGenConfigService.AddList(GetColumnList(input), newCodeGen.Entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        [HttpPost("/codeGenerate/delete")]
        public async Task DeleteCodeGen(List<DeleteCodeGenInput> inputs)
        {
            if (inputs == null || inputs.Count < 1) return;

            var codeGenConfigTaskList = new List<Task>();
            inputs.ForEach(u =>
            {
                _sysCodeGenRep.Delete(u.Id);

                // 删除配置表中
                codeGenConfigTaskList.Add(_codeGenConfigService.Delete(u.Id));
            });
            await Task.WhenAll(codeGenConfigTaskList);
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
        [HttpGet("/codeGenerate/detail")]
        public async Task<SysCodeGen> GetCodeGen([FromQuery] QueryCodeGenInput input)
        {
            return await _sysCodeGenRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
        }

        /// <summary>
        /// 获取数据库表(实体)集合
        /// </summary>
        /// <returns></returns>

        [HttpGet("/codeGenerate/InformationList")]
        public List<TableOutput> GetTableList()
        {
            return Db.GetDbContext().Model.GetEntityTypes().Select(u => new TableOutput
            {
                TableName = u.GetDefaultTableName()
            }).ToList();
        }

        /// <summary>
        /// 获取数据表列（实体属性）集合
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public List<TableColumnOuput> GetColumnList(AddCodeGenInput input)
        {
            var entityType = Db.GetDbContext().Model.GetEntityTypes().FirstOrDefault(u => u.ClrType.Name == input.TableName);
            if (entityType == null) return null;

            return entityType.GetProperties().Select(u => new TableColumnOuput
            {
                ColumnName = u.Name,
                ColumnKey = u.IsKey().ToString(),
                DataType = u.PropertyInfo.PropertyType.ToString(),
                ColumnComment = u.GetComment()
            }).ToList();
        }

        /// <summary>
        /// 代码生成_本地项目
        /// </summary>
        /// <returns></returns>
        [HttpPost("/codeGenerate/runLocal")]
        public async void RunLocal(SysCodeGen input)
        {
            List<CodeGenConfig> codeGenConfigList = await _codeGenConfigService.List(new CodeGenConfig() { CodeGenId = input.Id });

            var vltEngine = new VelocityEngine();
            vltEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            vltEngine.SetProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            vltEngine.SetProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
            vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, App.WebHostEnvironment.WebRootPath + "\\Template");
            vltEngine.Init();

            VelocityContext vltContext = new VelocityContext();
            vltContext.Put("queryWhetherList", codeGenConfigList);      
            Template vltTemplate = vltEngine.GetTemplate("index.vue.vm");
            StringWriter vltWriter = new StringWriter();
            vltTemplate.Merge(vltContext, vltWriter);
            string CodeContent = vltWriter.GetStringBuilder().ToString();
            string CodeFilePath = "C:\\1.vue";

            File.WriteAllText(CodeFilePath, CodeContent);


        }

    }
}
