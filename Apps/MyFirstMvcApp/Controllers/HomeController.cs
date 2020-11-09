using System;
using System.Linq;
using System.Text;
using MyFirstMvcApp.ViewModels;
using SUS.HTTP;
using SUS.MvcFramework;

namespace MyFirstMvcApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserSignIn())
            {
                return this.Redirect("/Cards/All");
            }

            return this.View();
        }

        public HttpResponse About()
        {
            this.SignIn("tisho");
            return this.View();
        }
    }
}
