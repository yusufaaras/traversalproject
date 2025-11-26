using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFreamework;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProject.ViewCompanent.Comments
{
    public class _CommentList:ViewComponent
    {
        CommentManager commentManager =new CommentManager(new EfCommentDal());
        Context context = new Context();
        public IViewComponentResult Invoke(int id)
        {
            ViewBag.commentCount =context.Comments.Where(x=>x.DestinationId==id).Count() ;
            var values =commentManager.TGetListCommentWithDestinationAndUser(id);
            return View(values);
        }
    }
}
