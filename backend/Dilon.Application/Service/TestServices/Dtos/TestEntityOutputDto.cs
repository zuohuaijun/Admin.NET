using Dilon.Core.Service;

namespace Dilon.Application
{
    public class TestEntityOutputDto : EntityOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
    }
}
