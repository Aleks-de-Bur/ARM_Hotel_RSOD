using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models
{
    public class HotelDBEntities : DbContext 
    {
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Apartment> Apartments { get; set; }
        public virtual DbSet<AdditionalService> AdditionalServices { get; set; }
        public virtual DbSet<Living> Livings { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
    }
}