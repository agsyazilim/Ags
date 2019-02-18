using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Data.Domain.Media;
using Ags.Web.Framework.Models;

namespace Ags.Web.Areas.Admin.Models.Media.Galery
{
    public class PhotoGalleryModel:BaseAgsEntityModel
    {
        public PhotoGalleryModel()
        {
            AddGaleryPictureModel = new AddGaleryPictureModel();
            AddGaleryPictureModels = new List<AddGaleryPictureModel>();
            GalleryPictureSearchModel = new GalleryPictureSearchModel();
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        [Display(Name = "Galeri Adı :")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PhotoGallery"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }

        public int DisplayOrder { get; set; }

        public AddGaleryPictureModel AddGaleryPictureModel { get; set; }
        public IList<AddGaleryPictureModel> AddGaleryPictureModels { get; set; }
        public GalleryPictureSearchModel GalleryPictureSearchModel { get; set; }
    }
}