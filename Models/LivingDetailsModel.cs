using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class LivingDetailsModel
    {
        public Living Living { get; set; }
        public List<AdditionalService> Services { get; set; }
        public List<Guest> Guests { get; set; }
    }
}