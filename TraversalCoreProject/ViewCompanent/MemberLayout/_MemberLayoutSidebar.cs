using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProject.ViewCompanent.MemberLayout
{
    public class _MemberLayoutSidebar:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
