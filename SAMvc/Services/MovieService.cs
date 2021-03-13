using SAMvc.Contexts;
using SAMvc.Entities;
using SAMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SAMvc.Services
{
    public class MovieService
    {
        // CRUD

        //private MoviesContext _db = new MoviesContext();  _db objesi bu classta newleyip kullanmak yerine dependency injection üzerinden dışarıdan alıp kullanmak daha iyi bi yöntemmiş

        private readonly MoviesContext _db;

        public MovieService(MoviesContext db)
        {
            _db = db;
        }

        public IQueryable<MovieModel> GetQuery()
        {
            try
            {
                return _db.Movies.OrderBy(m => m.Name).Select(m => new MovieModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    BoxOfficeReturn = m.BoxOfficeReturn,
                    ProductionYear = m.ProductionYear,
                    Directors = m.MovieDirectors.Select(md => new DirectorModel()
                    {
                        Id = md.Director.Id,
                        Name = md.Director.Name,
                        Surname = md.Director.Surname,
                        Retired = md.Director.Retired
                    }).ToList(),
                    Reviews = m.Reviews.Select(r => new ReviewModel()
                    {
                        Id = r.Id,
                        Content = r.Content,
                        Rating = r.Rating,
                        Reviewer = r.Reviewer,
                        MovieId = r.MovieId
                    }).ToList()
                    //,DirectorNamesHtml = string.Join("<br />", m.MovieDirectors.Select(md => md.Director.Name + " " + md.Director.Surname))
                    ,
                    DirectorIds = m.MovieDirectors.Select(md => md.DirectorId).ToList()
                });
            }
            catch (Exception exc)
            {

                throw exc;
            }

        }

        public void Add(MovieModel model)
        {
            try
            {
                // 1. Yöntem
                //if (model.DirectorIds == null)
                //    model.DirectorIds = new List<int>();
                Movie entity = new Movie()
                {
                    Name = model.Name,
                    BoxOfficeReturn = model.BoxOfficeReturn,
                    ProductionYear = model.ProductionYear,
                    // 2. Yöntem
                    //MovieDirectors = (model.DirectorIds == null ? new List<int>() : model.DirectorIds).Select(did => new MovieDirector()
                    //{
                    //    DirectorId = did
                    //}).ToList()

                    // 3. Yöntem
                    MovieDirectors = (model.DirectorIds ?? new List<int>()).Select(dId => new MovieDirector()
                    {
                        DirectorId = dId
                    }).ToList()
                };
                _db.Movies.Add(entity);
                _db.SaveChanges();
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }

        public void Update(MovieModel model)
        {
            try
            {
                Movie entity = _db.Movies.Find(model.Id);
                _db.MovieDirectors.RemoveRange(entity.MovieDirectors);
                entity.Name = model.Name;
                entity.BoxOfficeReturn = model.BoxOfficeReturn;
                entity.ProductionYear = model.ProductionYear;
                entity.MovieDirectors = (model.DirectorIds ?? new List<int>()).Select(dID => new MovieDirector()
                {
                    DirectorId= dID
                }).ToList();
                                
                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                Movie entity = _db.Movies.Find(id);
                if (entity.Reviews != null && entity.Reviews.Count > 0)
                    return false;
                _db.MovieDirectors.RemoveRange(entity.MovieDirectors);
                _db.Movies.Remove(entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}