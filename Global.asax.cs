using ARM_Hotel.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ARM_Hotel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MyIdentityDbContext db = new MyIdentityDbContext();
            RoleStore<ClientRole> roleStore = new RoleStore<ClientRole>(db);
            RoleManager<ClientRole> roleManager = new RoleManager<ClientRole>(roleStore);
            if (!roleManager.RoleExists("Admin"))
            {
                ClientRole newRole = new ClientRole("Admin", "������������� �������� ������� ������� � �������");
                roleManager.Create(newRole);
            }
            if (!roleManager.RoleExists("Operator"))
            {
                ClientRole newRole = new ClientRole("Operator", "��������� ����� ������������� ������ �������������, ��������� ���������� � ������������ � �������");
                roleManager.Create(newRole);
            }
            if (!roleManager.RoleExists("Client"))
            {
                ClientRole newRole = new ClientRole("Client", "������� ����� ������ ������������� � �������� ���� ������ � �������");
                roleManager.Create(newRole);
            }
        }
    }
}
