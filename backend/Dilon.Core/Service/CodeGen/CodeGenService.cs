using System;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Furion.ViewEngine;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilon.Core.Service.CodeGen
{
    /// <summary>
    /// 代码生成器服务
    /// </summary>
    [ApiDescriptionSettings(Name = "CodeGen", Order = 100)]
    public class CodeGenService : ICodeGenService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysCodeGen> _sysCodeGenRep; // 代码生成器仓储
        private readonly ICodeGenConfigService _codeGenConfigService;
        private readonly IViewEngine _viewEngine;

        private readonly IRepository<SysMenu> _sysMenuRep; // 菜单表仓储

        public CodeGenService(IRepository<SysCodeGen> sysCodeGenRep,
            ICodeGenConfigService codeGenConfigService,
            IViewEngine viewEngine,
            IRepository<SysMenu> sysMenuRep)
        {
            _sysCodeGenRep = sysCodeGenRep;
            _codeGenConfigService = codeGenConfigService;
            _viewEngine = viewEngine;
            _sysMenuRep = sysMenuRep;
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
            var isExist =
                await _sysCodeGenRep.DetachedEntities.AnyAsync(u => u.TableName == input.TableName && u.Id != input.Id);
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
                TableName = u.GetDefaultTableName(),
                TableComment = u.GetComment()
            }).ToList();
        }

        /// <summary>
        /// 根据表名获取列
        /// </summary>
        /// <returns></returns>
        [HttpGet("/codeGenerate/ColumnList/{tableName}")]
        public List<TableColumnOuput> GetColumnListByTableName(string tableName)
        {
            // 获取实体类型属性
            var entityType = Db.GetDbContext().Model.GetEntityTypes().FirstOrDefault(u => u.ClrType.Name == tableName);
            if (entityType == null) return null;

            // 获取原始类型属性
            var type = entityType.ClrType;
            if (type == null) return null;

            // 按原始类型的顺序获取所有实体类型属性（不包含导航属性，会返回null）
            return type.GetProperties()
                .Select(propertyInfo => entityType.FindProperty(propertyInfo.Name))
                .Where(p => p != null)
                .Select(p => new TableColumnOuput
                {
                    ColumnName = p.Name,
                    ColumnKey = p.IsKey().ToString(),
                    DataType = p.PropertyInfo.PropertyType.ToString(),
                    NetType = GenCodeUtil.ConvertDataType(p.PropertyInfo.PropertyType.ToString()),
                    ColumnComment = p.GetComment()
                }).ToList();
        }

        /// <summary>
        /// 获取数据表列（实体属性）集合
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public List<TableColumnOuput> GetColumnList([FromQuery] AddCodeGenInput input)
        {
            // 获取实体类型属性
            var entityType = Db.GetDbContext().Model.GetEntityTypes()
                .FirstOrDefault(u => u.ClrType.Name == input.TableName);
            if (entityType == null) return null;

            // 获取原始类型属性
            var type = entityType.ClrType;
            if (type == null) return null;

            // 按原始类型的顺序获取所有实体类型属性（不包含导航属性，会返回null）
            return type.GetProperties()
                .Select(propertyInfo => entityType.FindProperty(propertyInfo.Name))
                .Where(p => p != null)
                .Select(p => new TableColumnOuput
                {
                    ColumnName = p.Name,
                    ColumnKey = p.IsKey().ToString(),
                    DataType = p.PropertyInfo.PropertyType.ToString(),
                    ColumnComment = p.GetComment()
                }).ToList();
        }

        /// <summary>
        /// 代码生成_本地项目
        /// </summary>
        /// <returns></returns>
        [HttpPost("/codeGenerate/runLocal")]
        public async Task RunLocal(SysCodeGen input)
        {
            var templatePathList = GetTemplatePathList();
            var targetPathList = GetTargetPathList(input);
            for (var i = 0; i < templatePathList.Count; i++)
            {
                var tContent = File.ReadAllText(templatePathList[i]);

                var tableFieldList =
                    await _codeGenConfigService.List(new CodeGenConfig() {CodeGenId = input.Id}); // 字段集合
                if (i >= 5) // 适应前端首字母小写
                {
                    tableFieldList.ForEach(u =>
                    {
                        u.ColumnName = u.ColumnName.Substring(0, 1).ToLower() + u.ColumnName[1..];
                    });
                }

                var queryWhetherList =
                    tableFieldList.Where(u => u.QueryWhether == YesOrNot.Y.ToString()).ToList(); // 前端查询集合
                var tResult = _viewEngine.RunCompileFromCached(tContent, new
                {
                    input.AuthorName,
                    input.BusName,
                    input.NameSpace,
                    ClassName = input.TableName,
                    QueryWhetherList = queryWhetherList,
                    TableField = tableFieldList
                });

                var dirPath = new DirectoryInfo(targetPathList[i]).Parent.FullName;
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                File.WriteAllText(targetPathList[i], tResult, Encoding.UTF8);
            }

            await AddMenu(input.TableName, input.BusName);
        }

        private async Task AddMenu(string className, string busName)
        {
            // 先删除该表已生成的菜单列表
            var menus = await _sysMenuRep.DetachedEntities.Where(u => u.Code.StartsWith("dilon_" + className.ToLower()))
                .ToListAsync();
            menus.ForEach(u => { u.Delete(); });

            // 目录
            var menuType0 = new SysMenu
            {
                Pid = 0,
                Pids = "[0],",
                Name = busName + "管理",
                Code = "dilon_" + className.ToLower(),
                Type = 1,
                Icon = "robot",
                Router = "/" + className.ToLower(),
                Component = "PageView",
                Application = "busapp"
            };
            var pid0 = _sysMenuRep.InsertNowAsync(menuType0).GetAwaiter().GetResult().Entity.Id;

            // 菜单
            var menuType1 = new SysMenu
            {
                Pid = pid0,
                Pids = "[0],[" + pid0 + "],",
                Name = busName + "管理",
                Code = "dilon_" + className.ToLower() + "_mgr",
                Type = 1,
                Router = "/" + className.ToLower(),
                Component = "main/" + className + "/index",
                Application = "busapp",
                OpenType = 1
            };
            var pid1 = _sysMenuRep.InsertNowAsync(menuType1).GetAwaiter().GetResult().Entity.Id;

            // 按钮-page
            var menuType2 = new SysMenu
            {
                Pid = pid1,
                Pids = "[0],[" + pid0 + "],[" + pid1 + "],",
                Name = busName + "查询",
                Code = "dilon_" + className.ToLower() + "_mgr_page",
                Type = 2,
                Permission = className + ":page",
                Application = "busapp",
            }.InsertAsync();

            // 按钮-detail
            var menuType2_1 = new SysMenu
            {
                Pid = pid1,
                Pids = "[0],[" + pid0 + "],[" + pid1 + "],",
                Name = busName + "详情",
                Code = "dilon_" + className.ToLower() + "_mgr_detail",
                Type = 2,
                Permission = className + ":detail",
                Application = "busapp",
            }.InsertAsync();

            // 按钮-add
            var menuType2_2 = new SysMenu
            {
                Pid = pid1,
                Pids = "[0],[" + pid0 + "],[" + pid1 + "],",
                Name = busName + "增加",
                Code = "dilon_" + className.ToLower() + "_mgr_add",
                Type = 2,
                Permission = className + ":add",
                Application = "busapp",
            }.InsertAsync();

            // 按钮-delete
            var menuType2_3 = new SysMenu
            {
                Pid = pid1,
                Pids = "[0],[" + pid0 + "],[" + pid1 + "],",
                Name = busName + "删除",
                Code = "dilon_" + className.ToLower() + "_mgr_delete",
                Type = 2,
                Permission = className + ":delete",
                Application = "busapp",
            }.InsertAsync();

            // 按钮-edit
            var menuType2_4 = new SysMenu
            {
                Pid = pid1,
                Pids = "[0],[" + pid0 + "],[" + pid1 + "],",
                Name = busName + "编辑",
                Code = "dilon_" + className.ToLower() + "_mgr_edit",
                Type = 2,
                Permission = className + ":edit",
                Application = "busapp",
            }.InsertAsync();
        }

        /// <summary>
        /// 获取模板文件路径集合
        /// </summary>
        /// <returns></returns>
        private List<string> GetTemplatePathList()
        {
            var templatePath = App.WebHostEnvironment.WebRootPath + @"\Template\";
            return new List<string>()
            {
                templatePath + "Service.cs.vm",
                templatePath + "IService.cs.vm",
                templatePath + "Input.cs.vm",
                templatePath + "Output.cs.vm",
                templatePath + "Dto.cs.vm",
                templatePath + "index.vue.vm",
                templatePath + "addForm.vue.vm",
                templatePath + "editForm.vue.vm",
                templatePath + "manage.js.vm",
            };
        }

        /// <summary>
        /// 设置生成文件路径
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private List<string> GetTargetPathList(SysCodeGen input)
        {
            var backendPath = new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.FullName +
                              @"\Dilon.Application\Service\" + input.TableName + @"\";
            var servicePath = backendPath + input.TableName + "Service.cs";
            var iservicePath = backendPath + "I" + input.TableName + "Service.cs";
            var inputPath = backendPath + @"Dto\" + input.TableName + "Input.cs";
            var outputPath = backendPath + @"Dto\" + input.TableName + "Output.cs";
            var viewPath = backendPath + @"Dto\" + input.TableName + "Dto.cs";
            var frontendPath = new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.Parent.FullName +
                               @"\frontend\src\views\main\";
            var indexPath = frontendPath + input.TableName + @"\index.vue";
            var addFormPath = frontendPath + input.TableName + @"\addForm.vue";
            var editFormPath = frontendPath + input.TableName + @"\editForm.vue";
            var apiJsPath = new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.Parent.FullName +
                            @"\frontend\src\api\modular\main\" + input.TableName + "Manage.js";

            return new List<string>()
            {
                servicePath,
                iservicePath,
                inputPath,
                outputPath,
                viewPath,
                indexPath,
                addFormPath,
                editFormPath,
                apiJsPath
            };
        }
    }
}