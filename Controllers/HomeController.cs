using ARM_Hotel.Models;
using ARM_Hotel.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ARM_Hotel.Controllers
{
    public class HomeController : Controller
    {
        private HotelDBEntities dbase = new HotelDBEntities();
        private UserManager<IdentityClient> userManager;
        private RoleManager<ClientRole> roleManager;
        public HomeController()
        {
            MyIdentityDbContext db = new MyIdentityDbContext();
            UserStore<IdentityClient> userStore = new UserStore<IdentityClient>(db);
            userManager = new UserManager<IdentityClient>(userStore);
            RoleStore<ClientRole> roleStore = new RoleStore<ClientRole>(db);
            roleManager = new RoleManager<ClientRole>(roleStore);
        }

        public ActionResult Index(int? clientId)
        {
            if (User.IsInRole("Client"))
            {
                var curuser = userManager.FindById(User.Identity.GetUserId());
                Client client = dbase.Clients.FirstOrDefault(l => l.Email == curuser.Email);
                if (client == null)
                {
                    var user = userManager.FindByName(User.Identity.Name);
                    Client newclient = new Client();
                    newclient.Email = user.Email;
                    newclient.FirstName = user.LastName;

                    return View("~/Views/Clients/Create.cshtml", newclient);
                }
                ViewBag.ClientId = client.Id;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}