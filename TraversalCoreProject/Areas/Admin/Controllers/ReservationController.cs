using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFreamework;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TraversalCoreProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]/{id?}")]
    [Authorize(Roles = "Admin")]
    public class ReservationController : Controller
    {
        private readonly ReservationManager _reservationManager;
        private readonly DestinationManager _destinationManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly Context _context;

        public ReservationController(UserManager<AppUser> userManager)
        {
            _reservationManager = new ReservationManager(new EfReservationDal());
            _destinationManager = new DestinationManager(new EfDestinationDal());
            _userManager = userManager;
            _context = new Context();
        }

        private void PopulateSelectLists()
        {
            var destinations = _destinationManager.GetList();
            ViewBag.Destinations = destinations
                .Select(d => new SelectListItem { Text = d.City, Value = d.DestinationId.ToString() })
                .ToList();

            var users = _userManager.Users
                .Select(u => new { u.Id, Display = (u.Email ?? u.UserName) })
                .ToList() 
                .Select(u => new SelectListItem { Text = u.Display, Value = u.Id.ToString() })
                .ToList();

            ViewBag.Users = users;
        }

        // GET: Admin/Reservation/Index
        public async Task<IActionResult> Index(string searchEmail, string sortOrder, string searchStatus)
        {
            var values = _context.Reservations
                .Include(r => r.Destination)
                .Include(r => r.AppUser)
                .AsQueryable();

            // 1. Email Araması
            if (!string.IsNullOrEmpty(searchEmail))
            {
                values = values.Where(x => x.AppUser.Email.Contains(searchEmail) || x.AppUser.UserName.Contains(searchEmail));
            }

            // 2. Durum Filtresi
            if (!string.IsNullOrEmpty(searchStatus))
            {
                values = values.Where(x => x.Status == searchStatus);
            }

            // 3. Sıralama Mantığı
            switch (sortOrder)
            {
                case "date_asc":
                    values = values.OrderBy(x => x.ReservationDate);
                    break;
                case "date_desc":
                    values = values.OrderByDescending(x => x.ReservationDate);
                    break;
                case "city_asc":
                    values = values.OrderBy(x => x.Destination.City);
                    break;
                case "city_desc":
                    values = values.OrderByDescending(x => x.Destination.City);
                    break;
                default:
                    values = values.OrderByDescending(x => x.ReservationId);
                    break;
            }

            return View(await values.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Destination)
                .Include(r => r.AppUser)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null) return NotFound();
            return View(reservation);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateSelectLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Reservation model)
        {
            if (!ModelState.IsValid)
            {
                PopulateSelectLists();
                return View(model);
            }
            model.Status = "Onaylandı";
            _reservationManager.TAdd(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var reservation = _reservationManager.TGetById(id);
            if (reservation == null) return NotFound();
            PopulateSelectLists();
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Reservation model)
        {
            _reservationManager.TUpdate(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var reservation = _reservationManager.TGetById(id);
            if (reservation != null)
            {
                reservation.Status = "Onaylandı";
                _reservationManager.TUpdate(reservation);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            var reservation = _reservationManager.TGetById(id);
            if (reservation != null)
            {
                reservation.Status = "Reddedildi";
                _reservationManager.TUpdate(reservation);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var reservation = _reservationManager.TGetById(id);
            if (reservation == null) return NotFound();
            return View(reservation);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var reservation = _reservationManager.TGetById(id);
            if (reservation != null)
            {
                _reservationManager.TDelete(reservation);
            }
            return RedirectToAction("Index");
        }
    }
}