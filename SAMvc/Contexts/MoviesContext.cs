using SAMvc.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SAMvc.Contexts
{
    public class MoviesContext : DbContext
    {
        public MoviesContext() : base("MoviesContext")
        {

        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<MovieDirector> MovieDirectors { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Fluent API
            modelBuilder.Entity<Review>()
                .HasRequired(review => review.Movie)// required: mutlaka olmalı, optinal: olmasa da olur
                .WithMany(movie => movie.Reviews)
                .HasForeignKey(review => review.MovieId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<MovieDirector>()
                .HasRequired(movieDirector => movieDirector.Movie)
                .WithMany(movie => movie.MovieDirectors)
                .HasForeignKey(movieDirector => movieDirector.MovieId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<MovieDirector>()
                .HasRequired(movieDirector => movieDirector.Director)
                .WithMany(director => director.MovieDirectors)
                .HasForeignKey(movieDirector => movieDirector.DirectorId)
                .WillCascadeOnDelete(false);
        }
    }
}