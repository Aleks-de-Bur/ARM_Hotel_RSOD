using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name ="Фамилия")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Имя")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите отчество")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Дата рождения")]
        [Required(ErrorMessage = "Введите дату рождения")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Введите серию паспорта")]
        [StringLength(4)]
        [Display(Name = "Серия паспорта")]
        public string SeriaPas { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "Введите номер паспорта")]
        [StringLength(6)]
        [Display(Name = "Номер паспорта")]
        public string NumberPas { get; set; }

        [Required(ErrorMessage = "Введите номер телефона")]
        [StringLength(12, MinimumLength = 11)]
        [Display(Name = "Номер телефона")]
        public string TelNumber { get; set; }


        public List<Living> Livings { get; set; }
        public List<Booking> Bookings { get; set; }
        public Client()
        {
            Livings = new List<Living>();
            Bookings = new List<Booking>();
        }
    }
}