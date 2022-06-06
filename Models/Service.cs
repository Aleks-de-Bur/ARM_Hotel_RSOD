using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Display(Name = "Наименование услуги")]
        [Required(ErrorMessage = "Введите название услуги")]
        [StringLength(50, MinimumLength = 3)]
        public string ServiceName { get; set; }

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Введите цену")]
        [Range(100, 50000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public List<AdditionalService> AdditionalServices { get; set; }
        public Service()
        {
            AdditionalServices = new List<AdditionalService>();
        }
    }
}