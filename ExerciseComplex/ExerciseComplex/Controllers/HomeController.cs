using ExerciseComplex.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                    ViewBag.WarningMessage = "Пользователь с таким именем уже зарегистрирован. Пожалуйста, выберите другое имя.";
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
            using (DiplomContext db = new DiplomContext())
            {
                var exercisesdif = db.ExerciseDifficulties.Select(d => d.Name).ToList();
                ViewBag.ExerciseDifficulties = new SelectList(exercisesdif);

                var exercisetype = db.ExerciseTypes.Select(d => d.Name).ToList();
                ViewBag.ExerciseTypes = new SelectList(exercisetype);

                var exerciseaim = db.ExerciseAims.Select(d => d.Name).ToList();
                ViewBag.ExerciseAims = new SelectList(exerciseaim);

                return View();
            }
        }
        public IActionResult Generate(string exerciseDifficulty, string exerciseType, string exerciseAim)
        {
            int id_dif, id_type, id_aim;
            if (exerciseDifficulty == "Легко")
            {
                id_dif = 1;
            }
            else if(exerciseDifficulty == "Средне")
            {
                id_dif = 2;
            }
            else
            {
                id_dif = 3;
            }

            if (exerciseType == "Спина и Шея")
            {
                id_type = 1;
            }
            else if(exerciseType == "Ягодицы")
            {
                id_type = 2;
            }
            else if(exerciseType == "Пресс и косые")
            {
                id_type = 3;
            }
            else if(exerciseType== "Плечи")
            {
                id_type = 4;
            }
            else if(exerciseType == "Руки")
            {
                id_type = 5;
            }
            else if(exerciseType == "Грудь")
            {
                id_type= 6;
            }
            else
            {
                id_type = 7;
            }
            if(exerciseAim == "Похудеть")
            {
                id_aim = 1;
            }
            else if(exerciseAim == "Рельеф")
            {
                id_aim = 2;
            }
            else
            {
                id_aim = 3;
            }
            ViewBag.DifficultyId = id_dif;
            ViewBag.TypeId = id_type;
            ViewBag.AimId = id_aim;
            return View();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SaveComplex(string complexname, string complexdesc)
        {
            if (User.Identity.IsAuthenticated) 
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

                if (!string.IsNullOrEmpty(userId))
                {
                    using (var db = new DiplomContext())
                    {
                        var newComplex = new Complex
                        {
                            Name = complexname,
                            Description = complexdesc,
                            UserId = int.Parse(userId)
                        };

                        db.Complexes.Add(newComplex);
                        await db.SaveChangesAsync();
                    }
                }

                return RedirectToAction("LK_Profile");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult MyComplexes()
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
        public async Task<IActionResult> SaveProfile(UserProfileViewModel model)
        {

            using (DiplomContext db = new DiplomContext())
            {
                if (model.Login == null || model.Name == null || model.Surname == null || model.Patronymic == null) 
                {
                    
                }
                else
                {
                    var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                    var user = await db.Users.FirstOrDefaultAsync(o => o.Id == userId);

                    user.Login = model.Login;
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.Patronymic = model.Patronymic;

                    await db.SaveChangesAsync();
                }

                return RedirectToAction("LK_Profile");
            }
        }

        public async Task<IActionResult> MyComplexesData()
        {
            using (DiplomContext db = new DiplomContext())
            {               
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    int? userIdInt = int.Parse(userId); // Конвертируем userId в int?
                    var complexes = await db.Complexes.Where(c => c.UserId == userIdInt).ToListAsync();

                    return View(complexes);
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }


        public async Task<IActionResult> LK_Profile()
        {
            UserProfileViewModel model = new UserProfileViewModel();

            // Получение данных пользователя из базы данных
            using (DiplomContext db = new DiplomContext())
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                var user = await db.Users.FirstOrDefaultAsync(o => o.Id == userId);

                model.Login = user.Login;
                model.Name = user.Name;
                model.Surname = user.Surname;
                model.Patronymic = user.Patronymic;
            }

            return View(model); // Передача модели в представление
        }

        public async Task<IActionResult> Login(string login, string password)
        {
            using (DiplomContext db = new DiplomContext())
            {
                var user = await db.Users.FirstOrDefaultAsync(o => o.Login == login && o.Password == password);
                if (user != null)
                {
                    if (user.RolesId == 1)
                    {
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "2")
                };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        return RedirectToAction("Table");
                    }
                    else
                    {
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "1")
                };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        return RedirectToAction("LK_Profile");
                    }
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
