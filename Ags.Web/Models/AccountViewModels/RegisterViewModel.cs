using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Lütfen Mail Adresi Giriniz")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre Giriniz")]
        [StringLength(100, ErrorMessage = "{0} en az {2} ve en fazla {1} karakter uzunluğunda olmalıdır. ", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Şifre ve onay şifresi uyuşmuyor.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Lütfen Adınızı Giriniz")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyadınızı Giriniz")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [UIHint("Picture")]
        [AgsDisplayName("Avatar Yükle")]
        public int AvatarId { get; set; }

    }
}
