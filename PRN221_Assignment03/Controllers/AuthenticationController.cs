
using Microsoft.AspNetCore.Mvc;
using PRN221_Assignment03.Models;

namespace PRN221_Assignment03.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly Prn221Asm03Context _context;
        public AuthenticationController(Prn221Asm03Context context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(AppUser user)
        {
            if (ModelState.IsValid)
            {
                var check = _context.AppUsers.FirstOrDefault(s => s.Email == user.Email);
                if (check == null)
                {
                    _context.Add(user);
                    _context.SaveChanges();
                    ViewBag.RegisterMessage = "Register account success";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.RegisterError = "Account already exists";
                    return View();
                }

            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var user = _context.AppUsers.SingleOrDefault(s => s.Email == email && s.Password == password);


                if (user != null)
                {
                
                    HttpContext.Session.SetString("Fullname", user.FullName);
                    HttpContext.Session.SetInt32("UserId", user.UserId);

                    return RedirectToAction("Index", "Posts", new { area = "" });


                }
                else
                {
                    ViewBag.loginError = "Invalid email or password";
                    return View();
                }

            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
