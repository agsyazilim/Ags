﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Data.Domain.Media;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Videos
{
    public class VideoModel:BaseAgsEntityModel
    {
        public VideoModel()
        {
            AvailableVideoGalery  = new List<SelectListItem>();

        }
        /// <summary>
        /// Gets or sets the video gallery identifier.
        /// </summary>
        /// <value>The video gallery identifier.</value>
        [AgsDisplayName("Video Galery :")]
        public int VideoGalleryId { get; set; }
        [UIHint("Picture")]
        [Display(Name = "Video Resim Yükle")]
        public int PictureId { get; set; }
        /// <summary>
        /// resim url
        /// </summary>
        [Display(Name = "Resim url")]
        public string PictureUrl { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        [AgsDisplayName("Sırası :")]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the descriptions.
        /// </summary>
        /// <value>The descriptions.</value>
        [AgsDisplayName("Sırası :")]
        public string Descriptions { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Video"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        [AgsDisplayName("Yayında :")]
        [UIHint("Boolean")]
        public bool Published { get; set; }
        /// <summary>
        /// Gets or sets the embed code.
        /// </summary>
        /// <value>The embed code.</value>
        [AgsDisplayName("Yayında :")]
        public string EmbedCode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is approved.
        /// </summary>
        /// <value><c>true</c> if this instance is approved; otherwise, <c>false</c>.</value>
        [AgsDisplayName("Onaylı :")]
        [UIHint("Boolean")]
        public bool IsApproved { get; set; }

        public IList<SelectListItem> AvailableVideoGalery { get; set; }
    }
}