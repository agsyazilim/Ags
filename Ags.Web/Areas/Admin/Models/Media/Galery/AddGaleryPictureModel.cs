using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Media.Galery
{
    public class AddGaleryPictureModel:BaseAgsEntityModel
    {
        public int GaleriId { get; set; }

        [UIHint("Picture")]
        [AgsDisplayName("Resim :")]
        public int PictureId { get; set; }

        [AgsDisplayName("Resim :")]
        public string PictureUrl { get; set; }

        [AgsDisplayName("Sırası :")]
        public int DisplayOrder { get; set; }

        [AgsDisplayName("Alt Başlık Bilgisi :")]
        public string OverrideAltAttribute { get; set; }

        [AgsDisplayName("Başlık Bilgisi")]
        public string OverrideTitleAttribute { get; set; }
        [AgsDisplayName("Varsa Url Girebilirsiniz :")]
        public string Url { get; set; }
    }
}