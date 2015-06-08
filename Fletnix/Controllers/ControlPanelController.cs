using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fletnix.Models;
using Fletnix.Models.QueryBuilders;
using FletnixDatabase.Models;
using Fletnix.Models.ViewModels;
using log4net.Repository.Hierarchy;
using Microsoft.Ajax.Utilities;


namespace Fletnix.Controllers
{
    public class ControlPanelController : Controller
    {

        private Entities db = new Entities();

        public ActionResult Index()
        {
            if (IsAdmin())
            {
                return RedirectToAction("Administrators", "ControlPanel");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Administrators(String username)
        {
            if (IsAdmin())
            {
                if (username != null)
                {
                    var users = db.User.Where(user => user.Username.Contains(username)).Take(10).ToList();
                    var result = new ControlPanelViewModel
                    {
                        Users = users
                    };

                    return View(result);
                }
                else
                {
                    var users = db.User.Take(10);
                    var result = new ControlPanelViewModel
                    {
                        Users = users.ToList()
                    };

                    return View(result);
                }
            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult SwitchType(int userID, string usertype)
        {
            if (usertype != null)
            {
                var user = db.User.Find(userID);
                if (user != null)
                {
                    user.UserType = usertype;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Administrators", "ControlPanel");
        }


        public Boolean IsAdmin()
        {
            return (string)System.Web.HttpContext.Current.Session["UserType"] == "Administrator";
        }
    }
}