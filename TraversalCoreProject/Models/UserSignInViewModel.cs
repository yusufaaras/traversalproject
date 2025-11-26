using System.ComponentModel.DataAnnotations;
using TraversalCoreProject.Areas.Admin.Models;

namespace TraversalCoreProject.Models
{
    public class UserSignInViewModel
    {
        public int id { get; set; } 
        [Required(ErrorMessage = "Lütfen Kullanıcı Adını Giriniz")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Lütfen Şifrenizi Giriniz")]
        public string Password { get; set; }

        //public RoleAssignViewModel Role { get; set; }
    }

}
