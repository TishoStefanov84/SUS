using System.Linq;
using MyFirstMvcApp.Data;
using MyFirstMvcApp.ViewModels;
using SUS.HTTP;
using SUS.MvcFramework;

namespace MyFirstMvcApp.Controllers
{
    public class CardsController : Controller
    {
        public HttpResponse Add()
        {
            if (!this.IsUserSignIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost("/Cards/Add")]
        public HttpResponse DoAdd()
        {
            var dbContext = new ApplicationDbContext();

            if (this.Request.FormData["name"].Length < 5)
            {
                return this.Error("Name should be at least 5 characters long.");
            }

            dbContext.Cards.Add(new Card
            {
                Attack = int.Parse(this.Request.FormData["attack"]),
                Health = int.Parse(this.Request.FormData["health"]),
                Description = this.Request.FormData["description"],
                Name = this.Request.FormData["name"],
                ImageUrl = this.Request.FormData["image"],
                Keyword = this.Request.FormData["keyword"],
            });

            dbContext.SaveChanges();

            return this.Redirect("/");
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignIn())
            {
                return this.Redirect("/Users/Login");
            }

            var db = new ApplicationDbContext();
            var cardsViewModel = db.Cards.Select(x => new CardViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Attack = x.Attack,
                Health = x.Health,
                ImageUrl = x.ImageUrl,
                Type = x.Keyword,

            }).ToList();

            return this.View(cardsViewModel);
        }

        public HttpResponse Collection()
        {
            if (!this.IsUserSignIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }
    }
}
