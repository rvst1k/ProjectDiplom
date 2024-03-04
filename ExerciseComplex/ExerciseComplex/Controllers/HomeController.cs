using ExerciseComplex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace ExerciseComplex.Controllers
{
    public class HomeController : Controller
    {

        DiplomContext db;
        private readonly ILogger<HomeController> _logger;
        public HomeController( ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexAuthorized()
        {
            return View();
        }

        public IActionResult Complex()
        {
            return View();
        }
     
        public IActionResult Registration(string login, string password)
        {            
            using (DiplomContext db = new DiplomContext())
            {
                int count = (from c in db.Users where c.Login == login && c.Password == password && c.RolesId == 2 select c).Count();
                if (count != 0)
                {
                    return View("Register");
                }
                else
                {                                       
                    try
                    {
                        User z = new User();
                        z.Login = login;
                        z.Password = password;
                        z.RolesId = 2;
                        z.Gender = false;                       
                        db.Add(z);
                        db.SaveChanges();
                        return View("LoginPopup");
                        throw new Exception("Это ошибка!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
                        return View("Register");
                    }
            }
            }
        }

        public IActionResult ComplexAuthorized()
        {
            return View();
        }

        public IActionResult Exercise()
        {          
            return View();
        }

        public IActionResult ExerciseAuthorized()
        {
            return View();
        }

        public IActionResult Save(string login, string name)
        {
            using (DiplomContext db = new DiplomContext())
            {
                var validUser = db.Users.FirstOrDefault(u => u.Login == login);
                validUser.Name = name;
                ViewBag.Name = name;
                validUser.Name = ViewBag.Name;
                db.SaveChanges();
            }
            return View("LK_Profile");
        }

        public IActionResult LoginPopup()
        {          
            return View();
        }
        public IActionResult Register()
        {
            
            return View();
        }


        public IActionResult Table()
        {

            return View();
        }


        public IActionResult LK_Profile(string login, string password)
        {
            using (DiplomContext db = new DiplomContext())
            {
                var validUser = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password && u.RolesId == 2);
                var validAdmin = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password && u.RolesId == 1);                
                if (validUser != null)
                {
                    ViewBag.Login = validUser.Login;
                    ViewBag.Password = validUser.Password;
                    ViewBag.Name = validUser.Name;
                    ViewBag.Surname = validUser.Surname;
                    ViewBag.Patronymic = validUser.Patronymic;
                    return View();                    
                }
                else if(validAdmin != null)
                {
                    return View("Table");
                }
                else
                {
                    return View("LoginPopup");
                }

                
            }
        }

        public IActionResult Admin_Panel()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Delete1(int? id)
        {
            if (id != null)
            {
                User? user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}