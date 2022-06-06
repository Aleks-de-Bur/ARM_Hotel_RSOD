using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class Apartment
    {
        public int Id { get; set; }

        [Display(Name = "Номер")]
        [Required(ErrorMessage = "Введите номер")]
        [StringLength(4, MinimumLength = 1)]
        public string Number { get; set; }

        [Display(Name = "Тип номера")]
        [Required(ErrorMessage = "Введите тип номера")]
        [StringLength(50, MinimumLength = 3)]
        public string Type { get; set; }

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Введите цену")]
        [Range(100, 1000000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Максимальное количество гостей")]
        [Required(ErrorMessage = "Введите максимальное количество гостей")]
        [Range(0, 15)]
        public int MaxGuests { get; set; }

        public List<Photo> Photos { get; set; }
        public List<Living> Living { get; set; }
        public List<Booking> Booking { get; set; }

        public Apartment()
        {
            Photos = new List<Photo>();
            Living = new List<Living>();
            Booking = new List<Booking>();
        }
    }
}