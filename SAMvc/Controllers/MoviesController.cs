using SAMvc.Contexts;
using SAMvc.Entities;
using SAMvc.Models;
using SAMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SAMvc.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MoviesContext _db;
        private readonly MovieService _movieService;
        private readonly DirectorService _directorService;

        public MoviesController()
        {
            _db = new MoviesContext();
            _movieService = new MovieService(_db);
            _directorService = new DirectorService(_db);
        }

        // GET: Movies
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            List<Movie> movies = _db.Movies.ToList();
            return View(movies);
        }


        // GET: Movies/List
        public ActionResult List()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Login", "Account");
                List<MovieModel> model = _movieService.GetQuery().ToList();
                return View("MovieList", model);
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        public ActionResult ListAfterDelete(int? result = null)
        {
            try
            {
                if (result.HasValue)
                {
                    if (result.Value == 1)
                        TempData["Message"] = "Movie deleted successfully.";
                    else if (result.Value == 0)
                        TempData["Message"] = "Movie could not be deleted because there are relational reviews.";
                    else // -1
                        TempData["Message"] = "An error occured while deleting the movie!";
                }
                List<MovieModel> model = _movieService.GetQuery().ToList();
                return View("MovieList", model);
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        // GET: Movies/Create
        public ViewResult Create()
        {
            try
            {
                List<int> years = new List<int>();
                for (int year = DateTime.Now.Year + 1; year >= 1930; year--)
                {
                    years.Add(year);
                }
                ViewBag.Years = years;

                List<DirectorModel> directors = _directorService.GetQuery().ToList();
                ViewBag.Directors = directors;

                //return View();
                //return new ViewResult();
                return View();
            }
            catch (Exception exc)
            {
                return View("Exception");                
            }
        }

        /*
                     ActionResult
                     /          \
            ViewResult         ContentResult
         */

        public ContentResult GetHtmlContent()
        {
            //return new ContentResult
            return Content("<b><i>Content Result.</i></b>", "text/html");
        }

        public ActionResult GetMoviesXmlContent()
        {
            List<MovieModel> movies = _movieService.GetQuery().ToList();
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            xml += "<MovieModels>";
            foreach (MovieModel movie in movies)
            {
                xml += "<MovieModel>";
                xml += "<Id>" + movie.Id + "</Id>";
                xml += "<Name>" + movie.Name + "</Name>";
                xml += "<ProductionYear>" + movie.ProductionYear + "</ProductionYear>";
                xml += "<BoxOfficeReturn>" + movie.BoxOfficeReturn + "</BoxOfficeReturn>";
                xml += "</MovieModel>";
            }
            xml += "</MovieModels>";
            return Content(xml, "application/xml");
        }

        public string GetString()
        {
            return "String.";
        }

        public EmptyResult GetEmpty()
        {
            return new EmptyResult();
        }



        // GET: Movies/CreateSubmit
        //public ActionResult CreateSubmit(string Name, double? BoxOfficeReturn, string ProductionYear)
        //{
        //    return Content("Movie Submitted: Name = " + Name + ", BoxOfficeReturn = " + BoxOfficeReturn + ", Production Year = " + ProductionYear);
        //}
        [HttpPost]
        public ActionResult Create(string Name, double? BoxOfficeReturn, string ProductionYear, List<int> DirectorIds)
        {            
            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return Content("Name Must not be empty.");

                if (Name.Length > 250)
                    return Content("Name must not be empty and name must have maximum 250 characters.");

                MovieModel model = new MovieModel()
                {
                    Name = Name,
                    BoxOfficeReturn = BoxOfficeReturn,
                    ProductionYear = ProductionYear,
                    DirectorIds = DirectorIds
                };
                _movieService.Add(model);
                //return RedirectToAction("List", "Movies");
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        public ActionResult Details(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    //return View("_Exception");
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Id is required!");
                }

                MovieModel model = _movieService.GetQuery().SingleOrDefault(m => m.Id == id);
                if (model == null)
                {
                    //return View("_Exception");
                    //return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    return HttpNotFound();
                }
                return View(model);
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                MovieModel model = _movieService.GetQuery().SingleOrDefault(m => m.Id == id);
                if (model == null)
                    return HttpNotFound();
                List<int> years = new List<int>();
                for (int year = DateTime.Now.Year + 1; year >= 1930; year--)
                    years.Add(year);
                List<SelectListItem> yearSelectListItems = years.Select(y => new SelectListItem()
                {
                    Value = y.ToString(),
                    Text = y.ToString()
                }).ToList();
                SelectList yearSelectList = new SelectList(yearSelectListItems, "Value", "Text", model.ProductionYear);

                //ViewBag.Years = yearSelectList;
                ViewData["Years"] = yearSelectList;

                List<DirectorModel> directors = _directorService.GetQuery().ToList();
                MultiSelectList directorMultiSelectList = new MultiSelectList(directors, "Id", "FullName", model.DirectorIds);

                ViewData["Directors"] = directorMultiSelectList;

                return View(model);
            }
            catch (Exception)
            {
                return View("Exception");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovieModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            try
            {
                if (ModelState.IsValid)
                {
                    _movieService.Update(model);
                    return RedirectToAction("List");
                }

                List<int> years = new List<int>();
                for (int year = DateTime.Now.Year + 1; year >= 1930; year--)
                {
                    years.Add(year);
                }
                List<SelectListItem> selectListItems = years.Select(y => new SelectListItem()
                {
                    Value = y.ToString(),
                    Text = y.ToString()
                }).ToList();
                SelectList yearSelectList = new SelectList(selectListItems, "Value", "Text", model.ProductionYear);
                ViewBag.Years = yearSelectList;

                List<DirectorModel> directors = _directorService.GetQuery().ToList();
                MultiSelectList directorMultiSelectList = new MultiSelectList(directors, "Id", "FullName", model.DirectorIds);

                ViewData["Directors"] = directorMultiSelectList;


                return View(model);
            }
            catch (Exception)
            {
                return View("Exception");
            }
        }

        //public ActionResult DeleteMovie(int? id)
        //{
        //    try
        //    {
        //        if (id == null)
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        bool result = _movieService.Delete(id.Value);
        //        if (result)
        //            TempData["Message"] = "Movie deeted successfully.";
        //        else
        //            TempData["Message"] = "Movie could not be deleted because there are relational reviews.";

        //        return RedirectToAction("List");
        //    }
        //    catch (Exception)
        //    {
        //        TempData["Message"] = "An erro occured while deletin the movie!";
        //        return RedirectToAction("List");
        //    }
        //}

        public ActionResult Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                MovieModel model = _movieService.GetQuery().SingleOrDefault(m => m.Id == id);
                if (model == null)
                    return HttpNotFound();
                //if (model.Directors != null && model.Directors.Count > 0)
                //{
                //    model.DirectorNamesHtml = "";
                //    foreach (DirectorModel directorModel in model.Directors)
                //        model.DirectorNamesHtml += directorModel.Name + " " + directorModel.Surname + "<br />";
                //}
                return View(model);
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                bool result = _movieService.Delete(id.Value);
                if (result)
                    return RedirectToAction("ListAfterDelete", new { result = 1 });
                return RedirectToAction("ListAfterDelete", new { result = 0 });
            }
            catch (Exception exc)
            {
                return RedirectToAction("ListAfterDelete", new { result = -1 });
            }
        }
    }
}