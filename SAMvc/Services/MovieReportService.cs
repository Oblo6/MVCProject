using SAMvc.Contexts;
using SAMvc.Entities;
using SAMvc.Models;
using System;
using System.Linq;

namespace SAMvc.Services
{
    public class MovieReportService
    {
        private readonly MoviesContext _db;

        public MovieReportService(MoviesContext db)
        {
            _db = db;
        }
        public IQueryable<MovieReportsInnerJoinModel> GetInnerJoinQuery()
        {
            try
            {
                IQueryable<Movie> movieQuery = _db.Movies.AsQueryable();
                IQueryable<MovieDirector> movieDirectorQuery = _db.MovieDirectors.AsQueryable();
                IQueryable<Director> directorQuery = _db.Directors.AsQueryable();
                IQueryable<Review> reviewQuery = _db.Reviews.AsQueryable();

                IQueryable<MovieReportsInnerJoinModel> query = from m in movieQuery
                                                               join md in movieDirectorQuery
                                                               on m.Id equals md.MovieId
                                                               join d in directorQuery
                                                               on md.DirectorId equals d.Id
                                                               join r in reviewQuery
                                                               on m.Id equals r.MovieId
                                                               select new MovieReportsInnerJoinModel()
                                                               {
                                                                   MovieName = m.Name,
                                                                   MovieProductionYear = m.ProductionYear,
                                                                   MovieBoxOfficeReturnValue = m.BoxOfficeReturn,
                                                                   DirectorFullName = d.Name + " " + d.Surname,
                                                                   DirectorRetiredValue = d.Retired,
                                                                   ReviewContent = r.Content,
                                                                   ReviewRatingValue = r.Rating,
                                                                   ReviewReviewer = r.Reviewer,
                                                                   ReviewDateValue = r.Date
                                                               };
                return query;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public IQueryable<MovieReportLeftOuterJoinModel> GetLeftOuterJoinquery()
        {
            try
            {
                IQueryable<Movie> movieQuery = _db.Movies.AsQueryable();
                IQueryable<MovieDirector> movieDirectorQuery = _db.MovieDirectors.AsQueryable();
                IQueryable<Director> directorQuery = _db.Directors.AsQueryable();
                IQueryable<Review> reviewQuery = _db.Reviews.AsQueryable();

                IQueryable<MovieReportLeftOuterJoinModel> query = from m in movieQuery
                                                                  join md in movieDirectorQuery
                                                                  on m.Id equals md.MovieId into movieDirectorJoin
                                                                  from subMovieDirectorJoin in movieDirectorJoin.DefaultIfEmpty()
                                                                  join d in directorQuery
                                                                  on subMovieDirectorJoin.DirectorId equals d.Id into directorJoin
                                                                  from subDirectorJoin in directorJoin.DefaultIfEmpty()
                                                                  join r in reviewQuery
                                                                  on m.Id equals r.MovieId into reviewJoin
                                                                  from subReviewJoin in reviewJoin.DefaultIfEmpty()
                                                                  select new MovieReportLeftOuterJoinModel()
                                                                  {
                                                                      MovieName = m.Name,
                                                                      MovieProductionYear = m.ProductionYear,
                                                                      MovieBoxOfficeReturnValue = m.BoxOfficeReturn,
                                                                      DirectorFullName = subDirectorJoin.Name + " " + subDirectorJoin.Surname,
                                                                      DirectorRetiredValue = subDirectorJoin.Retired,
                                                                      ReviewContent = subReviewJoin.Content,
                                                                      ReviewRatingValue = subReviewJoin.Rating,
                                                                      ReviewReviewer = subReviewJoin.Reviewer
                                                                      //,ReviewDate = subReviewJoin.Date
                                                                  };
                return query;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}