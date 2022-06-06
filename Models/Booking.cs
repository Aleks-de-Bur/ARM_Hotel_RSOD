using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Display(Name = "Дата заселения")]
        [Required(ErrorMessage = "Введите дату заселения")]
        [DataType(DataType.Date)]
        public DateTime Settling { get; set; }

        [Display(Name = "Дата выселения")]
        [Required(ErrorMessage = "Введите дату выселения")]
        [DataType(DataType.Date)]
        public DateTime Eviction { get; set; }

        [Display(Name = "Номер")]
        [Required(ErrorMessage = "Введите номер")]
        [StringLength(4, MinimumLength = 1)]
        public string Number { get; set; }

        [Display(Name = "Количество гостей")]
        [Required(ErrorMessage = "Введите количество гостей")]
        [Range(0, 15)]
        public int ValueOfGuests { get; set; }

        [Display(Name = "Клиент")]
        public int? ClientId { get; set; }
        public Client Client { get; set; }

        public List<Guest> Guests { get; set; }

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Введите цену")]
        [Range(100, 1000000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Тип номера")]
        [Required(ErrorMessage = "Введите тип номера")]
        [StringLength(50, MinimumLength = 3)]
        public string Type { get; set; }


        [Display(Name = "ID номера")]
        public int? ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public Booking()
        {
            Guests = new List<Guest>();
        }
    }
}