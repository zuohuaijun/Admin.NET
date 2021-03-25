using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilon.Application
{
    public interface IBaseId
    {
        [Required]
        public long Id { get; set; }
    }
}
