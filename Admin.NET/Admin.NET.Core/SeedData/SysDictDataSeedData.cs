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
    [IgnoreUpdate]
    public IEnumerable<SysDictData> HasData()
    {
        return new[]
        {
            new SysDictData{ Id=1300000000101, DictTypeId=1300000000101, Value="输入框", Code="Input", OrderNo=100, Remark="输入框", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000102, DictTypeId=1300000000101, Value="外键", Code="fk", OrderNo=100, Remark="外键", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000103, DictTypeId=1300000000101, Value="时间选择", Code="DatePicker", OrderNo=100, Remark="时间选择", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000104, DictTypeId=1300000000101, Value="选择器", Code="Select", OrderNo=100, Remark="选择器", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000105, DictTypeId=1300000000101, Value="数字输入框", Code="InputNumber", OrderNo=100, Remark="数字输入框", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000106, DictTypeId=1300000000101, Value="文本域", Code="InputTextArea", OrderNo=100, Remark="文本域", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000107, DictTypeId=1300000000101, Value="上传", Code="Upload", OrderNo=100, Remark="上传", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000108, DictTypeId=1300000000101, Value="树选择", Code="ApiTreeSelect", OrderNo=100, Remark="树选择", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000109, DictTypeId=1300000000101, Value="开关", Code="Switch", OrderNo=100, Remark="开关", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000110, DictTypeId=1300000000101, Value="常量选择器", Code="ConstSelector", OrderNo=100, Remark="常量选择器", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000111, DictTypeId=1300000000101, Value="枚举选择器", Code="EnumSelector", OrderNo=100, Remark="枚举选择器", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },

            new SysDictData{ Id=1300000000201, DictTypeId=1300000000102, Value="等于", Code="==", OrderNo=1, Remark="等于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000202, DictTypeId=1300000000102, Value="模糊", Code="like", OrderNo=1, Remark="模糊", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000203, DictTypeId=1300000000102, Value="大于", Code=">", OrderNo=1, Remark="大于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000204, DictTypeId=1300000000102, Value="小于", Code="<", OrderNo=1, Remark="小于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000205, DictTypeId=1300000000102, Value="不等于", Code="!=", OrderNo=1, Remark="不等于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000206, DictTypeId=1300000000102, Value="大于等于", Code=">=", OrderNo=1, Remark="大于等于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000207, DictTypeId=1300000000102, Value="小于等于", Code="<=", OrderNo=1, Remark="小于等于", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000208, DictTypeId=1300000000102, Value="不为空", Code="isNotNull", OrderNo=1, Remark="不为空", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000209, DictTypeId=1300000000102, Value="时间范围", Code="~", OrderNo=1, Remark="时间范围", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },

            new SysDictData{ Id=1300000000301, DictTypeId=1300000000103, Value="long", Code="long", OrderNo=1, Remark="long", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000302, DictTypeId=1300000000103, Value="string", Code="string", OrderNo=1, Remark="string", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000303, DictTypeId=1300000000103, Value="DateTime", Code="DateTime", OrderNo=1, Remark="DateTime", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000304, DictTypeId=1300000000103, Value="bool", Code="bool", OrderNo=1, Remark="bool", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000305, DictTypeId=1300000000103, Value="int", Code="int", OrderNo=1, Remark="int", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000306, DictTypeId=1300000000103, Value="double", Code="double", OrderNo=1, Remark="double", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000307, DictTypeId=1300000000103, Value="float", Code="float", OrderNo=1, Remark="float", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000308, DictTypeId=1300000000103, Value="decimal", Code="decimal", OrderNo=1, Remark="decimal", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000309, DictTypeId=1300000000103, Value="Guid", Code="Guid", OrderNo=1, Remark="Guid", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000310, DictTypeId=1300000000103, Value="DateTimeOffset", Code="DateTimeOffset", OrderNo=1, Remark="DateTimeOffset", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },

            new SysDictData{ Id=1300000000401, DictTypeId=1300000000104, Value="下载压缩包", Code="100", OrderNo=1, Remark="下载压缩包", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000402, DictTypeId=1300000000104, Value="生成到本项目", Code="2", OrderNo=4, Remark="生成到本项目", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000403, DictTypeId=1300000000104, Value="下载压缩包(前端)", Code="111", OrderNo=2, Remark="下载压缩包(前端)", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000404, DictTypeId=1300000000104, Value="下载压缩包(后端)", Code="121", OrderNo=3, Remark="下载压缩包(后端)", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000405, DictTypeId=1300000000104, Value="生成到本项目(前端)", Code="211", OrderNo=5, Remark="生成到本项目(前端)", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000406, DictTypeId=1300000000104, Value="生成到本项目(后端)", Code="221", OrderNo=6, Remark="生成到本项目(后端)", Status=StatusEnum.Enable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },

            new SysDictData{ Id=1300000000501, DictTypeId=1300000000105, Value="EntityBaseId【基础实体Id】", Code="EntityBaseId", OrderNo=1, Remark="【基础实体Id】", Status=StatusEnum.Disable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000502, DictTypeId=1300000000105, Value="EntityBase【基础实体】", Code="EntityBase", OrderNo=1, Remark="【基础实体】", Status=StatusEnum.Disable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000503, DictTypeId=1300000000105, Value="EntityTenantId【租户实体Id】", Code="EntityTenantId", OrderNo=1, Remark="【租户实体Id】", Status=StatusEnum.Disable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000504, DictTypeId=1300000000105, Value="EntityTenant【租户实体】", Code="EntityTenant", OrderNo=1, Remark="【租户实体】", Status=StatusEnum.Disable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysDictData{ Id=1300000000505, DictTypeId=1300000000105, Value="EntityBaseData【业务实体】", Code="EntityBaseData", OrderNo=1, Remark="【业务实体】", Status=StatusEnum.Disable, CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
        };
    }
}