using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace TraversalCoreProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Guides")]
    public class GuidesController : Controller
    {
        private readonly IGuideService _guideService;
        public GuidesController(IGuideService guideService) { _guideService = guideService; }

        [Route("Index")]
        [Route("")]
        public IActionResult Index() 
        { 
            var values = _guideService.GetList(); 
            return View(values); 
        }

        [Route("AddGuide")]
        [HttpGet]
        public IActionResult AddGuide() => View();

        [Route("AddGuide")]
        [HttpPost]
        public async Task<IActionResult> AddGuide(Guide guide, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Dosya yükleme yolu
                var resource = Directory.GetCurrentDirectory();
                var extension = Path.GetExtension(ImageFile.FileName);
                var imagename = Guid.NewGuid() + extension;
                var saveLocation = resource + "/wwwroot/guideimages/" + imagename;
        
                // Klasör yoksa oluştur
                var directoryPath = Path.Combine(resource, "wwwroot/guideimages");
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                // Dosyayı kaydet
                using (var stream = new FileStream(saveLocation, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
        
                // Veritabanına kaydedilecek yol
                guide.Image = "/guideimages/" + imagename;
            }
    
            // Eğer dosya yüklenmemişse, modeldeki 'Image' (URL alanı) zaten dolu gelecektir.
            _guideService.TAdd(guide);
            return RedirectToAction("Index");
        }

        [Route("DeleteGuide/{id}")]
        public IActionResult DeleteGuide(int id)
        {
            var value = _guideService.TGetById(id);
    
            // Fiziksel dosyayı silme işlemi
            if (!string.IsNullOrEmpty(value.Image) && !value.Image.StartsWith("http"))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", value.Image.TrimStart('/'));
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            _guideService.TDelete(value);
            return RedirectToAction("Index");
        }
        // DUZENLEME SAYFASI ACILMIYORDU, ROUTE EKLENDI
        [Route("EditGuide/{id}")]
        [HttpGet]
        public IActionResult EditGuide(int id)
        {
            var values = _guideService.TGetById(id);
            return View(values);
        }

        [Route("EditGuide/{id}")]
        [HttpPost]
        public IActionResult EditGuide(Guide guide)
        {
            _guideService.TUpdate(guide);
            return RedirectToAction("Index");
        }
    }
}
