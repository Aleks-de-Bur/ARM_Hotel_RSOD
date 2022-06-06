using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models.Security
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Введите старый пароль")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 20 символов")]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Введите новый пароль")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 20 символов")]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmNewPassword { get; set; }
    }
}