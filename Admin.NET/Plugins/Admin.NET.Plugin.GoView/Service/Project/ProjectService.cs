namespace Admin.NET.Plugin.GoView.Service;

/// <summary>
/// 项目管理服务
/// </summary>
[UnifyProvider("GoView")]
[ApiDescriptionSettings(GoViewConst.GroupName, Order = 100)]
public class ProjectService : IDynamicApiController
{
    private readonly SqlSugarRepository<GoViewProject> _goViewProjectRep;
    private readonly SqlSugarRepository<GoViewProjectData> _goViewProjectDataRep;
    private readonly SqlSugarRepository<SysFile> _sysFileRep;
    private readonly SysFileService _fileService;

    public ProjectService(SqlSugarRepository<GoViewProject> goViewProjectRep,
        SqlSugarRepository<GoViewProjectData> goViewProjectDataRep,
        SqlSugarRepository<SysFile> fileRep,
        SysFileService fileService)
    {
        _goViewProjectRep = goViewProjectRep;
        _goViewProjectDataRep = goViewProjectDataRep;
        _sysFileRep = fileRep;
        _fileService = fileService;
    }

    /// <summary>
    /// 获取项目列表
    /// </summary>
    [DisplayName("项目列表")]
    public async Task<List<ProjectItemOutput>> GetList([FromQuery] int page = 1, [FromQuery] int limit = 12)
    {
        var pagedList = await _goViewProjectRep.AsQueryable()
            .Select(u => new ProjectItemOutput(), true)
            .ToPagedListAsync(page, limit);

        UnifyContext.Fill(pagedList.Total);
        return pagedList.Items.ToList();
    }

    /// <summary>
    /// 新增项目
    /// </summary>
    [ApiDescriptionSettings(Name = "Create")]
    [DisplayName("新增项目")]
    public async Task<ProjectCreateOutput> Create(ProjectCreateInput input)
    {
        var project = input.Adapt<GoViewProject>();
        project.State = GoViewProjectState.UnPublish;

        project = await _goViewProjectRep.AsInsertable(project).ExecuteReturnEntityAsync();

        return new ProjectCreateOutput
        {
            Id = project.Id
        };
    }

    /// <summary>
    /// 获取项目
    /// </summary>
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetData")]
    [DisplayName("获取项目")]
    public async Task<ProjectDetailOutput> GetData([FromQuery] long projectId)
    {
        var projectData = await _goViewProjectDataRep.GetFirstAsync(u => u.Id == projectId);
        if (projectData == null) return null;

        var project = await _goViewProjectRep.GetFirstAsync(u => u.Id == projectId);

        var projectDetail = project.Adapt<ProjectDetailOutput>();
        projectDetail.Content = projectData.Content;

        return projectDetail;
    }

    /// <summary>
    /// 保存项目
    /// </summary>
    [ApiDescriptionSettings(Name = "save/data")]
    [DisplayName("保存项目")]
    public async Task SaveData([FromForm] ProjectSaveDataInput input)
    {
        if (await _goViewProjectDataRep.IsAnyAsync(u => u.Id == input.ProjectId))
        {
            await _goViewProjectDataRep.AsUpdateable()
                .SetColumns(u => new GoViewProjectData
                {
                    Content = input.Content
                })
                .Where(u => u.Id == input.ProjectId)
                .ExecuteCommandAsync();
        }
        else
        {
            await _goViewProjectDataRep.InsertAsync(new GoViewProjectData
            {
                Id = input.ProjectId,
                Content = input.Content,
            });
        }
    }

