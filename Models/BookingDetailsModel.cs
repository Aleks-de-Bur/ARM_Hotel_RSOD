using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class BookingDetailsModel
    {
        public Booking Booking { get; set; }
        public List<Guest> Guests { get; set; }
    }
}