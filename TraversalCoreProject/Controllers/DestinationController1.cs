using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFreamework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TraversalCoreProject.Controllers
{
    [AllowAnonymous]
    public class DestinationController1 : Controller
    {
        DestinationManager destinationManager = new DestinationManager(new EfDestinationDal());
        private readonly UserManager<AppUser> _userManager;

        public DestinationController1(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var values = destinationManager.GetList();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> DestinationDetails(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.i = id;
                ViewBag.destID = id;
                var value = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.userID = value.Id;
                var values = destinationManager.TGetDestinationWithGuide(id);
                return View(values);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public IActionResult DestinationDetails(Destination_yerler p)
        {
            return View();
        }
    }
}