    /// <summary>
    /// 修改项目
    /// </summary>
    [DisplayName("修改项目")]
    public async Task Edit(ProjectEditInput input)
    {
        // 前端只传修改的字段，更新时需要忽略空列
        var project = await _goViewProjectRep.GetFirstAsync(u => u.Id == input.Id);
        input.Adapt(project);
        await _goViewProjectRep.AsUpdateable(project).IgnoreColumns(true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除项目
    /// </summary>
    [ApiDescriptionSettings(Name = "Delete")]
    [DisplayName("删除项目")]
    [UnitOfWork]
    public async Task Delete([FromQuery] string ids)
    {
        var idList = ids.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(u => Convert.ToInt64(u)).ToList();
        await _goViewProjectRep.AsDeleteable().Where(u => idList.Contains(u.Id)).ExecuteCommandAsync();
        await _goViewProjectDataRep.AsDeleteable().Where(u => idList.Contains(u.Id)).ExecuteCommandAsync();
    }

    /// <summary>
    /// 修改发布状态
    /// </summary>
    [HttpPut]
    [DisplayName("修改发布状态")]
    public async Task Publish(ProjectPublishInput input)
    {
        await _goViewProjectRep.AsUpdateable()
            .SetColumns(u => new GoViewProject
            {
                State = input.State
            })
            .Where(u => u.Id == input.Id)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 上传预览图
    /// </summary>
    [DisplayName("上传预览图")]
    public async Task<ProjectUploadOutput> Upload(IFormFile @object)
    {
        /*
         * 前端逻辑（useSync.hook.ts 的 dataSyncUpdate 方法）：
         * 如果 FileUrl 不为空，使用 FileUrl
         * 否则使用 GetOssInfo 接口获取到的 BucketUrl 和 FileName 进行拼接
         */

        //文件名格式示例 13414795568325_index_preview.png
        var fileNameSplit = @object.FileName.Split('_');
        var idStr = fileNameSplit[0];
        if (!long.TryParse(idStr, out var id)) return new ProjectUploadOutput();

        //将预览图转换成 Base64
        var ms = new MemoryStream();
        await @object.CopyToAsync(ms);
        var base64Image = Convert.ToBase64String(ms.ToArray());

        //保存
        if (await _goViewProjectDataRep.IsAnyAsync(u => u.Id == id))
        {
            await _goViewProjectDataRep.AsUpdateable()
                .SetColumns(u => new GoViewProjectData
                {
                    IndexImageData = base64Image
                })
                .Where(u => u.Id == id)
                .ExecuteCommandAsync();
        }
        else
        {
            await _goViewProjectDataRep.InsertAsync(new GoViewProjectData
            {
                Id = id,
                IndexImageData = base64Image,
            });
        }

        var output = new ProjectUploadOutput
        {
            Id = id,
            BucketName = null,
            CreateTime = null,
            CreateUserId = null,
            FileName = null,
            FileSize = 0,
            FileSuffix = "png",
            FileUrl = $"api/goview/project/getIndexImage/{id}",
            UpdateTime = null,
            UpdateUserId = null
        };

        #region 使用 SysFileService 方式（已注释）

        ////删除已存在的预览图
        //var uploadFileName = Path.GetFileNameWithoutExtension(@object.FileName);
        //var existFiles = await _fileRep.GetListAsync(u => u.FileName == uploadFileName);
        //foreach (var f in existFiles)
        //    await _fileService.DeleteFile(new DeleteFileInput { Id = f.Id });

        ////保存预览图
        //var result = await _fileService.UploadFile(@object, "");
        //var file = await _fileRep.GetFirstAsync(u => u.Id == result.Id);
        //int.TryParse(file.SizeKb, out var size);

        ////本地存储，使用拼接的地址
        //var fileUrl = file.BucketName == "Local" ? $"{file.FilePath}/{file.Id}{file.Suffix}" : file.Url;

        //var output = new ProjectUploadOutput
        //{
        //    Id = file.Id,
        //    BucketName = file.BucketName,
        //    CreateTime = file.CreateTime,
        //    CreateUserId = file.CreateUserId,
        //    FileName = $"{file.FileName}{file.Suffix}",
        //    FileSize = size,
        //    FileSuffix = file.Suffix?[1..],
        //    FileUrl = fileUrl,
        //    UpdateTime = null,
        //    UpdateUserId = null
        //};

        #endregion 使用 SysFileService 方式（已注释）

        return output;
    }

    /// <summary>
    /// 获取预览图
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetIndexImage")]
    [DisplayName("获取预览图")]
    public async Task<IActionResult> GetIndexImage(long id)
    {
        var projectData = await _goViewProjectDataRep.AsQueryable().IgnoreColumns(u => u.Content).FirstAsync(u => u.Id == id);
        if (projectData?.IndexImageData == null)
            return new NoContentResult();

        var bytes = Convert.FromBase64String(projectData.IndexImageData);
        return new FileStreamResult(new MemoryStream(bytes), "image/png");
    }
}