using SqlSugar;
using Admin.NET.Core;
using Admin.NET.Core.Service;
using System.Collections.Generic;
using System;
namespace Admin.NET.Application
{
    /// <summary>
    /// GenerateTest输出参数
    /// </summary>
    public class generatetestOutput
    {
       /// <summary>
       /// 主键Id
       /// </summary>
       public long Id { get; set; }
    
       /// <summary>
       /// 编码
       /// </summary>
       public string Code { get; set; }
    
       /// <summary>
       /// 名称
       /// </summary>
       public string Name { get; set; }
    
       /// <summary>
       /// 价格
       /// </summary>
       public decimal Price { get; set; }
    
       /// <summary>
       /// 过期日期
       /// </summary>
       public DateTime ExpireDate { get; set; }
    
       /// <summary>
       /// 状态
       /// </summary>
       public bool Status { get; set; }
    
    }


}
