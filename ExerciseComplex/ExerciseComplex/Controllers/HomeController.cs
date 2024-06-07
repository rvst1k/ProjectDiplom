using ExerciseComplex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace ExerciseComplex.Controllers
{
    public class HomeController : Controller
    {

        DiplomContext db;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
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
                var existingUserCount = db.Users.Count(u => u.Login == login && u.Password == password && u.RolesId == 2);

                if (existingUserCount > 0)
                {
                    return View("Register");
                }

                try
                {
                    User newUser = new User
                    {
                        Login = login,
                        Password = password,
                        RolesId = 2,
                        Gender = false
                    };

                    db.Add(newUser);
                    db.SaveChanges();

                    return View("LoginPopup");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
                    return View("Register");
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

        [HttpPost]
        public IActionResult SomeEditsky(int userId, string name, string surname, string patronymic)
        {
            using (DiplomContext db = new DiplomContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    user.Name = name;
                    user.Surname = surname;
                    user.Patronymic = patronymic;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        // Обработка ошибки сохранения, например, вывод сообщения об ошибке или логирование
                        Console.WriteLine($"Ошибка при сохранении изменений: {ex.Message}");
                    }
                }
                return RedirectToAction("LK_Profile");
            }
        }
        public class UserProfileViewModel
        {
            public string Login { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Patronymic { get; set; }          
        }

        public IActionResult LK_Profile(string login, string name, string surname, string patronymic)
        {
            var viewModel = new UserProfileViewModel
            {
                Login = login,
                Name = name,
                Surname = surname,
                Patronymic = patronymic
               
            };
            return View(viewModel);
        }
       
        public IActionResult Login(string login, string password)
        {
            using (DiplomContext db = new DiplomContext())
            {                
                var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                {
                    if (user.RolesId == 1)
                    {
                        return RedirectToAction("Table", "Home");
                    }
                    else if (user.RolesId == 2)
                    {
                        return RedirectToAction("LK_Profile", "Home", new
                        {
                            login = user.Login,
                            name = user.Name,
                            surname = user.Surname,
                            patronymic = user.Patronymic
                       });
                    }
                }
                return RedirectToAction("Login", "Home");
            }
        }

        public IActionResult Admin_Panel()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name, string description, string preview, string link)
        {
            using (DiplomContext db = new DiplomContext())
            {
                Exercise exercise1 = new Exercise()
                {
                    Name = name,
                    Description = description,
                    Preview = preview,
                    Link = link,
                    TypeId = 1,
                    DifficultyId = 2,
                    AimId = 3,
                    CaloriesNumber = 1
                };
                db.Add(exercise1);
                db.SaveChanges();

                return View("Table");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            using (DiplomContext db = new DiplomContext())
            {
                if (id != null)
                {
                    Exercise exercise = await db.Exercises.FirstOrDefaultAsync(p => p.Id == id);
                    if (exercise != null)
                        return View(exercise);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string description, string preview, string link)
        {
            using (DiplomContext db = new DiplomContext())
            {
                Exercise exercise = await db.Exercises.FindAsync(id);
                if (exercise == null)
                {
                    return NotFound();
                }
                exercise.Name = name;
                exercise.Description = description;
                exercise.Preview = preview;
                exercise.Link = link;
                db.Exercises.Update(exercise);
                db.SaveChanges();
                return RedirectToAction("Table");
            }
        }

        public async Task<IActionResult> Delete(int id, string name, string description, string preview, string link)
        {
            using (DiplomContext db = new DiplomContext())
            {
                Exercise exercise = await db.Exercises.FindAsync(id);
                if (exercise == null)
                {
                    return NotFound();
                }
                db.Remove(exercise);
                db.SaveChanges();
                return RedirectToAction("Table");
            }
        }

        public async Task<IActionResult> About(int? id)
        {
            using (DiplomContext db = new DiplomContext())
            {
                if (id != null)
                {
                    Exercise exercise = await db.Exercises.FirstOrDefaultAsync(p => p.Id == id);
                    if (exercise != null)
                        return View(exercise);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> About(int id, string name, string description, string preview, string link)
        {
            return RedirectToAction("Table");
        }
    }
}
