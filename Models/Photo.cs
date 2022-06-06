using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class Photo
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }  
        public int? ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }

    }
}