using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFreamework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TraversalCoreProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Destination")]
    public class DestinationController : Controller
    {
        private readonly IDestinationService _destinationService;

        public DestinationController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            var values = _destinationService.GetList();
            return View(values);
        }

        // YENI ROTA EKLEME SAYFASININ ACILMASI ICIN BU ROTA SART
        [HttpGet]
        [Route("AddDestination")]
        public IActionResult AddDestination()
        {
            IGuideService guideService = new GuideManager(new EfGuideDal());
            var guideList = guideService.GetList()
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.GuideId.ToString()
                }).ToList();
            ViewBag.Guides = guideList;
            return View();
        }

        [HttpPost]
        [Route("AddDestination")]
        public async Task<IActionResult> AddDestination(Destination_yerler destination, IFormFile fileImage,
            IFormFile fileCover, IFormFile fileImage2)
        {
            await HandleImageUploads(destination, fileImage, fileCover, fileImage2);
            _destinationService.TAdd(destination);
            return RedirectToAction("Index", "Destination", new { area = "Admin" });
        }

        [HttpGet]
        [Route("UpdateDestination/{id}")]
        public IActionResult UpdateDestination(int id)
        {
            var values = _destinationService.TGetById(id);
            return View(values);
        }

        [HttpPost]
        [Route("UpdateDestination/{id}")]
        public async Task<IActionResult> UpdateDestination(Destination_yerler destination, IFormFile fileImage,
            IFormFile fileCover, IFormFile fileImage2)
        {
            await HandleImageUploads(destination, fileImage, fileCover, fileImage2);
            _destinationService.TUpdate(destination);
            return RedirectToAction("Index", "Destination", new { area = "Admin" });
        }

        [Route("DeleteDestination/{id}")]
        public IActionResult DeleteDestination(int id)
        {
            var values = _destinationService.TGetById(id);

            // Silinecek resim yollarını bir listeye alalım
            string[] imagesToDelete = { values.Image, values.CoverImage, values.Image2 };

            foreach (var imagePath in imagesToDelete)
            {
                if (!string.IsNullOrEmpty(imagePath) && !imagePath.StartsWith("http"))
                {
                    // Veritabanındaki yol "/destinationimages/resim.jpg" ise baştaki / işaretini temizleyip tam yolu buluruz
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
            }

            _destinationService.TDelete(values);
            return RedirectToAction("Index", "Destination", new { area = "Admin" });
        }

        // Ortak Fotoğraf Yükleme Metodu
        private async Task HandleImageUploads(Destination_yerler destination, IFormFile f1, IFormFile f2, IFormFile f3)
        {
            async Task<string> SaveFile(IFormFile file)
            {
                if (file == null || file.Length == 0) return null;
                var extension = Path.GetExtension(file.FileName);
                var imageName = Guid.NewGuid() + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/destinationimages/", imageName);
                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
                return "/destinationimages/" + imageName;
            }

            var p1 = await SaveFile(f1);
            if (p1 != null) destination.Image = p1;
            var p2 = await SaveFile(f2);
            if (p2 != null) destination.CoverImage = p2;
            var p3 = await SaveFile(f3);
            if (p3 != null) destination.Image2 = p3;
        }
    }
}