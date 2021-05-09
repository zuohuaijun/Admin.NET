namespace Admin.NET.Core
{
    /// <summary>
    /// 数据操作类型
    /// </summary>
    public enum DataOpType
    {
        /// <summary>
        /// 其它
        /// </summary>
        OTHER,

        /// <summary>
        /// 增加
        /// </summary>
        ADD,

        /// <summary>
        /// 删除
        /// </summary>
        DELETE,

        /// <summary>
        /// 编辑
        /// </summary>
        EDIT,

        /// <summary>
        /// 更新
        /// </summary>
        UPDATE,

        /// <summary>
        /// 查询
        /// </summary>
        QUERY,

        /// <summary>
        /// 详情
        /// </summary>
        DETAIL,

        /// <summary>
        /// 树
        /// </summary>
        TREE,

        /// <summary>
        /// 导入
        /// </summary>
        IMPORT,

        /// <summary>
        /// 导出
        /// </summary>
        EXPORT,

        /// <summary>
        /// 授权
        /// </summary>
        GRANT,

        /// <summary>
        /// 强退
        /// </summary>
        FORCE,

        /// <summary>
        /// 清空
        /// </summary>
        CLEAN,

        /// <summary>
        /// 修改状态
        /// </summary>
        CHANGE_STATUS
    }
}