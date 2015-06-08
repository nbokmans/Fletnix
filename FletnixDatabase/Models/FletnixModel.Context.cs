﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FletnixDatabase.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cast> Cast { get; set; }
        public virtual DbSet<CastMember> CastMember { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Keyword> Keyword { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<MovieDiscount> MovieDiscount { get; set; }
        public virtual DbSet<PurchaseHistory> PurchaseHistory { get; set; }
        public virtual DbSet<ReviewRating> ReviewRating { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Season> Season { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<Title> Title { get; set; }
        public virtual DbSet<TitleReview> TitleReview { get; set; }
        public virtual DbSet<TvEpisode> TvEpisode { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserDiscount> UserDiscount { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        public virtual DbSet<WatchHistory> WatchHistory { get; set; }
    
        public virtual int uspBuyMovie(Nullable<int> titleID, Nullable<int> userID)
        {
            var titleIDParameter = titleID.HasValue ?
                new ObjectParameter("TitleID", titleID) :
                new ObjectParameter("TitleID", typeof(int));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspBuyMovie", titleIDParameter, userIDParameter);
        }
    
        public virtual int uspLogError(ObjectParameter errorLogID)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspLogError", errorLogID);
        }
    
        public virtual int uspPrintError()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspPrintError");
        }
    
        public virtual int uspWatchMovie(Nullable<int> titleID, Nullable<int> userID)
        {
            var titleIDParameter = titleID.HasValue ?
                new ObjectParameter("TitleID", titleID) :
                new ObjectParameter("TitleID", typeof(int));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspWatchMovie", titleIDParameter, userIDParameter);
        }
    }
}
