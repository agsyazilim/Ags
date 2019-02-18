using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Customers
{
    public class AdminRegisterViewModel
    {
        public AdminRegisterViewModel()
        {
            IdentityRoles = new List<SelectListItem>();

        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre :")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrarı :")]
        [Compare("Password", ErrorMessage = "Şifreler Uyuşmuyor")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Adı :")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Soyadı :")]
        public string LastName { get; set; }
        [Display(Name = "Facebook Link :")]
        public string FacebookLink { get; set; }
        [Display(Name = "Twitter Link :")]
        public string TwitterLink { get; set; }
        [Display(Name = "Instagram Link :")]
        public string InstagramLink { get; set; }
        [Display(Name = "Facebook Link :")]
        public string AvatarUrl { get; set; }
        [Display(Name = "Facebook Link :")]
        public string NickName { get; set; }
        [Display(Name = "Telefon :")]
        public string Phone { get; set; }
        public string Id { get; set; }
        [UIHint("Picture")]
        [AgsDisplayName("Avatar Yükle :")]
        public int AvatarId { get; set; }
        public List<SelectListItem> IdentityRoles { get; set; }
        [Display(Name = "Rol Seçiniz :")]
        public string SelectedUserRoleId { get; set; }

    }
}
