using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ags.Data.Common;
using Ags.Data.Domain.Seo;

namespace Ags.Data.Domain.Catalog
{
    /// <summary>
    /// Class CompanyCategory.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class CompanyCategory :BaseEntity,ISlugSupported
    {
        /// <summary>
        /// The companies
        /// </summary>
        private ICollection<Company> _companies;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DisplayName("Adı")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the meta title.
        /// </summary>
        /// <value>The meta title.</value>
        [DisplayName("Meta Title")]
        public string MetaTitle { get; set; }
        /// <summary>
        /// Gets or sets the meta description.
        /// </summary>
        /// <value>The meta description.</value>
        [DisplayName("Meta Açıklama")]
        public string MetaDescription { get; set; }
        /// <summary>
        /// Gets or sets the meta keyword.
        /// </summary>
        /// <value>The meta keyword.</value>
        [DisplayName("Meta Anahtar Kelime")]
        public string MetaKeyword { get; set; }
        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>The parent identifier.</value>
        [DisplayName("Üst Kategori")]
        public int? ParentId { get; set; }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        [UIHint("DateTimeNullable")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>The update date.</value>
        [UIHint("DateTime")]
        [DisplayName("Güncllenme Tarihi")]
        public DateTime? UpdateDate { get; set; }
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        [UIHint("DateTime")]
        [DisplayName("Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        [UIHint("DateTime")]
        [DisplayName("Bitiş Tarihi")]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CompanyCategory"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        [DisplayName("Aktif mi?")]
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the companies.
        /// </summary>
        /// <value>The companies.</value>
        public virtual ICollection<Company> Companies
        {
            get=>_companies??(_companies=new List<Company>());
            protected set=>_companies=value;
        }
    }
}
