using BusinessLayer.Concrete;
using DataAccessLayer.EntityFreamework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProject.Areas.Member.Controllers
{
    [Area("Member")]
    [Route("Member/[controller]/[action]")]
    public class DestinationController : Controller
    {
        DestinationManager destinationManager=new DestinationManager(new EfDestinationDal());
        public IActionResult Index()
        {
            var values = destinationManager.GetList();
            return View(values);
        }
        public IActionResult GetCitiesSerachByName(string serachString)
        {
            ViewData["CurrentFilter"]=serachString;
            var values=from x in destinationManager.GetList() select x;
            if(!string.IsNullOrEmpty(serachString))
            {
                string firstChar = serachString.Substring(0, 1).ToUpper();
                string restOfString = serachString.Substring(1).ToLower();

                // Karşılaştırma yapılıyor
                string formattedSearchString = firstChar + restOfString;
                values = values.Where(y => y.City.StartsWith(formattedSearchString));
            }
            return View(values.ToList());
        }
    }
}
