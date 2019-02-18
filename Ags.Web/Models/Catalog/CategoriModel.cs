using System.Collections.Generic;
using Ags.Web.Framework.Models;
using Ags.Web.Models.Media;
using Ags.Web.Models.News;

namespace Ags.Web.Models.Catalog
{
    public class CategoriModel:BaseAgsEntityModel
    {
        public CategoriModel()
        {
           
            LargeNewsModels = new List<NewsItemModel>();
            NewsModels = new List<NewsItemModel>();
            PagingFilteringContext = new CatalogPagingFilteringModel();
            VideoGalleryModel = new VideoGalleryModel();
            GalleryModel = new GalleryModel();
            SliderModel = new SliderModel();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public IList<SubCategoryModel> SubCategories { get; set; }
        public IList<NewsItemModel> LargeNewsModels { get; set; }
        public IList<NewsItemModel> NewsModels { get; set; }
        public VideoGalleryModel VideoGalleryModel { get; set; }
        public GalleryModel GalleryModel { get; set; }
        public SliderModel SliderModel { get; set; }
        public int PhotoId { get; set; }
        public int VideoId { get; set; }
        public int SliderId { get; set; }
        public int BannerPictureId { get; set; }
        public int BannerLitlePictureId { get; set; }
        public string BannerPictureUrl { get; set; }
        public string BannerLittlePicturUrl { get; set; }
        public CatalogPagingFilteringModel PagingFilteringContext { get; set; }
        public partial class SubCategoryModel:BaseAgsEntityModel
        {
            public SubCategoryModel()
            {
                VideoGalleryModel = new VideoGalleryModel();
                GalleryModel = new GalleryModel();
                SliderModel = new SliderModel();
            }
            public string Name { get; set; }

            public string SeName { get; set; }

            public string Description { get; set; }
            public VideoGalleryModel VideoGalleryModel { get; set; }
            public GalleryModel GalleryModel { get; set; }
            public SliderModel SliderModel { get; set; }
        }
    }
}