namespace Admin.NET.Application.SeedData;

/// <summary>
/// 实体种子数据
/// </summary>
public class TestSeedData : ISqlSugarEntitySeedData<Test>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Test> HasData()
    {
        yield return new Test { Id = 1300000000101, Name = "张三", Age = 20, CreateTime = DateTime.Parse("2022-04-12 00:00:00") };
        yield return new Test { Id = 1300000000102, Name = "李四", Age = 30, CreateTime = DateTime.Parse("2022-04-12 00:00:00") };
    }
}