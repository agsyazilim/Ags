using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Common
{
    public class AdvertisementModel:BaseAgsEntityModel
    {
        public AdvertisementModel()
        {
            AvailableSections = new List<SelectListItem>();
        }
        /// <summary>
        /// Gets or sets the picture identifier.
        /// </summary>
        /// <value>The picture identifier.</value>
        [UIHint("Picture")]
        [AgsDisplayName("Resim :")]
        public int PictureId { get; set; }
        /// <summary>
        /// Gets or sets the advertising space identifier.
        /// </summary>
        /// <value>The advertising space identifier.</value>
        [AgsDisplayName("Reklamın Çıkacağı Yer :")]
        [Required]
        public int SectionId { get; set; }
        /// <summary>
        /// Gets or sets the target identifier.
        /// </summary>
        /// <value>The target identifier.</value>
        [AgsDisplayName("Reklam Başka Sayfada Açılsın mı? :")]
        [UIHint("Boolean")]
        public bool TargetId { get; set; }
        /// <summary>
        /// Gets or sets the code flash.
        /// </summary>
        /// <value>The code flash.</value>
        [AgsDisplayName("Reklam Kodu Yerleştir :")]
        public string CodeFlash { get; set; }
        /// <summary>
        /// Gets or sets the flash code.
        /// </summary>
        /// <value>The flash code.</value>
        [AgsDisplayName("Flash Kodu Yerleştir :")]
        public string FlashCode { get; set; }
        /// <summary>
        /// Gets or sets the URL address.
        /// </summary>
        /// <value>The URL address.</value>
        [AgsDisplayName("Url Adresi Varsa :")]
        public string UrlAddress { get; set; }
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        [AgsDisplayName("Reklam Başlama Tarihi :")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        [AgsDisplayName("Rekla Sona Erme Tarihi :")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        [UIHint("DateTimeNullable")]
        [AgsDisplayName("Oluşturma Tarihi :")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is approved.
        /// </summary>
        /// <value><c>true</c> if this instance is approved; otherwise, <c>false</c>.</value>
        [AgsDisplayName("Reklam Onaylandı mı? :")]
        [UIHint("Boolean")]
        public bool IsApproved { get; set; }
        public string PictureUrl { get; set; }
        public IList<SelectListItem> AvailableSections { get; set; }
    }
}