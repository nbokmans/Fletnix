using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FletnixDatabase.Models;
using System.ComponentModel.DataAnnotations;

namespace Fletnix.Models.ViewModels
{
    public class ControlPanelViewModel
    {
        public ControlPanelViewModel()
        {
            var db = new Entities();
            var usertypes = new List<UserType>();
            usertypes.AddRange(db.UserType.ToList());
            UserType = new SelectList(usertypes, "UserType1", "UserType1");
        }
            public List<User> Users { get; set; }
            public String username { get; set; }
            public SelectList UserType { get; set; }
            public string usertype { get; set; }
            
    }
}