// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using AngleSharp;
using AngleSharp.Html.Dom;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统行政区域服务
/// </summary>
[ApiDescriptionSettings(Order = 310)]
public class SysRegionService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysRegion> _sysRegionRep;

    // Url地址-国家统计局行政区域2023年
    private readonly string _url = "http://www.stats.gov.cn/sj/tjbz/tjyqhdmhcxhfdm/2023/index.html";

    public SysRegionService(SqlSugarRepository<SysRegion> sysRegionRep)
    {
        _sysRegionRep = sysRegionRep;
    }

    /// <summary>
    /// 获取行政区域分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取行政区域分页列表")]
    public async Task<SqlSugarPagedList<SysRegion>> Page(PageRegionInput input)
    {
        return await _sysRegionRep.AsQueryable()
            .WhereIF(input.Pid > 0, u => u.Pid == input.Pid || u.Id == input.Pid)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code.Contains(input.Code))
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取行政区域列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取行政区域列表")]
    public async Task<List<SysRegion>> GetList([FromQuery] RegionInput input)
    {
        return await _sysRegionRep.GetListAsync(u => u.Pid == input.Id);
    }

    /// <summary>
    /// 增加行政区域
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("增加行政区域")]
    public async Task<long> AddRegion(AddRegionInput input)
    {
        var isExist = await _sysRegionRep.IsAnyAsync(u => u.Name == input.Name && u.Code == input.Code);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.R2002);

        var sysRegion = input.Adapt<SysRegion>();
        var newRegion = await _sysRegionRep.AsInsertable(sysRegion).ExecuteReturnEntityAsync();
        return newRegion.Id;
    }

    /// <summary>
    /// 更新行政区域
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新行政区域")]
    public async Task UpdateRegion(UpdateRegionInput input)
    {
        if (input.Pid != 0)
        {
            var pRegion = await _sysRegionRep.GetFirstAsync(u => u.Id == input.Pid);
            _ = pRegion ?? throw Oops.Oh(ErrorCodeEnum.D2000);
        }
        if (input.Id == input.Pid)
            throw Oops.Oh(ErrorCodeEnum.R2001);

        var sysRegion = await _sysRegionRep.GetFirstAsync(u => u.Id == input.Id);
        var isExist = await _sysRegionRep.IsAnyAsync(u => (u.Name == input.Name && u.Code == input.Code) && u.Id != sysRegion.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.R2002);

        //// 父Id不能为自己的子节点
        //var regionTreeList = await _sysRegionRep.AsQueryable().ToChildListAsync(u => u.Pid, input.Id, true);
        //var childIdList = regionTreeList.Select(u => u.Id).ToList();
        //if (childIdList.Contains(input.Pid))
        //    throw Oops.Oh(ErrorCodeEnum.R2001);

        await _sysRegionRep.AsUpdateable(input.Adapt<SysRegion>()).IgnoreColumns(true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除行政区域
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除行政区域")]
    public async Task DeleteRegion(DeleteRegionInput input)
    {
        var regionTreeList = await _sysRegionRep.AsQueryable().ToChildListAsync(u => u.Pid, input.Id, true);
        var regionIdList = regionTreeList.Select(u => u.Id).ToList();
        await _sysRegionRep.DeleteAsync(u => regionIdList.Contains(u.Id));
    }

    /// <summary>
    /// 同步行政区域
    /// </summary>
    /// <returns></returns>
    [DisplayName("同步行政区域")]
    public async Task Sync()
    {
        await _sysRegionRep.DeleteAsync(u => u.Id > 0);

        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        var dom = await context.OpenAsync(_url);

        // 省级
        var itemList = dom.QuerySelectorAll("table.provincetable tr.provincetr td a");
        foreach (IHtmlAnchorElement item in itemList)
        {
            var region = await _sysRegionRep.InsertReturnEntityAsync(new SysRegion
            {
                Pid = 0,
                Name = item.TextContent,
                Remark = item.Href,
                Level = 1,
            });

            // 市级
            if (string.IsNullOrEmpty(item.Href))
                continue;
            var dom1 = await context.OpenAsync(item.Href);
            var itemList1 = dom1.QuerySelectorAll("table.citytable tr.citytr td a");
            for (var i1 = 0; i1 < itemList1.Length; i1 += 2)
            {
                var item1 = (IHtmlAnchorElement)itemList1[i1 + 1];
                var region1 = await _sysRegionRep.InsertReturnEntityAsync(new SysRegion
                {
                    Pid = region.Id,
                    Name = item1.TextContent,
                    Code = itemList1[i1].TextContent,
                    Remark = item1.Href,
                    Level = 2,
                });

                // 区县级
                if (string.IsNullOrEmpty(item1.Href))
                    continue;
                var dom2 = await context.OpenAsync(item1.Href);
                var itemList2 = dom2.QuerySelectorAll("table.countytable tr.countytr td a");
                for (var i2 = 0; i2 < itemList2.Length; i2 += 2)
                {
                    var item2 = (IHtmlAnchorElement)itemList2[i2 + 1];
                    var region2 = await _sysRegionRep.InsertReturnEntityAsync(new SysRegion
                    {
                        Pid = region1.Id,
                        Name = item2.TextContent,
                        Code = itemList2[i2].TextContent,
                        Remark = item2.Href,
                        Level = 3,
                    });

                    // 街道级
                    if (string.IsNullOrEmpty(item2.Href))
                        continue;
                    var dom3 = await context.OpenAsync(item2.Href);
                    var itemList3 = dom3.QuerySelectorAll("table.towntable tr.towntr td a");
                    for (var i3 = 0; i3 < itemList3.Length; i3 += 2)
                    {
                        var item3 = (IHtmlAnchorElement)itemList3[i3 + 1];
                        var region3 = await _sysRegionRep.InsertReturnEntityAsync(new SysRegion
                        {
                            Pid = region2.Id,
                            Name = item3.TextContent,
                            Code = itemList3[i3].TextContent,
                            Remark = item3.Href,
                            Level = 4,
                        });

                        // 村级
                        if (string.IsNullOrEmpty(item3.Href))
                            continue;
                        var dom4 = await context.OpenAsync(item3.Href);
                        var itemList4 = dom4.QuerySelectorAll("table.villagetable tr.villagetr td");
                        for (var i4 = 0; i4 < itemList4.Length; i4 += 3)
                        {
                            await _sysRegionRep.InsertAsync(new SysRegion
                            {
                                Pid = region3.Id,
                                Name = itemList4[i4 + 2].TextContent,
                                Code = itemList4[i4].TextContent,
                                CityCode = itemList4[i4 + 1].TextContent,
                                Level = 5,
                            });
                        }
                    }
                }
            }
        }
    }
}