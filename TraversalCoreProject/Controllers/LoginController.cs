using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProject.Models;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace TraversalCoreProject.Controllers
{
	[AllowAnonymous]
	public class LoginController : Controller
	{
		private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _SignInManager;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        [HttpGet]
		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(UserRegistorViewModel p) // async eklendi
		{
            AppUser appUser = new AppUser()
            {
                Name = p.Name,
                Surname = p.SurName,
                Email = p.Mail,
                UserName = p.UserName
            };
            if (p.Password == p.ConifrmPassword)
            {
                var result = await _UserManager.CreateAsync(appUser, p.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("SignIn");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(p);
        }

		[HttpGet]
		public IActionResult SignIn()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(UserSignInViewModel p, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(p);

            var identifier = (p.UserName ?? string.Empty).Trim();

            // Kullanıcıyı önce e-posta olarak dene, yoksa kullanıcı adıyla dene
            AppUser user = null;

            if (identifier.Contains("@"))
            {
                user = await _UserManager.FindByEmailAsync(identifier);
            }

            if (user == null)
            {
                user = await _UserManager.FindByNameAsync(identifier);
            }

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı/e-posta veya şifre hatalı.");
                return View(p);
            }

            // Burada user.UserName ile giriş deniyoruz (PasswordSignInAsync username tabanlıdır)
            var result = await _SignInManager.PasswordSignInAsync(user.UserName, p.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // Admin rolü kontrolü
                var roles = await _UserManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                // Eğer returnUrl güvenli bir yerel url ise yönlendir, değilse ana sayfa
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return LocalRedirect(returnUrl);

                return RedirectToAction("Index", "Default");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Hesabınız geçici olarak kilitlenmiştir. Lütfen daha sonra tekrar deneyin.");
                return View(p);
            }

            // Başarısız giriş
            ModelState.AddModelError("", "Kullanıcı adı/e-posta veya şifre hatalı.");
            return View(p);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut(int reservation = 0)
        {
            await _SignInManager.SignOutAsync();

            if (reservation == 1)
            {
                return Redirect("/Default/Index#newReservation");
            }

            return RedirectToAction("Index", "Default");
        }
    }
}