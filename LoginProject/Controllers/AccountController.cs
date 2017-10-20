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
            /*if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home", "Account");
            }*/
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
                    user = db.Users.FirstOrDefault(u => u.UserName == model.UserName);
                    if (user.Password != model.Password )
                    {
                        ModelState.AddModelError("", "Пользователя с таким паролем нет");
                        return View(model);
                    }

                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return RedirectToAction("Home", "Account");

                    }
                }

            }
            return View(model);
        }

        public ActionResult Home()
        {
                ViewBag.val = User.Identity.Name;
                 return View();
        }
        
            [HttpPost]
            [ValidateAntiForgeryToken]
        public string Update( HomePageModel model)
        {
            string result = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                result = "Ваш логин: " + User.Identity.Name;
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
                    if (User.Identity.Name != null)
                    {
                        val = User.Identity.Name;

                        var user = db.Users.Where(s=>s.UserName == val).FirstOrDefault<User>();
                        if (user != null)
                        {
                            user.Name = model.Name;
                            user.Address = model.Address;
                           
                            db.SaveChanges();

                        }
                      
                    }
                }
            }

            return result;
        }
    }
}