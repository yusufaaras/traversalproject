using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProject.ViewCompanent.MemberLayout
{
    public class _MemberLayoutFooter:ViewComponent
    {
        public  IViewComponentResult Invoke()
        {
            return View();
        } 
    }
}
