namespace Admin.NET.Core.Service
{
    public class WechatPayTransactionInput
    {
        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; }
    }

    public class WechatPayParaInput
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public string PrepayId { get; set; }
    }
}