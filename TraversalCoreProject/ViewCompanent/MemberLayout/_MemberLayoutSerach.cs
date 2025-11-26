using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProject.ViewCompanent.MemberLayout
{
    public class _MemberLayoutSerach:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
