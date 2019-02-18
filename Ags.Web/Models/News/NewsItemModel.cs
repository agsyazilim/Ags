using System;
using System.Collections.Generic;
using Ags.Data.Domain.Catalog;
using Ags.Web.Framework.Models;
using Ags.Web.Models.Media;

namespace Ags.Web.Models.News
{
    public class NewsItemModel : BaseAgsEntityModel
    {
        public NewsItemModel()
        {
            Comments = new List<NewsCommentModel>();
            AddNewComment = new AddNewsCommentModel();
            VideoGalleryModel = new VideoGalleryModel();

        }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public string PictureUrl { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategorySeName { get; set; }
        public string Title { get; set; }
        public string Short { get; set; }
        public string Full { get; set; }
        public bool BigNews { get; set; }
        public bool AllowComments { get; set; }
        public int NumberOfComments { get; set; }
        public int NumberOfRead { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CustomerName { get; set; }
        public string AvatarUrl { get; set; }
        
        public DateTime? EndDateUtc { get; set; }
        public Category Category { get; set; }
        public VideoGalleryModel VideoGalleryModel { get; set; }
        public List<PictureModel> PictureModels { get; set; }
        public int VideoId { get; set; }
        public IList<NewsCommentModel> Comments { get; set; }
        public AddNewsCommentModel AddNewComment { get; set; }
    }
}