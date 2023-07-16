// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Plugin.GoView.Service;

/// <summary>
/// 项目管理服务
/// </summary>
[UnifyProvider("GoView")]
[ApiDescriptionSettings(GoViewConst.GroupName, Module = "goview", Name = "project", Order = 100)]
public class GoViewProService : IDynamicApiController
{
    private readonly SqlSugarRepository<GoViewPro> _goViewProRep;
    private readonly SqlSugarRepository<GoViewProData> _goViewProDataRep;
    private readonly SqlSugarRepository<SysFile> _sysFileRep;
    private readonly SysFileService _fileService;

    public GoViewProService(SqlSugarRepository<GoViewPro> goViewProjectRep,
        SqlSugarRepository<GoViewProData> goViewProjectDataRep,
        SqlSugarRepository<SysFile> fileRep,
        SysFileService fileService)
    {
        _goViewProRep = goViewProjectRep;
        _goViewProDataRep = goViewProjectDataRep;
        _sysFileRep = fileRep;
        _fileService = fileService;
    }

    /// <summary>
    /// 获取项目列表
    /// </summary>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    [DisplayName("获取项目列表")]
    public async Task<List<GoViewProItemOutput>> GetList([FromQuery] int page = 1, [FromQuery] int limit = 12)
    {
        var res = await _goViewProRep.AsQueryable()
            .Select(u => new GoViewProItemOutput(), true)
            .ToPagedListAsync(page, limit);
        return res.Items.ToList();
    }

    /// <summary>
    /// 新增项目
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Create")]
    [DisplayName("新增项目")]
    public async Task<GoViewProCreateOutput> Create(GoViewProCreateInput input)
    {
        var project = await _goViewProRep.AsInsertable(input.Adapt<GoViewPro>()).ExecuteReturnEntityAsync();
        return new GoViewProCreateOutput
        {
            Id = project.Id
        };
    }

    /// <summary>
    /// 修改项目
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("修改项目")]
    public async Task Edit(GoViewProEditInput input)
    {
        await _goViewProRep.AsUpdateable(input.Adapt<GoViewPro>()).IgnoreColumns(true).ExecuteCommandAsync();
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
        await _goViewProRep.AsDeleteable().Where(u => idList.Contains(u.Id)).ExecuteCommandAsync();
        await _goViewProDataRep.AsDeleteable().Where(u => idList.Contains(u.Id)).ExecuteCommandAsync();
    }

    /// <summary>
    /// 修改发布状态
    /// </summary>
    [HttpPut]
    [DisplayName("修改发布状态")]
    public async Task Publish(GoViewProPublishInput input)
    {
        await _goViewProRep.AsUpdateable()
            .SetColumns(u => new GoViewPro
            {
                State = input.State
            })
            .Where(u => u.Id == input.Id)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取项目数据
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetData")]
    [DisplayName("获取项目数据")]
    public async Task<GoViewProDetailOutput> GetData([FromQuery] long projectId)
    {
        var projectData = await _goViewProDataRep.GetFirstAsync(u => u.Id == projectId);
        if (projectData == null) return null;

        var project = await _goViewProRep.GetFirstAsync(u => u.Id == projectId);
        var projectDetail = project.Adapt<GoViewProDetailOutput>();
        projectDetail.Content = projectData.Content;

        return projectDetail;
    }

    /// <summary>
    /// 保存项目数据
    /// </summary>
    [ApiDescriptionSettings(Name = "save/data")]
    [DisplayName("保存项目数据")]
    public async Task SaveData([FromForm] GoViewProSaveDataInput input)
    {
        if (await _goViewProDataRep.IsAnyAsync(u => u.Id == input.ProjectId))
        {
            await _goViewProDataRep.AsUpdateable()
                .SetColumns(u => new GoViewProData
                {
                    Content = input.Content
                })
                .Where(u => u.Id == input.ProjectId)
                .ExecuteCommandAsync();
        }
        else
        {
            await _goViewProDataRep.InsertAsync(new GoViewProData
            {
                Id = input.ProjectId,
                Content = input.Content,
            });
        }
    }

    /// <summary>
    /// 上传预览图
    /// </summary>
    [DisplayName("上传预览图")]
    public async Task<GoViewProUploadOutput> Upload(IFormFile @object)
    {
        /*
         * 前端逻辑（useSync.hook.ts 的 dataSyncUpdate 方法）：
         * 如果 FileUrl 不为空，使用 FileUrl
         * 否则使用 GetOssInfo 接口获取到的 BucketUrl 和 FileName 进行拼接
         */

        //文件名格式示例 13414795568325_index_preview.png
        var fileNameSplit = @object.FileName.Split('_');
        var idStr = fileNameSplit[0];
        if (!long.TryParse(idStr, out var id)) return new GoViewProUploadOutput();

        //将预览图转换成 Base64
        var ms = new MemoryStream();
        await @object.CopyToAsync(ms);
        var base64Image = Convert.ToBase64String(ms.ToArray());

        //保存
        if (await _goViewProDataRep.IsAnyAsync(u => u.Id == id))
        {
            await _goViewProDataRep.AsUpdateable()
                .SetColumns(u => new GoViewProData
                {
                    IndexImageData = base64Image
                })
                .Where(u => u.Id == id)
                .ExecuteCommandAsync();
        }
        else
        {
            await _goViewProDataRep.InsertAsync(new GoViewProData
            {
                Id = id,
                IndexImageData = base64Image,
            });
        }

        var output = new GoViewProUploadOutput
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
    [NonUnify]
    [ApiDescriptionSettings(Name = "GetIndexImage")]
    [DisplayName("获取预览图")]
    public async Task<IActionResult> GetIndexImage(long id)
    {
        var projectData = await _goViewProDataRep.AsQueryable().IgnoreColumns(u => u.Content).FirstAsync(u => u.Id == id);
        if (projectData?.IndexImageData == null)
            return new NoContentResult();

        var bytes = Convert.FromBase64String(projectData.IndexImageData);
        return new FileStreamResult(new MemoryStream(bytes), "image/png");
    }
}