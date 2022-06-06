using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models.Security
{
    public class IdentityClient : IdentityUser
    {
        public string LastName { get; set; }

    }
}