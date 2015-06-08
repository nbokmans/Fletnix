using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FletnixDatabase.Models;

namespace Fletnix.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Movies = new List<Movie>();
        }
        public IEnumerable<Movie> Movies { get; set; }
    }

}