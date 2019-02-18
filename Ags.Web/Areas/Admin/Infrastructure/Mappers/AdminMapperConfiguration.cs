using Ags.Data.Core;
using Ags.Data.Domain.Blogs;
using Ags.Data.Domain.Catalog;
using Ags.Data.Domain.Logging;
using Ags.Data.Domain.Message;
using Ags.Data.Domain.News;
using Ags.Data.Domain.Polls;
using Ags.Data.Domain.Seo;
using Ags.Data.Domain.Stores;
using Ags.Data.Domain.Tasks;
using Ags.Data.Domain.Topics;
using Ags.Services.Seo;
using Ags.Web.Areas.Admin.Models.Blogs;
using Ags.Web.Areas.Admin.Models.Catalog;
using Ags.Web.Areas.Admin.Models.Common;
using Ags.Web.Areas.Admin.Models.Email;
using Ags.Web.Areas.Admin.Models.Enews;
using Ags.Web.Areas.Admin.Models.Logging;
using Ags.Web.Areas.Admin.Models.News;
using Ags.Web.Areas.Admin.Models.Polls;
using Ags.Web.Areas.Admin.Models.Stores;
using Ags.Web.Areas.Admin.Models.Tasks;
using Ags.Web.Areas.Admin.Models.Templates;
using Ags.Web.Areas.Admin.Models.Topics;
using Ags.Web.Framework.Models;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;


namespace Ags.Web.Areas.Admin.Infrastructure.Mappers
{
    /// <summary>
    /// AutoMapper configuration for admin area models
    /// </summary>
    public class AdminMapperConfiguration : Profile
    {
        #region Ctor

        public AdminMapperConfiguration()
        {


            //add some generic mapping rules
            ForAllMaps((mapConfiguration, map) =>
            {
                //exclude Form and CustomProperties from mapping BaseAgsModel
                if (typeof(BaseAgsModel).IsAssignableFrom(mapConfiguration.DestinationType))
                {
                    map.ForMember(nameof(BaseAgsModel.Form), options => options.Ignore());
                    map.ForMember(nameof(BaseAgsModel.CustomProperties), options => options.Ignore());
                }
            });

            CreateMap<BlogComment, BlogCommentModel>()
                .ForMember(model => model.Comment, options => options.Ignore())
                .ForMember(model => model.CreatedOn, options => options.Ignore())
                .ForMember(model => model.CustomerInfo, options => options.Ignore());
            CreateMap<BlogCommentModel, BlogComment>()
                .ForMember(entity => entity.BlogPost, options => options.Ignore())
                .ForMember(entity => entity.CommentText, options => options.Ignore())
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.CustomerId, options => options.Ignore());//customer gelecek

            CreateMap<BlogPost, BlogPostModel>()
                .ForMember(model => model.ApprovedComments, options => options.Ignore())
                .ForMember(model => model.CreatedOn, options => options.Ignore())
                .ForMember(model => model.EndDate, options => options.Ignore())
                .ForMember(model => model.NotApprovedComments, options => options.Ignore())
                .ForMember(model => model.AvailableEditors, options => options.Ignore())
                .ForMember(model => model.SeName, options => options.MapFrom(entity => ServiceProviderFactory.ServiceProvider.GetRequiredService<IUrlRecordService>().GetSeName(entity, true)))
                .ForMember(model => model.StartDate, options => options.Ignore());
            CreateMap<BlogPostModel, BlogPost>()
                .ForMember(entity => entity.BlogComments, options => options.Ignore())
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.EndDateUtc, options => options.Ignore())
                .ForMember(entity => entity.Customer, options => options.Ignore())
                .ForMember(entity => entity.StartDateUtc, options => options.Ignore());

