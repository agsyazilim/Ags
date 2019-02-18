using System.Collections.Generic;
using Ags.Data.Domain.Media;
using Ags.Web.Framework.Models;

namespace Ags.Web.Areas.Admin.Models.Videos
{
    public class VideoKategoriModel:BaseAgsEntityModel
    {
        public VideoKategoriModel()
        {

            VideoSearchModel = new VideoSearchModel();
            AddVideoModel = new AddVideoModel();
            AddVideoModels = new List<AddVideoModel>();
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoGallery"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }

        public VideoSearchModel VideoSearchModel { get; set; }
        public AddVideoModel AddVideoModel { get; set; }
        public IList<AddVideoModel> AddVideoModels { get; set; }

    }
}