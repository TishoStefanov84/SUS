using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MyFirstMvcApp.Services;
using MyFirstMvcApp.ViewModels.Users;
using SUS.HTTP;
using SUS.MvcFramework;

namespace MyFirstMvcApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public HttpResponse Login()
        {
            if (this.IsUserSignIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            if (this.IsUserSignIn())
            {
                return this.Redirect("/");
            }

            var userId = this.userService.GetUserId(username, password);
            if (userId == null)
            {
                return this.Error("Invalid username or password");
            }

            this.SignIn(userId);
            return this.Redirect("/Cards/All");
        }

        public HttpResponse Register()
        {
            if (this.IsUserSignIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel model)
        {
            if (this.IsUserSignIn())
            {
                return this.Redirect("/");
            }

            if (model.Username == null || model.Username.Length < 5 || model.Username.Length > 20)
            {
                return this.Error("Invalid username. The username should be between 5 and 20 characters.");
            }

            if (!Regex.IsMatch(model.Username, @"^[a-zA-Z0-9\.]+$"))
            {
                return this.Error("Invalid username. Only alphanumeric characters are allowed.");
            }

            if (string.IsNullOrWhiteSpace(model.Email) || !new EmailAddressAttribute().IsValid(model.Email))
            {
                return this.Error("Invalid email.");
            }

            if (model.Password == null || model.Password.Length < 6 || model.Password.Length > 20)
            {
                return this.Error("Invalid password. Password should be between 6 and 20 chareacters.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.Error("Passwords should be the same.");
            }

            if (!this.userService.IsUsernameAvailable(model.Username))
            {
                return this.Error("Username already taken.");
            }

            if (!this.userService.IsEmailAvailable(model.Email))
            {
                return this.Error("Email already taken.");
            }

            this.userService.CreateUser(model.Username, model.Email, model.Password);

            return this.Redirect("/Users/Login");
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