            CreateMap<Category, CategoryModel>()
               .ForMember(model => model.AvailableCategories, options => options.Ignore())
               .ForMember(model => model.AvailableCategoryTemplates, options => options.Ignore())
                .ForMember(model => model.AvailableVideoGalleries, options => options.Ignore())
                .ForMember(model => model.AvailablePhotoGallery, options => options.Ignore())
                .ForMember(model => model.AvailableSliders, options => options.Ignore())
               .ForMember(model => model.Breadcrumb, options => options.Ignore())
                .ForMember(model => model.SeName, options => options.MapFrom(entity => ServiceProviderFactory.ServiceProvider.GetRequiredService<IUrlRecordService>().GetSeName(entity, true)));
            CreateMap<CategoryModel, Category>()
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.Deleted, options => options.Ignore())
                .ForMember(entity => entity.UpdatedOnUtc, options => options.Ignore());

            CreateMap<CategoryTemplate, CategoryTemplateModel>();
            CreateMap<CategoryTemplateModel, CategoryTemplate>();
            CreateMap<ActivityLog, ActivityLogModel>()
               .ForMember(model => model.ActivityLogTypeName, options => options.MapFrom(entity => entity.ActivityLogType.Name))
               .ForMember(model => model.CreatedOn, options => options.Ignore())
               .ForMember(model => model.CustomerEmail, options => options.MapFrom(entity => entity.Customer.Email));
            CreateMap<ActivityLogModel, ActivityLog>()
                .ForMember(entity => entity.ActivityLogType, options => options.Ignore())
                .ForMember(entity => entity.ActivityLogTypeId, options => options.Ignore())
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.Customer, options => options.Ignore())
                .ForMember(entity => entity.EntityId, options => options.Ignore())
                .ForMember(entity => entity.EntityName, options => options.Ignore());

            CreateMap<ActivityLogType, ActivityLogTypeModel>();
            CreateMap<ActivityLogTypeModel, ActivityLogType>()
                .ForMember(entity => entity.SystemKeyword, options => options.Ignore());

            CreateMap<Log, LogModel>()
                .ForMember(model => model.CreatedOn, options => options.Ignore())
                .ForMember(model => model.CustomerEmail, options => options.Ignore());
            CreateMap<LogModel, Log>()
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.Customer, options => options.Ignore())
                .ForMember(entity => entity.LogLevelId, options => options.Ignore());
            CreateMap<NewsItem, NewsItemModel>()
               .ForMember(model => model.ApprovedComments, options => options.Ignore())
               .ForMember(model => model.CreatedOn, options => options.Ignore())
                .ForMember(model => model.AvailableCategories, options => options.Ignore())
                .ForMember(model => model.AvailableEditors, options => options.Ignore())
               .ForMember(model => model.EndDate, options => options.Ignore())
               .ForMember(model => model.NotApprovedComments, options => options.Ignore())
               .ForMember(model => model.SeName, options => options.MapFrom(entity => ServiceProviderFactory.ServiceProvider.GetRequiredService<IUrlRecordService>().GetSeName(entity, true)))
               .ForMember(model => model.StartDate, options => options.Ignore());
            CreateMap<NewsItemModel, NewsItem>()
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.EndDateUtc, options => options.Ignore())
                .ForMember(entity => entity.NewsComments, options => options.Ignore())
                .ForMember(entity => entity.PictureId, options => options.Ignore())
                 .ForMember(entity => entity.VideoId, options => options.Ignore())
                .ForMember(entity => entity.Video, options => options.Ignore())
                 .ForMember(entity => entity.Customer, options => options.Ignore())
                .ForMember(entity => entity.StartDateUtc, options => options.Ignore());
            CreateMap<Poll, PollModel>()
              .ForMember(model => model.EndDate, options => options.Ignore())
              .ForMember(model => model.PollAnswerSearchModel, options => options.Ignore())
              .ForMember(model => model.StartDate, options => options.Ignore());
            CreateMap<PollModel, Poll>()
                .ForMember(entity => entity.EndDateUtc, options => options.Ignore())
                .ForMember(entity => entity.PollAnswers, options => options.Ignore())
                .ForMember(entity => entity.StartDateUtc, options => options.Ignore());
            CreateMap<UrlRecord, UrlRecordModel>()
           .ForMember(model => model.DetailsUrl, options => options.Ignore())
           .ForMember(model => model.Name, options => options.Ignore());
            CreateMap<UrlRecordModel, UrlRecord>()
                .ForMember(entity => entity.Slug, options => options.Ignore());
            CreateMap<Store, StoreModel>();
            CreateMap<StoreModel, Store>();
            CreateMap<ScheduleTask, ScheduleTaskModel>();
            CreateMap<ScheduleTaskModel, ScheduleTask>()
                .ForMember(entity => entity.Type, options => options.Ignore());
            CreateMap<Topic, TopicModel>()
                .ForMember(model => model.AvailableTopicTemplates, options => options.Ignore())
                .ForMember(model => model.SeName, options => options.MapFrom(entity => ServiceProviderFactory.ServiceProvider.GetRequiredService<IUrlRecordService>().GetSeName(entity, true)))
                .ForMember(model => model.Url, options => options.Ignore());
            CreateMap<TopicModel, Topic>();
            CreateMap<TopicTemplate, TopicTemplateModel>();
            CreateMap<TopicTemplateModel, TopicTemplate>();
            CreateMap<EnewsPaper, ENewsItemModel>()
                .ForMember(model => model.AvailableCategories, optins => optins.Ignore())
                .ForMember(model => model.Categori, options => options.Ignore())
                .ForMember(model => model.SeName,
                    options => options.MapFrom(entity =>
                        ServiceProviderFactory.ServiceProvider.GetRequiredService<IUrlRecordService>()
                            .GetSeName(entity, true)));
            CreateMap<ENewsItemModel, EnewsPaper>()
                .ForMember(entity => entity.NewsPaperCategory, options => options.Ignore());

            CreateMap<EmailAccount, EmailAccountModel>()
              .ForMember(model => model.IsDefaultEmailAccount, options => options.Ignore())
              .ForMember(model => model.Password, options => options.Ignore())
              .ForMember(model => model.SendTestEmailTo, options => options.Ignore());
            CreateMap<EmailAccountModel, EmailAccount>()
                .ForMember(entity => entity.Password, options => options.Ignore());


        }

        #endregion

    }
}