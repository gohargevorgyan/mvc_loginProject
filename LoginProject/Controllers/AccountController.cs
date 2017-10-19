using LoginProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LoginProject.Controllers
{
    public class AccountController : Controller
    {
        
        //
        // GET: /Account/
        public ActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration( RegisterModel model)
        {
            if (ModelState.IsValid)
            { 
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.FirstOrDefault(u => u.UserName == model.UserName);
                }
                if (user == null)
                {
                    using (UserContext db = new UserContext())
                    {

                        db.Users.Add(new User { UserName = model.UserName, Password = model.Password });
                        db.SaveChanges();
                        user = db.Users.Where(u => u.UserName == model.UserName && u.Password == model.Password).FirstOrDefault();
                    }
                }

                if (user != null)
                {
                  
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }



            return View (model);
              
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);

                }

                if (user != null)
                {

                   // FormsAuthentication.SetAuthCookie(model.UserName, true);
                    var userCookie = new HttpCookie("UserName", model.UserName);
                    userCookie.Expires.AddDays(365); HttpContext.Response.SetCookie(userCookie);
                    return RedirectToAction("Home", "Account");

                }


            }

            return View(model);

        }


        public ActionResult Home()
        {
            string val = Request.Cookies["UserName"].Value;
            ViewBag.val = Request.Cookies["UserName"].Value;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Update(HomePageModel model)
        {
            string result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                result = "Ваш логин: " + Request.Cookies["UserName"];
            }
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return ("Sorry, this method can't be called only from AJAX.");
            }

            if (ModelState.IsValid)
            {
               
                using (UserContext db = new UserContext())
                {

                    string val = "";
                    if (Request.Cookies["UserName"] != null)
                    {
                        val = Request.Cookies["UserName"].Value;

                        var user = db.Users.Where(s=>s.UserName == val).FirstOrDefault<User>();// get student from db
                        if (user != null)// change user 
                        {
                            user.Name = model.Name;
                            user.Address = model.Address;
                            //db.Entry(user).State = System.Data.Entity.EntityState.Modified;//Modified
                            db.SaveChanges();//update

                        }
                        
                       
                    }
                
                }
               

            }

            return result;
        }
    }
}