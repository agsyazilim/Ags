// ***********************************************************************
// Assembly         : Nop.Web
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-27-2018
// ***********************************************************************
// <copyright file="StoreInformationSettingsModel.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Configuration
{
    /// <summary>
    /// Represents a store information settings model
    /// Implements the <see cref="BaseAgsModel" />
    /// Implements the <see cref="ISettingsModel" />
    /// </summary>
    /// <seealso cref="BaseAgsModel" />
    /// <seealso cref="ISettingsModel" />
    public partial class StoreInformationSettingsModel : BaseAgsModel, ISettingsModel
    {

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether store is closed
        /// </summary>
        /// <summary>
        /// Gets or sets a value indicating whether [store closed].
        /// </summary>
        /// <value><c>true</c> if [store closed]; otherwise, <c>false</c>.</value>
        [AgsDisplayName("Site Kapalı :")]
        [UIHint("Boolean")]
        public bool StoreClosed { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        /// <summary>
        /// Gets or sets the logo picture identifier.
        /// </summary>
        /// <value>The logo picture identifier.</value>
        [UIHint("Picture")]
        [AgsDisplayName("Logo Yükle :")]
        public int LogoPictureId { get; set; }

        /// <summary>
        /// Gets or sets a default store theme
        /// </summary>
        /// <summary>
        /// Gets or sets the header banner picture identifier.
        /// </summary>
        /// <value>The header banner picture identifier.</value>
        [UIHint("Picture")]
        [AgsDisplayName("Banner Yükle :")]
        public int HeaderBannerPictureId { get; set; }
        [UIHint("Picture")]
        [AgsDisplayName("Beyaz Logo Yükle :")]
        public int WhiteLogoPictureId { get; set; }
        /// <summary>
        /// Contact Mail
        /// </summary>
        /// <summary>
        /// Gets or sets the contact email.
        /// </summary>
        /// <value>The contact email.</value>
        [AgsDisplayName("Mail Ekle :")]
        public string ContactEmail { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether mini profiler should be displayed in public store (used for debugging)
        /// </summary>
        /// <summary>
        /// Gets or sets a value indicating whether [display mini profiler in public store].
        /// </summary>
        /// <value><c>true</c> if [display mini profiler in public store]; otherwise, <c>false</c>.</value>
        [UIHint("Boolean")]
        public bool DisplayMiniProfilerInPublicStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mini profiler should be displayed only for users with access to the admin area
        /// </summary>
        /// <summary>
        /// Gets or sets a value indicating whether [display mini profiler for admin only].
        /// </summary>
        /// <value><c>true</c> if [display mini profiler for admin only]; otherwise, <c>false</c>.</value>
        [UIHint("Boolean")]
        public bool DisplayMiniProfilerForAdminOnly { get; set; }



        /// <summary>
        /// Gets or sets a value of Facebook page URL of the site
        /// </summary>
        /// <summary>
        /// Gets or sets the facebook link.
        /// </summary>
        /// <value>The facebook link.</value>
        [AgsDisplayName("Facebook Link :")]
        public string FacebookLink { get; set; }

        /// <summary>
        /// Gets or sets a value of Twitter page URL of the site
        /// </summary>
        /// <value>The twitter link.</value>
       [AgsDisplayName("Twitter Link :")]
        public string TwitterLink { get; set; }

        /// <summary>
        /// Gets or sets a value of YouTube channel URL of the site
        /// </summary>
        /// <value>The youtube link.</value>
        [AgsDisplayName("Youtube Link :")]
        public string YoutubeLink { get; set; }

        /// <summary>
        /// Gets or sets a value of Google+ page URL of the site
        /// </summary>
        /// <value>The google plus link.</value>
        [AgsDisplayName("GooglePlus Link :")]
        public string GooglePlusLink { get; set; }
        /// <summary>
        /// Gets or sets Instagram
        /// </summary>
        /// <value>The instagram link.</value>
        [AgsDisplayName("Instagram Link :")]
        public string InstagramLink { get; set; }

        /// <summary>
        /// Default grid page size
        /// </summary>
        /// <summary>
        /// Gets or sets the default size of the grid page.
        /// </summary>
        /// <value>The default size of the grid page.</value>
        [UIHint("Int32")]
        [AgsDisplayName("Grid Sayfa Sayısı :")]
        public int DefaultGridPageSize { get; set; }

        /// <summary>
        /// Popup grid page size (for popup pages)
        /// </summary>
        /// <summary>
        /// Gets or sets the size of the popup grid page.
        /// </summary>
        /// <value>The size of the popup grid page.</value>
        [UIHint("Int32")]
        [AgsDisplayName("Sayfada Gelecek Liste Sayısı :")]
        public int PopupGridPageSize { get; set; }

        /// <summary>
        /// A comma-separated list of available grid page sizes
        /// </summary>
        /// <summary>
        /// Gets or sets the grid page sizes.
        /// </summary>
        /// <value>The grid page sizes.</value>
        [AgsDisplayName("Tabloda Gelecek liste :")]
        public string GridPageSizes { get; set; }

        /// <summary>
        /// Additional settings for rich editor
        /// </summary>
        /// <summary>
        /// Gets or sets the rich editor additional settings.
        /// </summary>
        /// <value>The rich editor additional settings.</value>
        public string RichEditorAdditionalSettings { get; set; }

        /// <summary>
        /// A value indicating whether to javascript is supported in rich editor
        /// </summary>
        /// <value><c>true</c> if [rich editor allow java script]; otherwise, <c>false</c>.</value>
        [UIHint("Boolean")]
        public bool RichEditorAllowJavaScript { get; set; }

        /// <summary>
        /// A value indicating whether to style tag is supported in rich editor
        /// </summary>
        /// <value><c>true</c> if [rich editor allow style tag]; otherwise, <c>false</c>.</value>
        [UIHint("Boolean")]
        public bool RichEditorAllowStyleTag { get; set; }

        /// <summary>
        /// A value indicating whether to use rich editor on message templates and campaigns details pages
        /// </summary>
        /// <value><c>true</c> if [use rich editor in message templates]; otherwise, <c>false</c>.</value>
        [AgsDisplayName("Rich Editor Kullan")]
        [UIHint("Boolean")]
        public bool UseRichEditorInMessageTemplates { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to use IsoDateFormat in JSON results (used for avoiding issue with dates in KendoUI grids)
        /// </summary>
        /// <value><c>true</c> if [use iso date format in json result]; otherwise, <c>false</c>.</value>
        [UIHint("Boolean")]
        public bool UseIsoDateFormatInJsonResult { get; set; }

        /// <summary>
        /// Indicates whether to use nested setting design
        /// </summary>
        /// <value><c>true</c> if [use nested setting]; otherwise, <c>false</c>.</value>
        [UIHint("Boolean")]
        public bool UseNestedSetting { get; set; }

        /// <summary>
        /// Picture size of customer avatars (if enabled)
        /// </summary>
        /// <summary>
        /// Gets or sets the size of the avatar picture.
        /// </summary>
        /// <value>The size of the avatar picture.</value>
        [UIHint("Int32")]
        [AgsDisplayName("Avatar Resim Boyutu :")]
        public int AvatarPictureSize { get; set; }

        /// <summary>
        /// Maximum allowed picture size. If a larger picture is uploaded, then it'll be resized
        /// </summary>
        /// <value>The maximum size of the image.</value>
        [UIHint("Int32")]
        [AgsDisplayName("Max Resim Boyutu :")]
        public int MaximumImageSize { get; set; }

        /// <summary>
        /// Gets or sets a default quality used for image generation
        /// </summary>
        /// <value>The default image quality.</value>
        [UIHint("Int32")]
        [AgsDisplayName("Varsayılan kalite :")]
        public int DefaultImageQuality { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether single (/content/images/thumbs/) or multiple (/content/images/thumbs/001/ and /content/images/thumbs/002/) directories will used for picture thumbs
        /// </summary>
        /// <value><c>true</c> if [multiple thumb directories]; otherwise, <c>false</c>.</value>
        [UIHint("Boolean")]
        public bool MultipleThumbDirectories { get; set; }
        [AgsDisplayName("Copy Right Bilgisi :")]
        public string CopyRigth { get; set; }

        #endregion



    }
}