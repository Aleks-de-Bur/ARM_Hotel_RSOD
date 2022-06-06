using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class AdditionalService
    {
        public int Id { get; set; }
     
        public int? ServiceId { get; set; }
        public Service Service { get; set; }

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Введите цену")]
        [Range(100, 50000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }


        public int? LivingId { get; set; }
        public Living Living { get; set; }


    }
}