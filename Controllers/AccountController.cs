using ARM_Hotel.Models;
using ARM_Hotel.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ARM_Hotel.Controllers
{
    public class AccountController : Controller
    {
        private HotelDBEntities dbase = new HotelDBEntities();
        private UserManager<IdentityClient> userManager;
        private RoleManager<ClientRole> roleManager;
        public AccountController()
        {
            MyIdentityDbContext db = new MyIdentityDbContext();
            UserStore<IdentityClient> userStore = new UserStore<IdentityClient>(db);
            userManager = new UserManager<IdentityClient>(userStore);
            RoleStore<ClientRole> roleStore = new RoleStore<ClientRole>(db);
            roleManager = new RoleManager<ClientRole>(roleStore);
        }

        public ActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Client, Operator")]
        public ActionResult LocalCabinet()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                IdentityClient user = new IdentityClient();
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.LastName = model.LastName;
                IdentityResult result = userManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Client");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("UserName", "Ошибка при регистрации пользователя!");
                }
            }
            return View(model);
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityClient user = userManager.Find(model.UserName, model.Password);
                if (user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;

                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    authenticationManager.SignIn(props, identity);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                IdentityClient user = userManager.FindByName(HttpContext.User.Identity.Name);
                IdentityResult result = userManager.ChangePassword(user.Id, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", " Ошибка при смене пароля");
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult ChangeProfile()
        {
            IdentityClient user = userManager.FindByName(HttpContext.User.Identity.Name);
            ChangeProfile model = new ChangeProfile();
            model.LastName = user.LastName;
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfile(ChangeProfile model)
        {
            if (ModelState.IsValid)
            {
                IdentityClient user = userManager.FindByName(HttpContext.User.Identity.Name);
                user.LastName = model.LastName;
                IdentityResult result = userManager.Update(user);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Profile updated successfully.";
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка при сохранении профиля.");
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

    }

}