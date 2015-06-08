using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web.Security;
using Fletnix.Models;
using Fletnix.Models.Authentication;
using Fletnix.Models.Mail;
using Fletnix.Models.ViewModels;
using FletnixDatabase.Models;
using Microsoft.Ajax.Utilities;

namespace Fletnix.Controllers
{
    public class UserController : Controller
    {

        private Entities db = new Entities();

        // GET: /User/
        public ActionResult Index()
        {
            return LogIn();
        }

        [HttpGet]
        // GET: /User/LogIn
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        // POST: /User/LogIn
        public ActionResult LogIn(string username, string password)
        {
            if (username.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("", "You must enter a username.");
                return View();
            }
            if (password.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("", "You must enter a password.");
                return View();
            }

            if (!ModelState.IsValid)
                return View();

            var user = db.User.FirstOrDefault(u => u.Username.ToLower().Equals(username.ToLower()));
            if (user != null)
            {
                if (user.IsVerified)
                {
                    if (UserAuthenticationService.UserIsAuthorized(user, password))
                    {
                        FormsAuthentication.SetAuthCookie(username, false);
                        System.Web.HttpContext.Current.Session["UserType"] = user.UserType;
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "The pasword you entered is incorrect.");
                }
                else
                {
                    ModelState.AddModelError("", "You need to verify your account first before you can log in.");
                }
            }
            else
            {
                ModelState.AddModelError("", "There is no user with that username.");
            }

            return View();
        }

        [HttpGet]
        // GET: /User/Register
        public ActionResult Register()
        {
            var db = new Entities();
            var model = new RegisterUserViewModel
            {
                Countries = new SelectList(db.Country.ToList(), "CountryCode", "CountryName")
            };

            return View(model);
        }



        [HttpPost]
        // POST: /User/Register
        public ActionResult Register(RegisterUserViewModel userModel)
        {
            if (!MailExtension.IsValidEmail(userModel.EmailAddress))
            {
                ModelState.AddModelError("" ,"This email address is not valid.");
                return Register();
            }
            if (ModelState.IsValid)
            {
                using (var db = new Entities())
                {
                    var user = new User
                    {
                        Username = userModel.Username,
                        EmailAddress = userModel.EmailAddress,
                        Password = userModel.Password,
                        Firstname = userModel.Firstname,
                        Lastname = userModel.Lastname,
                        CountryCode = userModel.CountryCode,
                        Gender = userModel.Gender,
                        BankAccountNumber = userModel.BankAccountNumber,
                        DateOfBirth = userModel.DateOfBirth
                    };


                    if (db.User.Any(dbUser => dbUser.Username.ToLower().Equals(user.Username.ToLower())))
                    {
                        ModelState.AddModelError("", "A user with this username already exists.");
                        return Register();
                    }
                    //Set user attributes
                    UserAuthenticationService.SaltAndHashUserPassword(user);
                    user.VerificationToken = Guid.NewGuid().ToString("N");
                    if (user.DateOfBirth > DateTime.Now)
                    {
                        user.DateOfBirth = null;
                    }
                    user.IsVerified = FletnixDatabase.Models.User.DEFAULT_IS_VERIFIED;
                    user.UserType = FletnixDatabase.Models.User.DEFAULT_USER_TYPE;
                    user.CreationDate = DateTime.Now;

                    db.User.Add(user);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Something went wrong with your registration. Please try again.");
                        return Register();
                    }

                    SendVerificationMail(user);

                    return RedirectToAction("RegisterStepTwo");
                }
            }

            return Register();
        }

        private static void SendVerificationMail(User user)
        {
            var email = new VerificationMail
            {
                User = user,
                VerificationUrl = @"http://94.124.143.212/User/Verify/" + user.VerificationToken
            };

#if DEBUG
            email.VerificationUrl = @"http://localhost:1378/User/Verify/" + user.VerificationToken;
#endif

            email.Send();
        }

        [HttpGet]
        // GET: /User/RegisterStepTwo
        public ActionResult RegisterStepTwo()
        {
            return View();
        }

        // GET: /User/LogOff
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        // GET: /User/Verify
        public ActionResult Verify(string id)
        {
            using (var db = new Entities())
            {
                try
                {
                    var dbUser = db.User.First(user => user.VerificationToken == id);
                    if (dbUser != null)
                    {
                        if (dbUser.IsVerified)
                        {
                            ModelState.AddModelError("", "This account has already been verified.");
                        }
                        else
                        {
                            dbUser.IsVerified = true;
                            dbUser.VerificationToken = null;
                            db.SaveChanges();
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    ModelState.AddModelError("", "Something went wrong verifying this account. Please try again.");
                }

            }

            return View();
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            var db = new Entities();
            var model = new UserProfileViewModel();

            var username = User.Identity.Name;
            var user = db.User.FirstOrDefault(u => u.Username.ToLower().Equals(username.ToLower()));
            if (user != null)
            {
                if (user.IsVerified)
                {
                    model.BankAccountNumber = user.BankAccountNumber;
                    model.Firstname = user.Firstname;
                    model.Lastname = user.Lastname;
                    model.Gender = user.Gender;
                    model.emailadress = user.EmailAddress;
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult UserProfile(UserProfileViewModel userModel)
        {
            var db = new Entities();
            var username = User.Identity.Name;

            var user = db.User.FirstOrDefault(u => u.Username.ToLower().Equals(username.ToLower()));
            if (user != null)
            {
                if (user.IsVerified)
                {
                    if (userModel.NewPassword != null)
                    {
                        user.Password = userModel.NewPassword;
                        UserAuthenticationService.SaltAndHashUserPassword(user);
                    }
                    if (userModel.BankAccountNumber != null)
                    {
                        user.BankAccountNumber = userModel.BankAccountNumber;
                    }
                    if (userModel.Firstname != null)
                    {
                        user.Firstname = userModel.Firstname;
                    }
                    if (userModel.Lastname != null)
                    {
                        user.Lastname = userModel.Lastname;
                    }
                    if (userModel.Gender != null)
                    {
                        user.Gender = userModel.Gender;
                    }
                    if (userModel.emailadress != null)
                    {
                        user.EmailAddress = userModel.emailadress;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Something went wrong. Please try again.");
                        return UserProfile();
                    }
                }
            }
            TempData["Message"] = "Updated Successfully";
            return RedirectToAction("UserProfile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SwitchRole(int userID)
        {
            var User = db.User.FirstOrDefault(u => u.UserID == userID);
            User.UserType = User.UserType == "Administrator" ? "Customer" : "Administrator";
            db.SaveChanges();
            return RedirectToAction("Administrators", "ControlPanel");
        }

        [HttpGet]
        public ActionResult UserHistory(UserProfileViewModel userModel)
        {
            var db = new Entities();
            var username = User.Identity.Name;
            var user = db.User.FirstOrDefault(u => u.Username.ToLower().Equals(username.ToLower()));
            if (user != null)
            {
                userModel.UserPurchaseHistory = db.PurchaseHistory.Where(ph => ph.UserID == user.UserID).ToList();
                userModel.UserWatchHistory = db.WatchHistory.Where(m => m.UserID == user.UserID).ToList();
                return View(userModel);
            }
            return View();
        }
        [ChildActionOnly]
        public ActionResult _UserSuccess()
        {
            return PartialView();
        }
    }
}

