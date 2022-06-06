using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class GuestsInBookings
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
    }
}