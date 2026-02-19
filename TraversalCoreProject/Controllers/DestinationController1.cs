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
        [AllowAnonymous] // Zaten sınıf başında var ama metodun anonim erişime açık olduğunu netleştirir
        public async Task<IActionResult> DestinationDetails(int id)
        {
            ViewBag.i = id;
            ViewBag.destID = id;
 
            if (User.Identity.IsAuthenticated)
            {
                var value = await _userManager.FindByNameAsync(User.Identity.Name);
                if (value != null)
                {
                    ViewBag.userID = value.Id;
                }
            }

            // Veriyi her durumda çek ve View'a gönder
            var values = destinationManager.TGetDestinationWithGuide(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult DestinationDetails(Destination_yerler p)
        {
            return View();
        }
    }
}
