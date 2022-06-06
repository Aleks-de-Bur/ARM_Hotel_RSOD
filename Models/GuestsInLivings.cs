using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class GuestsInLivings
    {
        public int Id { get; set; }
        public int LivingId { get; set; }
        public Living Living { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
    }
}