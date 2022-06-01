namespace Admin.NET.Core.Service;

public class FileOutput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
        get
        {
            return this.Id + this.Suffix;
        }
    }

    /// <summary>
    /// URL
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 大小
    /// </summary>
    public string SizeKb { get; set; }

    /// <summary>
    /// 后缀
    /// </summary>
    public string Suffix { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    public string FilePath { get; set; }
}

[SugarTable("sys_file")]
[NotTable]
public class MapperSysFileOutput
{
    /// <summary>
    /// 雪花Id
    /// </summary>
    [SugarColumn(ColumnDescription = "Id", IsPrimaryKey = true, IsIdentity = false)]
    public long Id { get; set; }

    public string BucketName { get; set; }

    public string FileName { get; set; }

    public string Suffix { get; set; }

    public string FilePath { get; set; }
}