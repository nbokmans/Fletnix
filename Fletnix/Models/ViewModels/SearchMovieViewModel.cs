using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Fletnix.Models.QueryBuilders;
using FletnixDatabase.Models;
using System.ComponentModel.DataAnnotations;
using Foolproof;


namespace Fletnix.Models.ViewModels
{
    public class SearchMovieViewModel
    {
        public const int PAGE_SIZE = 50;
        public SearchMovieViewModel()
        {
            using (var db = new Entities())
            {
                var genres = new List<Genre> {new Genre {Genre1 = ""}};
                genres.AddRange(db.Genre.ToList());
                Genres = new SelectList(genres, "Genre1", "Genre1");
            }

            var ratings = TitleReview.AllowedRatings;
            Movies = new List<SearchMovieResult>();
            SearchParams = new SearchParams();
        }

        public int NumResults
        {
            get;
            set;
        }
        public int Page
        {
            get;
            set;
        }
        public IEnumerable<SearchMovieResult> Movies
        {
            get;
            set;
        }
        public SelectList Genres
        {
            get;
            set;
        }
        public SearchParams SearchParams
        {
            get;
            set;
        }

        public enum OrderOption
        {
            Title,
            Rating
        }
    }

    public class SearchParams
    {
        public string Title
        {
            get;
            set;
        }
        public string FirstName
        {
            get;
            set;
        }
        public string Keyword
        {
            get;
            set;
        }
        public string Genre
        {
            get;
            set;
        }

        [DataType(DataType.Text, ErrorMessage = "Please enter a valid year.")]
        [MinLength(4, ErrorMessage = "Please enter a valid year.")]
        [MaxLength(4, ErrorMessage = "Please enter a valid year.")]
        [RegularExpression(@"\d+", ErrorMessage = "Please enter a valid year.")]
        [Display(Name = "From year")]
        public string FromYear
        {
            get;
            set;
        }

        [DataType(DataType.Text, ErrorMessage = "Please enter a valid year.")]
        [MinLength(4, ErrorMessage = "Please enter a valid year.")]
        [MaxLength(4, ErrorMessage = "Please enter a valid year.")]
        [RegularExpression(@"\d+", ErrorMessage = "Please enter a valid year.")]
        [GreaterThanOrEqualTo("FromYear", ErrorMessage = "To year must be greater than from year.")]
        [Display(Name = "To year")]
        public string ToYear
        {
            get;
            set;
        }
        public string OrderBy
        {
            get;
            set;
        }
        public string Ordering
        {
            get;
            set;
        }
    }
}