using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyFirstMvcApp.Controllers;
using MyFirstMvcApp.Data;
using SUS.HTTP;
using SUS.MvcFramework;

namespace MyFirstMvcApp
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices()
        {
        }

        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }

    }
}
