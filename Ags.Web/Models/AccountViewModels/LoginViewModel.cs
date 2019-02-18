using System.ComponentModel.DataAnnotations;

namespace Ags.Web.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Mail Adresi Giriniz")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre Giriniz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla?")]
        public bool RememberMe { get; set; }
    }
}
