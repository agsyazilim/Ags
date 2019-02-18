using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Data.Domain.Catalog;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Companies
{
    public class CompanyModel:BaseAgsEntityModel
    {
        public CompanyModel()
        {
            AvailableCatagories = new List<SelectListItem>();
            AvailableStateProvince = new List<SelectListItem>();
        }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        [AgsDisplayName("Adı :")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        [AgsDisplayName("Adresi :")]
        [Required]
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        [AgsDisplayName("Telefonu :")]
        public string Phone { get; set; }
        /// <summary>
        /// Gets or sets the GSM.
        /// </summary>
        /// <value>The GSM.</value>
        [AgsDisplayName("Cep Telefonu :")]
        public string Gsm { get; set; }
        /// <summary>
        /// Gets or sets the fax.
        /// </summary>
        /// <value>The fax.</value>
        [AgsDisplayName("Fax :")]
        public string Fax { get; set; }
        /// <summary>
        /// Gets or sets the WWW.
        /// </summary>
        /// <value>The WWW.</value>
       [AgsDisplayName("Web Adresi :")]
        public string Www { get; set; }
       
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Company"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        [AgsDisplayName("Yayında mı? :")]
        [UIHint("Boolean")]
        public bool Published { get; set; }
        /// <summary>
        /// Gets or sets the video embed code.
        /// </summary>
        /// <value>The video embed code.</value>
        [AgsDisplayName("Video Kodu :")]
        public string VideoEmbedCode { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [AgsDisplayName("Açıklama :")]
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the picture identifier.
        /// </summary>
        /// <value>The picture identifier.</value>
        [AgsDisplayName("Resim Yükle :")]
        [UIHint("Picture")]
        public int PictureId { get; set; }
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        [AgsDisplayName("Yayın Tarihi :")]
        [UIHint("DateTime")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        [UIHint("DateTime")]
        [AgsDisplayName("Yayından Kalkış Tarihi")]
        public DateTime EndDate { get; set; }

        public string SeName { get; set; }
        [AgsDisplayName("Kategorisi :")]
        [Required]
        [Range(1,100000)]
        public int CompanyCategoryId { get; set; }
        public IList<SelectListItem> AvailableCatagories { get; set; }
        /// <summary>
        /// Gets or sets the state provence identifier.
        /// </summary>
        /// <value>The state provence identifier.</value>
        [AgsDisplayName("Şehir :")]
        [Required]
        [Range(1,100)]
        public int StateProvenceId { get; set; }
        public IList<SelectListItem> AvailableStateProvince { get; set; }

    }
}