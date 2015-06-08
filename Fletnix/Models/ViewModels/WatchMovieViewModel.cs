using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FletnixDatabase.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace Fletnix.Models.ViewModels
{
    public class WatchMovieViewModel
    {
        public WatchMovieViewModel()
        {
            Init();
        }


        public void Init()
        {
            var db = new Entities();
            var ratings = TitleReview.AllowedRatings;
            var reviewRatings = ReviewRating.AllowedRatings;
            ReviewRatings = reviewRatings;
            Ratings = ratings;
            Users = db.User;
            PurchaseHistories = db.PurchaseHistory;

        }


        public int userID { get; set; }
        public DateTime dateMinusOneDay = DateTime.Now.AddDays(-1);
        public String date { get; set; }
        public bool wroteReview = false;
        public List<SelectListItem> Ratings { get; set; }
        public List<SelectListItem> ReviewRatings { get; set; }
        public Title Title { get; set; }
        public TitleReview Review { get; set; }
        public int Rating { get; set; }
        [Display(Name ="Discounted price")]
        public string movieDiscountPrice { get; set; }
        public string Description { get; set; }
        public IQueryable<User> Users { get; set; }
        public IQueryable<PurchaseHistory> PurchaseHistories { get; set; }
    }
}