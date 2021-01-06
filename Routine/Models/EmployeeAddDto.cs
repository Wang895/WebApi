using Routine.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Models
{
    public class EmployeeAddDto:IValidatableObject
    {
        [Display(Name = "员工号")]
        [Required]
        [StringLength(10,MinimumLength = 10,ErrorMessage = "{0}的长度为{1}")]
        public string EmployeeNo { get; set; }
        [Display(Name = "名")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string FirstName { get; set; }
        [Display(Name = "姓")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string LastName { get; set; }
        [Display(Name = "性别")]
        public Gender Gender { get; set; }
        [Display(Name = "出生日期")]
        public DateTime DateOfBirth { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == LastName)
            {
                yield return new ValidationResult("姓和名不能一样",new [] {nameof(EmployeeAddDto)});
            }
        }
    }
}
