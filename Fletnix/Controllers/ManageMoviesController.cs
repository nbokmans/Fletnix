using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FletnixDatabase.Models;

namespace Fletnix.Controllers
{
    public class ManageMoviesController : Controller
    {
        private Entities db = new Entities();

        // GET: ManageMovies
        public ActionResult Index()
        {
            var title = db.Title.Include(t => t.Movie).Include(t => t.TvEpisode);
            return View(title.Take(50).ToList());
        }

        // GET: ManageMovies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var title = db.Title.Find(id);
            if (title == null)
            {
                return HttpNotFound();
            }
            return View(title);
        }

        // GET: ManageMovies/Create
        public ActionResult Create()
        {
            ViewBag.TitleID = new SelectList(db.Movie, "TitleID", "TitleID");
            ViewBag.TitleID = new SelectList(db.TvEpisode, "TitleID", "TitleID");
            return View();
        }

        // POST: ManageMovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TitleID,Title1,Price,IsWatchable,Duration,Description,PublicationDate,DiscountPercentage,AverageRating")] Title title)
        {
            if (ModelState.IsValid)
            {
                db.Title.Add(title);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TitleID = new SelectList(db.Movie, "TitleID", "TitleID", title.TitleID);
            ViewBag.TitleID = new SelectList(db.TvEpisode, "TitleID", "TitleID", title.TitleID);
            return View(title);
        }

        // GET: ManageMovies/Edit/5
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
            ViewBag.TitleID = new SelectList(db.Movie, "TitleID", "TitleID", title.TitleID);
            ViewBag.TitleID = new SelectList(db.TvEpisode, "TitleID", "TitleID", title.TitleID);
            return View(title);
        }

        // POST: ManageMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TitleID,Title1,Price,IsWatchable,Duration,Description,PublicationDate,DiscountPercentage,AverageRating")] Title title)
        {
            if (ModelState.IsValid)
            {
                db.Entry(title).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TitleID = new SelectList(db.Movie, "TitleID", "TitleID", title.TitleID);
            ViewBag.TitleID = new SelectList(db.TvEpisode, "TitleID", "TitleID", title.TitleID);
            return View(title);
        }

        // GET: ManageMovies/Delete/5
        public ActionResult Delete(int? id)
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
        
        // POST: ManageMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Title title = db.Title.Find(id);
            db.Title.Remove(title);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
