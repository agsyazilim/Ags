using System;
using System.Linq;
using Ags.Data.Common;
using Ags.Data.Core.Caching;
using Ags.Data.Domain.Topics;
using Ags.Services.Seo;
using Ags.Services.Topics;
using Ags.Web.Models.Topics;

namespace Ags.Web.Factories
{
    /// <summary>
    /// Represents the topic model factory
    /// </summary>
    public partial class TopicModelFactory : ITopicModelFactory
    {
        #region Fields

        private readonly IStaticCacheManager _cacheManager;
        private readonly IStoreContext _storeContext;
        private readonly ITopicService _topicService;
        private readonly ITopicTemplateService _topicTemplateService;
        private readonly IUrlRecordService _urlRecordService;


        #endregion

        #region Ctor

        public TopicModelFactory(
            IStaticCacheManager cacheManager,
            IStoreContext storeContext,
            ITopicService topicService,
            ITopicTemplateService topicTemplateService,
            IUrlRecordService urlRecordService)
        {
            this._cacheManager = cacheManager;
            this._storeContext = storeContext;
            this._topicService = topicService;
            this._topicTemplateService = topicTemplateService;
            this._urlRecordService = urlRecordService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare the topic model
        /// </summary>
        /// <param name="topic">Topic</param>
        /// <returns>Topic model</returns>
        protected virtual TopicModel PrepareTopicModel(Topic topic)
        {
            if (topic == null)
                throw new ArgumentNullException(nameof(topic));

            TopicModel model = new TopicModel
            {
                Id = topic.Id,
                SystemName = topic.SystemName,
                IncludeInSitemap = topic.IncludeInSitemap,
                IsPasswordProtected = topic.IsPasswordProtected,
                Title = topic.Title,
                Body = topic.Body,
                MetaKeywords = topic.MetaKeywords,
                MetaDescription = topic.MetaDescription,
                MetaTitle = topic.MetaTitle,
                SeName = _urlRecordService.GetSeName(topic),
                TopicTemplateId = topic.TopicTemplateId,
                Published = topic.Published
            };
            return model;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the topic model by topic identifier
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <returns>Topic model</returns>
        public virtual TopicModel PrepareTopicModelById(int topicId)
        {
            Topic topic = _topicService.GetTopicById(topicId);
            //ACL (access control list)
            if (topic == null)
                return null;
            return PrepareTopicModel(topic);
        }

        /// <summary>
        /// Get the topic model by topic system name
        /// </summary>
        /// <param name="systemName">Topic system name</param>
        /// <returns>Topic model</returns>
        public virtual TopicModel PrepareTopicModelBySystemName(string systemName)
        {
            //load by store
            Topic topic = _topicService.GetTopicBySystemName(systemName, _storeContext.CurrentStore.Id);
            if (topic == null)
                return null;
            return PrepareTopicModel(topic);
        }

        /// <summary>
        /// Get topic template view path
        /// </summary>
        /// <param name="topicTemplateId">Topic template identifier</param>
        /// <returns>View path</returns>
        public virtual string PrepareTemplateViewPath(int topicTemplateId)
        {
            TopicTemplate template = _topicTemplateService.GetTopicTemplateById(topicTemplateId);
            if (template == null)
                template = _topicTemplateService.GetAllTopicTemplates().FirstOrDefault();
            if (template == null)
                throw new Exception("No default template could be loaded");
            return template.ViewPath;
        }

        #endregion
    }
}