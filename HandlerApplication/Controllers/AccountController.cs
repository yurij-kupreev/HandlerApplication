using HandlerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HandlerApplication.DAL;
using Newtonsoft.Json;
using System.Configuration;

namespace HandlerApplication.Controllers
{

    public class AccountController : Controller
    {
        DataContext Context = new DataContext();
        //static Role role1 = new Role { RoleName = "Admin" };
        //static Role role2 = new Role { RoleName = "User" };

        //static User user1 = new User { Username = "admin", Email = "admin@ymail.com", FirstName = "Admin", Password = "123456", IsActive = true, CreateDate = DateTime.UtcNow, Roles = new List<Role>() { role1 } };
        //static User user2 = new User { Username = "user1", Email = "user1@ymail.com", FirstName = "User1", Password = "123456", IsActive = true, CreateDate = DateTime.UtcNow, Roles = new List<Role>() { role2 } };
        //static List<User> listOfUsers = new List<User>() { user1, user2 };
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {

            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var user = Context.Users.Where(u => u.Username == model.Username && u.Password == model.Password).FirstOrDefault();
                //var user = listOfUsers.Where(u => u.Username == model.Username && u.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    var roles = user.Roles.Select(m => m.RoleName).ToArray();

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = user.UserId;
                    //serializeModel.FirstName = user.FirstName;
                    //serializeModel.LastName = user.LastName;
                    serializeModel.roles = roles;

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    user.Email,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    false, //pass here true, if you want to implement remember me functionality
                    userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    //if (roles.Contains("Admin"))
                    //{
                    //    return RedirectToAction("Index", "Admin");
                    //}
                    //else if (roles.Contains("User"))
                    //{
                    //    return RedirectToAction("Index", "User");
                    //}
                    //else
                    //{
                    //    return RedirectToAction("Index", "Home");
                    //}
                    return RedirectToAction("Index", "Home");

                }

                ModelState.AddModelError("", "Incorrect username and/or password");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(RegisterViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                String role = ConfigurationManager.AppSettings["UserRoleName"];
                var user = new User() { Username = model.Username, Email = model.Email, Password = model.Password, Roles = new List<Role>() };
                if (Context.Users.Where(p => p.Email == user.Email).FirstOrDefault() == null)
                {
                    user.Roles.Add(Context.Roles.Where(p => p.RoleName == role).FirstOrDefault());
                    Context.Users.Add(user);
                    Context.SaveChanges();
                    var roles = user.Roles.Select(m => m.RoleName).ToArray();

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = user.UserId;
                    //serializeModel.FirstName = user.FirstName;
                    //serializeModel.LastName = user.LastName;
                    serializeModel.roles = roles;

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    user.Email,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    false, //pass here true, if you want to implement remember me functionality
                    userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "User with such email already exists");
            }
            ModelState.AddModelError("", "Incorrect username and/or password");
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", null);
        }
    }

}

