using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [MaxLength(10,ErrorMessage = "{0}长度不可超过10")]
        [Display(Name = "用户名")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [MinLength(8,ErrorMessage = "密码不能少于8位")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        public string Password { get; set; }      
    }
}
