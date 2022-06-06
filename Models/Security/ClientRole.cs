using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARM_Hotel.Models.Security
{
    public class ClientRole : IdentityRole
    {
        public ClientRole()
        {
        }
        public ClientRole(string roleName, string description) : base(roleName)
        {
            this.Description = description;
        }
        public string Description { get; set; }
    }
}