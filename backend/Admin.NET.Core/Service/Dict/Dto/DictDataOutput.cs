namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 字典值参数
    /// </summary>
    public class DictDataOutput
    {
        /// <summary>
        /// 字典Id
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 字典类型Id
        /// </summary>
        public long TypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public CommonStatus Status { get; set; }
    }
}