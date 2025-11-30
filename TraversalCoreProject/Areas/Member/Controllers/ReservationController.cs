using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFreamework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TraversalCoreProject.Areas.Member.Controllers
{
    [Area("Member")]
    public class ReservationController : Controller
    {
        DestinationManager destinationManager = new DestinationManager(new EfDestinationDal());
        ReservationManager reservationManager = new ReservationManager(new EfReservationDal());
        private readonly UserManager<AppUser> _userManager;
        public ReservationController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> MyCurrentReservation()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            var valuesList = reservationManager.GetListWithReservationByAccepted(values.Id);
            return View(valuesList);
        }
        public async Task<IActionResult> MyOldReservation()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            var valuesList = reservationManager.GetListWithReservationByPrevious(values.Id);
            return View(valuesList);
        }
        public async Task<IActionResult> MyApprovalReservation()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            var valuesList = reservationManager.GetListWithReservationByWaithAprroval(values.Id);
            return View(valuesList);
        }
        [HttpGet]
        public IActionResult NewReservation()
        {
            var values = destinationManager.GetList();
            List<SelectListItem> items = values.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.DestinationId.ToString()
            }).ToList();
            ViewBag.v = items;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewReservation(Reservation p)
        {
            // dropdown tekrar doldurma (hata döndüğünde view için gerekli)
            var values = destinationManager.GetList();
            ViewBag.v = values.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.DestinationId.ToString()
            }).ToList();

            if (!ModelState.IsValid)
            {
                return View(p);
            }

            // seçilen destinationId gelmiş mi kontrolü
            if (p.DestinationId <= 0)
            {
                ModelState.AddModelError("DestinationId", "Lütfen geçerli bir lokasyon seçin.");
                return View(p);
            }

            // DB'de seçilen destinasyonun varlığını kontrol et
            var selectedDestination = values.FirstOrDefault(d => d.DestinationId == p.DestinationId);
            if (selectedDestination == null)
            {
                ModelState.AddModelError("DestinationId", "Seçilen lokasyon veritabanında bulunamadı.");
                return View(p);
            }

            // kullanıcı kontrolü
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SignIn", "Login", new { area = "" });
            }

            // atamalar
            p.AppUserId = user.Id;
            p.Status = "Onay Bekliyor";

            reservationManager.TAdd(p);
            return RedirectToAction("MyCurrentReservation");
        }
        public IActionResult Deneme()
        {

            return View();
        }
    } 

}
