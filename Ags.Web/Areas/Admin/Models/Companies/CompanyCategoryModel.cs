using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Data.Domain.Catalog;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Companies
{
    public class CompanyCategoryModel:BaseAgsEntityModel
    {
        public CompanyCategoryModel()
        {
            AvailableCategories = new List<SelectListItem>();
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [AgsDisplayName("Adı :")]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the meta title.
        /// </summary>
        /// <value>The meta title.</value>
        [AgsDisplayName("Meta Title :")]
        public string MetaTitle { get; set; }
        /// <summary>
        /// Gets or sets the meta description.
        /// </summary>
        /// <value>The meta description.</value>
        [AgsDisplayName("Meta Açıklama :")]
        public string MetaDescription { get; set; }
        /// <summary>
        /// Gets or sets the meta keyword.
        /// </summary>
        /// <value>The meta keyword.</value>
        [AgsDisplayName("Meta Anahtar Kelime :")]
        public string MetaKeyword { get; set; }
        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>The parent identifier.</value>
        [AgsDisplayName("Üst Kategori :")]
        public int? ParentId { get; set; }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        [UIHint("DateTimeNullable")]
        [AgsDisplayName("Oluşturma Tarihi :")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>The update date.</value>
        [UIHint("DateTime")]
        [AgsDisplayName("Güncllenme Tarihi :")]
        public DateTime? UpdateDate { get; set; }
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        [UIHint("DateTime")]
        [AgsDisplayName("Başlangıç Tarihi :")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        [UIHint("DateTime")]
        [AgsDisplayName("Bitiş Tarihi :")]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CompanyCategory"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        [AgsDisplayName("Aktif mi? :")]
        [UIHint("Boolean")]
        public bool Published { get; set; }

        public string SeName { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }
    }
}