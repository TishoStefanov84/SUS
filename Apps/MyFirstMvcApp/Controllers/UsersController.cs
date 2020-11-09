using System;
using System.IO;
using System.Text;
using SUS.HTTP;
using SUS.MvcFramework;

namespace MyFirstMvcApp.Controllers
{
    public class UsersController : Controller
    {

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost("Users/Login")]
        public HttpResponse DoLogin()
        {
            return this.Redirect("/");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost("Users/Register")]
        public HttpResponse DoRegister()
        {
            return this.Redirect("/");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserSignIn())
            {
                return this.Error("Only logged-in users can logout.");
            }

            this.SignOut();
            return this.Redirect("/");
        }
    }
}
