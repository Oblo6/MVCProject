using SAMvc.Contexts;
using SAMvc.Entities;
using SAMvc.Models;
using SAMvc.Services;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SAMvc.Controllers
{
    [HandleError]
    public class ReviewsController : Controller
    {
        private MoviesContext db = new MoviesContext();
        private ReviewService reviewService;
        private MovieService movieService;

        public ReviewsController()
        {
            reviewService = new ReviewService(db);
            movieService = new MovieService(db);
        }

        // GET: Reviews
        public ActionResult Index()
        {
            return View(reviewService.GetQuery().ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            ReviewModel review = reviewService.GetQuery().SingleOrDefault(r => r.Id == id);
            if (review == null)
                return HttpNotFound();
            return View(review);
        }

        // GET: Reviews/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name");
            ReviewModel model = new ReviewModel();
            reviewService.FillAllRatings(model);

            return View(model);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReviewModel review)
        {
            if (ModelState.IsValid)
            {
                reviewService.Add(review);
                return RedirectToAction("Index");
            }
            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name", review.MovieId);
            reviewService.FillAllRatings(review);

            return View(review);
        }


        // GET: Reviews/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            ReviewModel review = reviewService.GetQuery().SingleOrDefault(r => r.Id == id);
            if (review == null)
                return HttpNotFound();
            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name", review.MovieId);
            reviewService.FillAllRatings(review);

            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        //public ActionResult Edit([Bind(Include = "Id,Content,Rating,Reviewer,Date,MovieId")] Review review)
        public ActionResult Edit(ReviewModel review)
        {
            if (ModelState.IsValid)
            {
                reviewService.Update(review);
                return RedirectToAction("Index");
            }

            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name", review.MovieId);
            reviewService.FillAllRatings(review);
            return View(review);
        }

        // GET: Reviews/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Review review = db.Reviews.Find(id);
        //    if (review == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(review);
        //}

        // POST: Reviews/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Review review = db.Reviews.Find(id);
        //    db.Reviews.Remove(review);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            reviewService.Delete(id);
            return RedirectToAction("Index");
        }


        public ActionResult TextException()
        {
            throw new Exception("Reviews Test Exception!");
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
