using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fletnix.Models
{

    public class SearchMovieResult
    {
        public int TitleID { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public int? Duration { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        public DateTime? PublicationDate { get; set; }
        public decimal? AverageRating { get; set; }
    }
}
