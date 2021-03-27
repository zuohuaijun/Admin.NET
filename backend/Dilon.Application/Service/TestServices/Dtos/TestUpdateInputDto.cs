using Dilon.Core;
using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dilon.Application
{
    public class TestUpdateInputDto : IUpdateInputDto, IValidatableObject
    {
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(10, ErrorMessage = "超过最大长度{1}")]
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Range(18, 150, ErrorMessage = "{0}应在{1}到{2}之间")]
        public int Age { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Db.GetRepository<Test>().Any(u => u.Name == Name && u.Id != Id))
            {
                yield return new ValidationResult(
                    $"{Name} 已经存在"
                    , new[] { nameof(Name) }
                );
            }
        }
    }
}
