using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Models.Compay
{
    public class AddCompanyModel
    {
        public AddCompanyModel()
        {
            AvailableCategoryS = new List<SelectListItem>();
        }
        [Display(Name = "Firma Adı")]
        public string Name { get; set; }
        [Display(Name = "Firma Adresi")]
        public string Address { get; set; }
        [Display(Name = "Firma Telefonu")]
        public string Phone { get; set; }
        [Display(Name = "Firma Gsm")]
        public string Gsm { get; set; }
        [Display(Name = "Firma Fax")]
        public string Fax { get; set; }
        [Display(Name = "Firma Web Adresi")]
        public string Www { get; set; }
        [UIHint("Picture")]
        [Display(Name = "Firma Resim")]
        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public string City { get; set; }
        [Display(Name = "Video Link")]
        public string VideoEmbedCode { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
        [Display(Name = "Mail Adresi")]
        public string Url { get; set; }

        public int CategoryId { get; set; }
        public List<SelectListItem> AvailableCategoryS { get; set; }
    }
}