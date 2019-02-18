using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Areas.Admin.Models.Media.Galery;
using Ags.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Media.Slider
{
    public class SliderModel:BaseAgsEntityModel
    {
        public SliderModel()
        {
            AddGaleryPictureModel = new AddGaleryPictureModel();
            AddGaleryPictureModels = new List<AddGaleryPictureModel>();
            SliderSearchModel = new SliderSearchModel();
            AvailableSection = new List<SelectListItem>();
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        [Display(Name = "Slider Adı :")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the section identifier.
        /// </summary>
        /// <value>The section identifier.</value>
        
        public int SectionId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AdminLTE.Areas.Admin.Models.Media.Slider"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime CreateDate { get; set; }
        public AddGaleryPictureModel AddGaleryPictureModel { get; set; }
        public IList<AddGaleryPictureModel> AddGaleryPictureModels { get; set; }
        public SliderSearchModel SliderSearchModel { get; set; }
        public IList<SelectListItem> AvailableSection { get; set; }

    }
}