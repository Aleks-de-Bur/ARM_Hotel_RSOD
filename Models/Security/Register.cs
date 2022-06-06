using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models.Security
{
    public class Register
    {
        [Required(ErrorMessage = "Введите логин")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Логин должен содержать от 4 до 20 символов")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "Фамилия должна содержать от 4 до 15 символов")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 15 символов")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 20 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Повторите пароль")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress]
        public string Email { get; set; }

    }
}