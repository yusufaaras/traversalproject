using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProject.ViewCompanent.MemberDashBoard
{
    public class _MemberStatistic:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
