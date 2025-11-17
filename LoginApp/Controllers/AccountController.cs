using AdminDashboard.Domain;
using AdminDashboard.Services;
using AdminDashboard.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LoginApp.Controllers
{
  
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            var LogedInUser = UserService.Login(Username, Password);

            if (LogedInUser == null)
            {

                ViewBag.LoginError = "Invalid username or password";

                return View();
            }

            if(LogedInUser.IsLocked)
            {
                ViewBag.LoginError = "Your account is locked. Please contact admin.";
                return View();
            }
           
            if (!LogedInUser.Password)
            {
                ViewBag.LoginError = "Invalid username or password";
                UserService.InceremntFaildAttempts(LogedInUser.Username);

                
                return View();
            }
            

             if(LogedInUser.Status == enRegStatus.pending)
             {
                  ViewBag.LoginError = "Your registration request is still pending.";
                  return View();
             }

             if (LogedInUser.Status == enRegStatus.rejected)
             {
                    ViewBag.LoginError = "Your registration request is rejected, you can't login.";
                    return View();
             }

             
                var ticket = new FormsAuthenticationTicket(
                        1,
                        Username,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        false,
                         LogedInUser.Type,
                        FormsAuthentication.FormsCookiePath
                    );



                string encryptedTicket = FormsAuthentication.Encrypt(ticket);


                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                authCookie.HttpOnly = false;
                Response.Cookies.Add(authCookie);


                HttpCookie userCookie = new HttpCookie("UserInfo");
                userCookie.HttpOnly = false;
                userCookie["Username"] = Username;
                userCookie["UserId"] = LogedInUser.UserId.ToString();
                userCookie["Type"] = LogedInUser.Type;
                userCookie["Email"] = LogedInUser.Email;
                userCookie.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(userCookie);


                 UserService.ResetFailedAttempts(LogedInUser.Username);

                return RedirectToAction("Index", "Home");
            



        }

        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            //Session.Clear();
            //Session.Abandon();

            //// Invalidate cookies
            //if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            //{
            //    var c = new HttpCookie(FormsAuthentication.FormsCookieName);
            //    c.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(c);
            //}

            //if (Request.Cookies["UserInfo"] != null)
            //{
            //    var c = new HttpCookie("UserInfo");
            //    c.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(c);
            //}

            //// Disable caching
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();
            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            return RedirectToAction("Login", "Account");
        }





        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Register(AddUserDto user)
        //{
        //    UserService.AddUser(user);

        //    return RedirectToAction("Login", "Account");
            
        //}



        public ActionResult UserProfile()
        {
            return View();
        }

    }




    
}