using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;


[assembly: OwinStartup(typeof(ARM_Hotel.App_Start.Startup))]

namespace ARM_Hotel.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            CookieAuthenticationOptions options = new CookieAuthenticationOptions();
            options.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie;
            options.LoginPath = new PathString("/account/login");
            app.UseCookieAuthentication(options);
        }
    }
}
