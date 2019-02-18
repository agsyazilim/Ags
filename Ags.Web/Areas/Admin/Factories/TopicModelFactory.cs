using System;
using System.Linq;
using AdminLTE.Areas.Admin.Factories;
using Ags.Data.Core;
using Ags.Data.Domain.Topics;
using Ags.Services.Seo;
using Ags.Services.Topics;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Topics;
using Ags.Web.Framework.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the topic model factory implementation
    /// </summary>
    public partial class TopicModelFactory : ITopicModelFactory
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ITopicService _topicService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public TopicModelFactory(
            IActionContextAccessor actionContextAccessor,
            IBaseAdminModelFactory baseAdminModelFactory,
            ITopicService topicService,
            IUrlHelperFactory urlHelperFactory,
            IUrlRecordService urlRecordService,
            IWebHelper webHelper)
        {
            this._actionContextAccessor = actionContextAccessor;
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._topicService = topicService;
            this._urlHelperFactory = urlHelperFactory;
            this._urlRecordService = urlRecordService;
            this._webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare topic search model
        /// </summary>
        /// <param name="searchModel">Topic search model</param>
        /// <returns>Topic search model</returns>
        public virtual TopicSearchModel PrepareTopicSearchModel(TopicSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged topic list model
        /// </summary>
        /// <param name="searchModel">Topic search model</param>
        /// <returns>Topic list model</returns>
        public virtual TopicListModel PrepareTopicListModel(TopicSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get topics
            System.Collections.Generic.IList<Topic> topics = _topicService.GetAllTopics(showHidden: true,
                storeId: 0,
                ignorAcl: true);

            //filter topics
            //TODO: move filter to topic service
            if (!string.IsNullOrEmpty(searchModel.SearchKeywords))
            {
                topics = topics.Where(topic => (topic.Title?.Contains(searchModel.SearchKeywords) ?? false) ||
                                               (topic.Body?.Contains(searchModel.SearchKeywords) ?? false)).ToList();
            }

            //prepare grid model
            TopicListModel model = new TopicListModel
            {
                Data = topics.PaginationByRequestModel(searchModel).Select(topic =>
                {
                    //fill in model values from the entity
                    TopicModel topicModel = topic.ToModel<TopicModel>();

                    //little performance optimization: ensure that "Body" is not returned
                    topicModel.Body = string.Empty;

                    return topicModel;
                }),
                Total = topics.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare topic model
        /// </summary>
        /// <param name="model">Topic model</param>
        /// <param name="topic">Topic</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Topic model</returns>
        public virtual TopicModel PrepareTopicModel(TopicModel model, Topic topic, bool excludeProperties = false)
        {

            if (topic != null)
            {
                //fill in model values from the entity
                model = model ?? topic.ToModel<TopicModel>();

                model.Url = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext)
                    .RouteUrl("Topic", new { SeName = _urlRecordService.GetSeName(topic) }, _webHelper.CurrentRequestProtocol);


            }

            //set default values for the new model
            if (topic == null)
            {
                model.DisplayOrder = 1;
                model.Published = true;
            }



            //prepare available topic templates
            _baseAdminModelFactory.PrepareTopicTemplates(model.AvailableTopicTemplates, false);



            return model;
        }

        #endregion
    }
}