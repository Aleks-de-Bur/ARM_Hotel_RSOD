using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models.Security
{
    public class ChangeProfile
    {
        [Required(ErrorMessage = "Введите фамилию")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "Фамилия должна содержать от 4 до 15 символов")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 15 символов")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

    }
}