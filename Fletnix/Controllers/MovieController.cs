using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime;
using Fletnix.Models;
using Fletnix.Models.QueryBuilders;
using FletnixDatabase.Models;
using Fletnix.Models.ViewModels;
using log4net.Repository.Hierarchy;
using Microsoft.Ajax.Utilities;

namespace Fletnix.Controllers
{
    public class MovieController : Controller
    {
        private Entities db = new Entities();

        // GET: Movie
        public ActionResult Index(string page)
        {
            return Index(new SearchParams(), page);
        }


        [HttpGet]
        public ActionResult Index(SearchParams searchParams, string page)
        {
            const int pageSize = SearchMovieViewModel.PAGE_SIZE;
            var skip = GetSkip(page, pageSize);

            var queryBuilder = new SearchMovieQueryBuilder()
                .Movie()
                .Title(searchParams.Title)
                .Genre(searchParams.Genre)
                .Firstname(searchParams.FirstName)
                .Keyword(searchParams.Keyword)
                .FromYear(searchParams.FromYear)
                .ToYear(searchParams.ToYear)
                .Ordering(searchParams.OrderBy, searchParams.Ordering)
                .Skip(skip)
                .Take(pageSize);

            var movies = queryBuilder.ExecuteQuery<SearchMovieResult>(db);
            var nResults = queryBuilder.GetNumTotalResults(db);

            var result = new SearchMovieViewModel
            {
                Movies = movies,
                NumResults = nResults,
                SearchParams = searchParams,
                Page = (skip / pageSize)
            };

            return View(result);
        }
        public ActionResult EditReview(int titleID, int userID)
        {
            if (Request.IsAuthenticated)
            {
                var username = User.Identity.Name;
                int loggedInID = db.User.First(user => user.Username.Equals(username)).UserID;
                if (userID == loggedInID)
                {
                    var model = new WatchMovieViewModel
                    {
                        Review = db.TitleReview.Find(titleID, userID)
                    };
                    short selected = (from s in db.TitleReview
                                      where s.UserID == userID && s.TitleID == titleID
                                      select s.Rating).First();
                    ViewBag.rating = new SelectList(model.Ratings, "text", "value", selected);
                    if (model.Review.AverageRating == 0)
                    {
                        if (model.Review == null)
                        {
                            return HttpNotFound();
                        }
                        ModelState.Clear();
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Watch", "Movie", new
            {
                ID = titleID
            });
        }



        private static int GetSkip(string page, int pageSize)
        {
            int skip;
            if (page == null)
            {
                skip = 0;
            }
            else
            {
                try
                {
                    skip = int.Parse(page) - 1;
                }
                catch (FormatException e)
                {
                    Debug.WriteLine("Unable to parse int: " + e.Message);
                    skip = 0;
                }
            }
            skip *= pageSize;
            return skip;
        }

        [HttpPost]
        public ActionResult BuyMovie(int titleID)
        {
            var title = db.Title.Find(titleID);
            var username = User.Identity.Name;
            var userId = db.User.First(u => u.Username.Equals(username)).UserID;

            db.uspBuyMovie(title.TitleID, userId);
            db.uspWatchMovie(title.TitleID, userId);
            return RedirectToAction("Watch", "Movie", new
            {
                ID = titleID
            });
        }

        private string GetVideoId(int videoID)
        {
            var title = db.Title.Find(videoID);
            if (title != null)
            {
                var searchParam = title.Title1;

                var youtube = new Models.Youtube.Youtube();
                return youtube.GetVideoIdOfFirstResult(searchParam + " trailer");

            }
            return null;
        }

        // GET: Movie/Watch/5
        public ActionResult Watch(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Movie");
            }

            var title = db.Title.Find(id);
            decimal discountTitle = title.DiscountPercentage;
            decimal discountUser = 0;
            var username = User.Identity.Name;
           
            if (!username.IsNullOrWhiteSpace())
            {
                discountUser = (decimal) db.User.First(user => user.Username.Equals(username)).DiscountPercentage;
            }


            if (title.VideoLocation == null)
            {
                var videoId = new MovieController().GetVideoId(id.Value);
                if (videoId == null)
                {
                    title.IsWatchable = false;
                }
                else
                {
                    title.IsWatchable = true;
                    const string youtubeUrl = @"http://www.youtube.com/embed/";
                    title.VideoLocation = youtubeUrl + videoId;
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Something went wrong.");
                }
            }

            var model = new WatchMovieViewModel
            {
                Title = title
            };
            if (discountUser != 0)
            {
                model.movieDiscountPrice = (Math.Round((decimal)title.Price * (1 - discountUser / 100) * (1 - discountTitle / 100), 2)).ToString();
            }
            else
            {
                model.movieDiscountPrice = "-";
            }
            if (model.Title == null)
            {
                return HttpNotFound();
            }



            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RateReview(int titleID, int userID, string rating)
        {
            var username = User.Identity.Name;
            var reviewRating = new ReviewRating
            {
                TitleReview_UserID = userID,
                TitleReview_TitleID = titleID,
                Rating = byte.Parse(rating),
                UserID = db.User.First(user => user.Username.Equals(username)).UserID
            };

            db.ReviewRating.Add(reviewRating);
            db.SaveChanges();
            return RedirectToAction("Watch", "Movie", new
            {
                ID = titleID
            });
        }

        // POST: TitleReview/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Watch(int id, TitleReview titleReview)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                titleReview.UserID = db.User.First(user => user.Username.Equals(username)).UserID;
                titleReview.TitleID = id;
                db.TitleReview.Add(titleReview);
                db.SaveChanges();
                return Watch(titleReview.TitleID);
            }

            ViewBag.TitleID = new SelectList(db.Title, "TitleID", "Title1", titleReview.TitleID);
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserType", titleReview.UserID);
            return Watch(titleReview.TitleID);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReview(int userID, int titleID)
        {
            var titleReview = db.TitleReview.Find(titleID, userID);

            if (titleReview == null)
            {
                return HttpNotFound();
            }
            var wmm = new WatchMovieViewModel
            {
                Title = db.Title.Find(titleID)
            };

            db.ReviewRating.RemoveRange(
                db.ReviewRating.Where(rr => rr.TitleReview_TitleID == titleID &&
                                            rr.TitleReview_UserID == userID));

            db.TitleReview.Remove(titleReview);

            db.SaveChanges();

            return RedirectToAction("Watch", "Movie", new
            {
                ID = titleID
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeReview(string newReview, int userID, int titleID, int Rating)
        {
            var titleReview = db.TitleReview.Find(titleID, userID);

            if (titleReview == null)
            {
                return HttpNotFound();
            }
            var wmm = new WatchMovieViewModel { Title = db.Title.First(m => m.TitleID.Equals(titleID)) };

            db.TitleReview.Remove(titleReview);
            db.SaveChanges();
            var temp = new TitleReview { TitleID = titleID, UserID = userID, Rating = (byte)Rating };
            if (!newReview.IsNullOrWhiteSpace())
            {
                temp.Description = newReview;
            }
            db.TitleReview.Add(temp);
            db.SaveChanges();
            return RedirectToAction("Watch", "Movie", new
            {
                ID = titleID
            });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Titles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Title title = db.Title.Find(id);
            if (title == null)
            {
                return HttpNotFound();
            }

            return View(title);
        }

        // POST: Titles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Title title)
        {
            if (((string)Session["UserType"] == "Administrator"))
            {
                if (ModelState.IsValid)
                {
                    var entry = db.Entry(title);
                    entry.State = EntityState.Modified;

                    entry.Property("DiscountPercentage").IsModified = false;
                    entry.Property("AverageRating").IsModified = false;


                    db.SaveChanges();
                }
            }
            else
            {
                ModelState.AddModelError("", "You must be logged in as administrator to edit movies.");
            }
            return View(title);
        }
    }
}

