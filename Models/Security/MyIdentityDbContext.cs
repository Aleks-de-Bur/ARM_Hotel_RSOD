using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models.Security
{
    public class MyIdentityDbContext : IdentityDbContext<IdentityClient>
    {
        public MyIdentityDbContext() : base("IdentityDB")
        {
        }
    }
}