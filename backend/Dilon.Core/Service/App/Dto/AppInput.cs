using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 系统应用参数
    /// </summary>
    public class AppInput : PageInputBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 是否默认激活（Y-是，N-否）,只能有一个系统默认激活
        /// 用户登录后默认展示此系统菜单
        /// </summary>
        public string Active { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }

    public class AddAppInput : AppInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "应用名称不能为空")]
        public override string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "应用编码不能为空")]
        public override string Code { get; set; }
    }

    public class DeleteAppInput
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        [Required(ErrorMessage = "应用Id不能为空")]
        public long Id { get; set; }
    }

    public class UpdateAppInput : AppInput
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        [Required(ErrorMessage = "应用Id不能为空")]
        public long Id { get; set; }
    }

    public class QueryAppInput : DeleteAppInput
    {

    }

    public class SetDefaultAppInput : DeleteAppInput
    {

    }
}
