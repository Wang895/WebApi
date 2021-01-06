using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Models
{
    public class CompanyAddDto
    {
        [Display(Name="公司名称")]
        [Required(ErrorMessage ="{0}是必填的")]
        [MaxLength(100,ErrorMessage = "{0}最大长度不超过{1}")]
        public string Name { set; get; }
        [Display(Name="公司简介")]
        [StringLength(500,MinimumLength =10,ErrorMessage ="{0}的长度应在{2}到{1}之间")]
        public string Introduction { set; get; }
        public IEnumerable<EmployeeAddDto> Employees { set; get; } = new List<EmployeeAddDto>();

    }
}
