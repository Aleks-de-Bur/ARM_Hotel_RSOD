using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class Guest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите фамилию")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Фамилия")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Имя")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите отчество")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Введите серию паспорта")]
        [StringLength(4)]
        [Display(Name = "Серия паспорта")]
        public string SeriaPas { get; set; }

        [Required(ErrorMessage = "Введите номер паспорта")]
        [StringLength(6)]
        [Display(Name = "Номер паспорта")]
        public string NumberPas { get; set; }


        public int? LivingId { get; set; }
        public Living Living { get; set; }
        public int? BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}