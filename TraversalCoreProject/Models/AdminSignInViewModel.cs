using EntityLayer.Concrete;

namespace TraversalCoreProject.Models
{
    public class AdminSignInViewModel:AppRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
