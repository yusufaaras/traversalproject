using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProject.ViewCompanent.MemberLayout
{
    public class _MemberLayoutHeaderContent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
