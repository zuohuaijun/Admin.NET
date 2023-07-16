// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统菜单返回结果
/// </summary>
public class MenuOutput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 父Id
    /// </summary>
    public long Pid { get; set; }

    /// <summary>
    /// 菜单类型（0目录 1菜单 2按钮）
    /// </summary>
    public MenuTypeEnum Type { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    public string Component { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    public string Permission { get; set; }

    /// <summary>
    /// 重定向
    /// </summary>
    public string Redirect { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int OrderNo { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public StatusEnum Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 菜单Meta
    /// </summary>
    public SysMenuMeta Meta { get; set; }

    /// <summary>
    /// 菜单子项
    /// </summary>
    public List<MenuOutput> Children { get; set; }
}

/// <summary>
/// 菜单Meta配置
/// </summary>
public class SysMenuMeta
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 是否内嵌
    /// </summary>
    public bool IsIframe { get; set; }

    /// <summary>
    /// 外链链接
    /// </summary>
    public string IsLink { get; set; }

    /// <summary>
    /// 是否隐藏
    /// </summary>
    public bool IsHide { get; set; }

    /// <summary>
    /// 是否缓存
    /// </summary>
    public bool IsKeepAlive { get; set; }

    /// <summary>
    /// 是否固定
    /// </summary>
    public bool IsAffix { get; set; }
}

/// <summary>
/// 配置菜单对象映射
/// </summary>
public class SysMenuMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<SysMenu, MenuOutput>()
            .Map(t => t.Meta.Title, o => o.Title)
            .Map(t => t.Meta.Icon, o => o.Icon)
            .Map(t => t.Meta.IsIframe, o => o.IsIframe)
            .Map(t => t.Meta.IsLink, o => o.OutLink)
            .Map(t => t.Meta.IsHide, o => o.IsHide)
            .Map(t => t.Meta.IsKeepAlive, o => o.IsKeepAlive)
            .Map(t => t.Meta.IsAffix, o => o.IsAffix);
    }
}