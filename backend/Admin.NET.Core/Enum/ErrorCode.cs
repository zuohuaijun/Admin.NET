using Furion.FriendlyException;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统错误码
    /// </summary>
    [ErrorCodeType]
    public enum ErrorCode
    {
        /// <summary>
        /// 用户名或密码不正确
        /// </summary>
        [ErrorCodeItemMetadata("用户名或密码不正确")]
        D1000,

        /// <summary>
        /// 非法操作！禁止删除自己
        /// </summary>
        [ErrorCodeItemMetadata("非法操作，禁止删除自己")]
        D1001,

        /// <summary>
        /// 记录不存在
        /// </summary>
        [ErrorCodeItemMetadata("记录不存在")]
        D1002,

        /// <summary>
        /// 账号已存在
        /// </summary>
        [ErrorCodeItemMetadata("账号已存在")]
        D1003,

        /// <summary>
        /// 旧密码不匹配
        /// </summary>
        [ErrorCodeItemMetadata("旧密码输入错误")]
        D1004,

        /// <summary>
        /// 测试数据禁止更改admin密码
        /// </summary>
        [ErrorCodeItemMetadata("测试数据禁止更改用户【admin】密码")]
        D1005,

        /// <summary>
        /// 数据已存在
        /// </summary>
        [ErrorCodeItemMetadata("数据已存在")]
        D1006,

        /// <summary>
        /// 数据不存在或含有关联引用，禁止删除
        /// </summary>
        [ErrorCodeItemMetadata("数据不存在或含有关联引用，禁止删除")]
        D1007,

        /// <summary>
        /// 禁止为管理员分配角色
        /// </summary>
        [ErrorCodeItemMetadata("禁止为管理员分配角色")]
        D1008,

        /// <summary>
        /// 重复数据或记录含有不存在数据
        /// </summary>
        [ErrorCodeItemMetadata("重复数据或记录含有不存在数据")]
        D1009,

        /// <summary>
        /// 禁止为超级管理员角色分配权限
        /// </summary>
        [ErrorCodeItemMetadata("禁止为超级管理员角色分配权限")]
        D1010,

        /// <summary>
        /// 非法数据
        /// </summary>
        [ErrorCodeItemMetadata("非法数据")]
        D1011,

        /// <summary>
        /// Id不能为空
        /// </summary>
        [ErrorCodeItemMetadata("Id不能为空")]
        D1012,

        /// <summary>
        /// 所属机构不在自己的数据范围内
        /// </summary>
        [ErrorCodeItemMetadata("没有权限操作该数据")]
        D1013,

        /// <summary>
        /// 禁止删除超级管理员
        /// </summary>
        [ErrorCodeItemMetadata("禁止删除超级管理员")]
        D1014,

        /// <summary>
        /// 禁止修改超级管理员状态
        /// </summary>
        [ErrorCodeItemMetadata("禁止修改超级管理员状态")]
        D1015,

        /// <summary>
        /// 没有权限
        /// </summary>
        [ErrorCodeItemMetadata("没有权限")]
        D1016,

        /// <summary>
        /// 账号已冻结
        /// </summary>
        [ErrorCodeItemMetadata("账号已冻结")]
        D1017,

        /// <summary>
        /// 父机构不存在
        /// </summary>
        [ErrorCodeItemMetadata("父机构不存在")]
        D2000,

        /// <summary>
        /// 当前机构Id不能与父机构Id相同
        /// </summary>
        [ErrorCodeItemMetadata("当前机构Id不能与父机构Id相同")]
        D2001,

        /// <summary>
        /// 已有相同组织机构,编码或名称相同
        /// </summary>
        [ErrorCodeItemMetadata("已有相同组织机构,编码或名称相同")]
        D2002,

        /// <summary>
        /// 没有权限操作机构
        /// </summary>
        [ErrorCodeItemMetadata("没有权限操作机构")]
        D2003,

        /// <summary>
        /// 该机构下有员工禁止删除
        /// </summary>
        [ErrorCodeItemMetadata("该机构下有员工禁止删除")]
        D2004,

        /// <summary>
        /// 附属机构下有员工禁止删除
        /// </summary>
        [ErrorCodeItemMetadata("附属机构下有员工禁止删除")]
        D2005,

        /// <summary>
        /// 只能增加下级机构
        /// </summary>
        [ErrorCodeItemMetadata("只能增加下级机构")]
        D2006,

        /// <summary>
        /// 字典类型不存在
        /// </summary>
        [ErrorCodeItemMetadata("字典类型不存在")]
        D3000,

        /// <summary>
        /// 字典类型已存在
        /// </summary>
        [ErrorCodeItemMetadata("字典类型已存在,名称或编码重复")]
        D3001,

        /// <summary>
        /// 字典类型下面有字典值禁止删除
        /// </summary>
        [ErrorCodeItemMetadata("字典类型下面有字典值禁止删除")]
        D3002,

        /// <summary>
        /// 字典值已存在
        /// </summary>
        [ErrorCodeItemMetadata("字典值已存在,名称或编码重复")]
        D3003,

        /// <summary>
        /// 字典值不存在
        /// </summary>
        [ErrorCodeItemMetadata("字典值不存在")]
        D3004,

        /// <summary>
        /// 字典状态错误
        /// </summary>
        [ErrorCodeItemMetadata("字典状态错误")]
        D3005,

        /// <summary>
        /// 菜单已存在
        /// </summary>
        [ErrorCodeItemMetadata("菜单已存在")]
        D4000,

        /// <summary>
        /// 路由地址为空
        /// </summary>
        [ErrorCodeItemMetadata("路由地址为空")]
        D4001,

        /// <summary>
        /// 打开方式为空
        /// </summary>
        [ErrorCodeItemMetadata("打开方式为空")]
        D4002,

        /// <summary>
        /// 权限标识格式为空
        /// </summary>
        [ErrorCodeItemMetadata("权限标识格式为空")]
        D4003,

        /// <summary>
        /// 权限标识格式错误
        /// </summary>
        [ErrorCodeItemMetadata("权限标识格式错误")]
        D4004,

        /// <summary>
        /// 权限不存在
        /// </summary>
        [ErrorCodeItemMetadata("权限不存在")]
        D4005,

        /// <summary>
        /// 父级菜单不能为当前节点，请重新选择父级菜单
        /// </summary>
        [ErrorCodeItemMetadata("父级菜单不能为当前节点，请重新选择父级菜单")]
        D4006,

        /// <summary>
        /// 不能移动根节点
        /// </summary>
        [ErrorCodeItemMetadata("不能移动根节点")]
        D4007,

        /// <summary>
        /// 已存在同名或同编码应用
        /// </summary>
        [ErrorCodeItemMetadata("已存在同名或同编码应用")]
        D5000,

        /// <summary>
        /// 默认激活系统只能有一个
        /// </summary>
        [ErrorCodeItemMetadata("默认激活系统只能有一个")]
        D5001,

        /// <summary>
        /// 该应用下有菜单禁止删除
        /// </summary>
        [ErrorCodeItemMetadata("该应用下有菜单禁止删除")]
        D5002,

        /// <summary>
        /// 已存在同名或同编码应用
        /// </summary>
        [ErrorCodeItemMetadata("已存在同名或同编码应用")]
        D5003,

        /// <summary>
        /// 已存在同名或同编码职位
        /// </summary>
        [ErrorCodeItemMetadata("已存在同名或同编码职位")]
        D6000,

        /// <summary>
        /// 该职位下有员工禁止删除
        /// </summary>
        [ErrorCodeItemMetadata("该职位下有员工禁止删除")]
        D6001,

        /// <summary>
        /// 通知公告状态错误
        /// </summary>
        [ErrorCodeItemMetadata("通知公告状态错误")]
        D7000,

        /// <summary>
        /// 通知公告删除失败
        /// </summary>
        [ErrorCodeItemMetadata("通知公告删除失败")]
        D7001,

        /// <summary>
        /// 通知公告编辑失败
        /// </summary>
        [ErrorCodeItemMetadata("通知公告编辑失败，类型必须为草稿")]
        D7002,

        /// <summary>
        /// 文件不存在
        /// </summary>
        [ErrorCodeItemMetadata("文件不存在")]
        D8000,

        /// <summary>
        /// 已存在同名或同编码参数配置
        /// </summary>
        [ErrorCodeItemMetadata("已存在同名或同编码参数配置")]
        D9000,

        /// <summary>
        /// 禁止删除系统参数
        /// </summary>
        [ErrorCodeItemMetadata("禁止删除系统参数")]
        D9001,

        /// <summary>
        /// 已存在同名任务调度
        /// </summary>
        [ErrorCodeItemMetadata("已存在同名任务调度")]
        D1100,

        /// <summary>
        /// 任务调度不存在
        /// </summary>
        [ErrorCodeItemMetadata("任务调度不存在")]
        D1101,

        /// <summary>
        /// 演示环境禁止修改数据
        /// </summary>
        [ErrorCodeItemMetadata("演示环境禁止修改数据")]
        D1200,

        /// <summary>
        /// 已存在同名或同管理员或同主机租户
        /// </summary>
        [ErrorCodeItemMetadata("已存在同名或同主机租户")]
        D1300,

        /// <summary>
        /// 该表代码模板已经生成过
        /// </summary>
        [ErrorCodeItemMetadata("该表代码模板已经生成过")]
        D1400,

        /// <summary>
        /// 该类型不存在
        /// </summary>
        [ErrorCodeItemMetadata("该类型不存在")]
        D1501,

        /// <summary>
        /// 该字段不存在
        /// </summary>
        [ErrorCodeItemMetadata("该字段不存在")]
        D1502,

        /// <summary>
        /// 该类型不是枚举类型
        /// </summary>
        [ErrorCodeItemMetadata("该类型不是枚举类型")]
        D1503,

        /// <summary>
        /// 该实体不存在
        /// </summary>
        [ErrorCodeItemMetadata("该实体不存在")]
        D1504,

        /// <summary>
        /// 父菜单不存在
        /// </summary>
        [ErrorCodeItemMetadata("父菜单不存在")]
        D1505,

        /// <summary>
        /// 已存在同名或同编码项目
        /// </summary>
        [ErrorCodeItemMetadata("已存在同名或同编码项目")]
        xg1000,

        /// <summary>
        /// 已存在相同证件号码人员
        /// </summary>
        [ErrorCodeItemMetadata("已存在相同证件号码人员")]
        xg1001,

        /// <summary>
        /// 检测数据不存在
        /// </summary>
        [ErrorCodeItemMetadata("检测数据不存在")]
        xg1002,
    }
}