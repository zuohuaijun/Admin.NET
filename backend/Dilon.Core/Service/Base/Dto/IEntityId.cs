using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    public interface IEntityId
    {
        [Required]
        public long Id { get; set; }
    }
}
