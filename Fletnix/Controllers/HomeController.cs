using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fletnix.Models.QueryBuilders;
using Fletnix.Models;
using Fletnix.Models.ViewModels;
using FletnixDatabase.Models;

namespace Fletnix.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();

        public ActionResult Index()
        {
            var movies = db.Movie.Where(m => m.Title.ImageLocation != null).Take(10);

            var hvm = new HomeViewModel();
            hvm.Movies = movies.ToList();
            

            return View(hvm);
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