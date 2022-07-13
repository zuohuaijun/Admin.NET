namespace Admin.NET.Core;

/// <summary>
/// 系统字典值表种子数据
/// </summary>
public class SysDictDataSeedData : ISqlSugarEntitySeedData<SysDictData>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysDictData> HasData()
    {
        return new[]
        {
            new SysDictData{ Id=269037953100001, DictTypeId=269037954100001, Value="输入框", Code="Input", Order=100, Remark="输入框", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953100002, DictTypeId=269037954100001, Value="外键", Code="fk", Order=100, Remark="外键", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953100003, DictTypeId=269037954100001, Value="时间选择", Code="DatePicker", Order=100, Remark="时间选择", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953100004, DictTypeId=269037954100001, Value="选择器", Code="Select", Order=100, Remark="选择器", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953100005, DictTypeId=269037954100001, Value="数字输入框", Code="InputNumber", Order=100, Remark="数字输入框", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953100006, DictTypeId=269037954100001, Value="文本域", Code="InputTextArea", Order=100, Remark="文本域", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953100007, DictTypeId=269037954100001, Value="上传", Code="Upload", Order=100, Remark="上传", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953100008, DictTypeId=269037954100001, Value="树选择", Code="ApiTreeSelect", Order=100, Remark="树选择", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953100009, DictTypeId=269037954100001, Value="开关", Code="Switch", Order=100, Remark="开关", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953100010, DictTypeId=269037954100001, Value="常量选择器", Code="ConstSelector", Order=100, Remark="常量选择器", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },

            new SysDictData{ Id=269037953110001, DictTypeId=269037954100002, Value="等于", Code="==", Order=1, Remark="等于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953110002, DictTypeId=269037954100002, Value="模糊", Code="like", Order=1, Remark="模糊", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953110003, DictTypeId=269037954100002, Value="大于", Code=">", Order=1, Remark="大于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953110004, DictTypeId=269037954100002, Value="小于", Code="<", Order=1, Remark="小于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953110005, DictTypeId=269037954100002, Value="不等于", Code="!=", Order=1, Remark="不等于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953110006, DictTypeId=269037954100002, Value="大于等于", Code=">=", Order=1, Remark="大于等于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953110007, DictTypeId=269037954100002, Value="小于等于", Code="<=", Order=1, Remark="小于等于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953110008, DictTypeId=269037954100002, Value="不为空", Code="isNotNull", Order=1, Remark="不为空", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953110009, DictTypeId=269037954100002, Value="时间范围", Code="~", Order=1, Remark="时间范围", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },

            new SysDictData{ Id=269037953120001, DictTypeId=269037954100003, Value="long", Code="long", Order=1, Remark="long", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953120002, DictTypeId=269037954100003, Value="string", Code="string", Order=1, Remark="string", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953120003, DictTypeId=269037954100003, Value="DateTime", Code="DateTime", Order=1, Remark="DateTime", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953120004, DictTypeId=269037954100003, Value="bool", Code="bool", Order=1, Remark="bool", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953120005, DictTypeId=269037954100003, Value="int", Code="int", Order=1, Remark="int", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953120006, DictTypeId=269037954100003, Value="double", Code="double", Order=1, Remark="double", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953120007, DictTypeId=269037954100003, Value="float", Code="float", Order=1, Remark="float", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953120008, DictTypeId=269037954100003, Value="decimal", Code="decimal", Order=1, Remark="decimal", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953120009, DictTypeId=269037954100003, Value="Guid", Code="Guid", Order=1, Remark="Guid", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953120010, DictTypeId=269037954100003, Value="DateTimeOffset", Code="DateTimeOffset", Order=1, Remark="DateTimeOffset", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },

            new SysDictData{ Id=269037953130001, DictTypeId=269037954100004, Value="下载压缩包", Code="1", Order=1, Remark="下载压缩包", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953130002, DictTypeId=269037954100004, Value="生成到本项目", Code="2", Order=1, Remark="生成到本项目", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },

            new SysDictData{ Id=269037953140001, DictTypeId=269037954200001, Value="省级", Code="省级", Order=1, Remark="省级", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953140002, DictTypeId=269037954200001, Value="市级", Code="市级", Order=2, Remark="市级", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953140003, DictTypeId=269037954200001, Value="区级", Code="区级", Order=3, Remark="区级", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953140004, DictTypeId=269037954200001, Value="县级", Code="县级", Order=4, Remark="县级", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=269037953140005, DictTypeId=269037954200001, Value="其他", Code="其他", Order=5, Remark="其他", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
        };
    }
}