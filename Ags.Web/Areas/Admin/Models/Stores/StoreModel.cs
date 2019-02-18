using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Stores
{
    /// <summary>
    /// Represents a store model
    /// </summary>
    public partial class StoreModel : BaseAgsEntityModel
    {

        #region Properties

        [AgsDisplayName("Adı :")]
        public string Name { get; set; }

        [AgsDisplayName("Url :")]
        public string Url { get; set; }

        [AgsDisplayName("Ssl Var :")]
        [UIHint("Boolean")]
        public virtual bool SslEnabled { get; set; }

        [AgsDisplayName("Hosts :")]
        public string Hosts { get; set; }

        [AgsDisplayName("Sırası")]
        public int DisplayOrder { get; set; }

        [AgsDisplayName("Firma Adı :")]
        public string CompanyName { get; set; }

        [AgsDisplayName("Adresi :")]
        public string CompanyAddress { get; set; }

        [AgsDisplayName("Telefonu :")]
        public string CompanyPhoneNumber { get; set; }
        #endregion
    }


}