using Admin.NET.Application.Const;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Pdf;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace Admin.NET.Application.Service;

/// <summary>
/// 自己业务服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 200)]
[AllowAnonymous]
public class Test2Service : IDynamicApiController, ITransient
{
    public Test2Service()
    {
    }

    /// <summary>
    /// 生成PDF文件
    /// </summary>
    /// <returns></returns>
    public async Task<dynamic> CreatePDFReport()
    {
        var tpl = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "temp/CovidReport.html"));
        var exporter = new PdfExporter();
        var pdfAtt = new PdfExporterAttribute();
        pdfAtt.Orientation = WkHtmlToPdfDotNet.Orientation.Portrait;
        pdfAtt.PaperKind = WkHtmlToPdfDotNet.PaperKind.A4;
        var result = await exporter.ExportBytesByTemplate(new ReportData
        {
            Name = "张三",
            IdNo = "130430xxxxxxxxxxxx",
            Sex = "男",
            Age = 35,
            TudeNo = "12345678901",
            CollectTime = "2022-08-01 12:33:33",
            ReceiveTime = "2022-08-01 18:33:33",
            CheckTime = "2022-08-02 18:33:33"
        }, pdfAtt, tpl);

        return new FileStreamResult(new MemoryStream(result), "application/octet-stream") { FileDownloadName = "核酸.PDF" };
    }
}

[Exporter(Name = "核酸报告")]
public class ReportData
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 身份证
    /// </summary>
    public string IdNo { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public string Sex { get; set; }

    /// <summary>
    /// 年龄
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// 试管编号
    /// </summary>
    public string TudeNo { get; set; }

    /// <summary>
    /// 采样时间
    /// </summary>
    public string CollectTime { get; set; }

    /// <summary>
    /// 收样时间
    /// </summary>
    public string ReceiveTime { get; set; }

    /// <summary>
    /// 检测时间
    /// </summary>
    public string CheckTime { get; set; }
}