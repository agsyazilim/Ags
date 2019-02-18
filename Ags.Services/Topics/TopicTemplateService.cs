using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Topics;

namespace Ags.Services.Topics
{
    /// <summary>
    /// Topic template service
    /// </summary>
    public partial class TopicTemplateService : ITopicTemplateService
    {
        #region Fields

        private readonly IRepository<TopicTemplate> _topicTemplateRepository;

        #endregion

        #region Ctor

        public TopicTemplateService(
            IRepository<TopicTemplate> topicTemplateRepository)
        {
            this._topicTemplateRepository = topicTemplateRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete topic template
        /// </summary>
        /// <param name="topicTemplate">Topic template</param>
        public virtual void DeleteTopicTemplate(TopicTemplate topicTemplate)
        {
            if (topicTemplate == null)
                throw new ArgumentNullException(nameof(topicTemplate));

            _topicTemplateRepository.Delete(topicTemplate);

            //event notification
        }

        /// <summary>
        /// Gets all topic templates
        /// </summary>
        /// <returns>Topic templates</returns>
        public virtual IList<TopicTemplate> GetAllTopicTemplates()
        {
            IOrderedQueryable<TopicTemplate> query = from pt in _topicTemplateRepository.Table
                        orderby pt.DisplayOrder, pt.Id
                        select pt;

            List<TopicTemplate> templates = query.ToList();
            return templates;
        }

        /// <summary>
        /// Gets a topic template
        /// </summary>
        /// <param name="topicTemplateId">Topic template identifier</param>
        /// <returns>Topic template</returns>
        public virtual TopicTemplate GetTopicTemplateById(int topicTemplateId)
        {
            if (topicTemplateId == 0)
                return null;

            return _topicTemplateRepository.GetById(topicTemplateId);
        }

        /// <summary>
        /// Inserts topic template
        /// </summary>
        /// <param name="topicTemplate">Topic template</param>
        public virtual void InsertTopicTemplate(TopicTemplate topicTemplate)
        {
            if (topicTemplate == null)
                throw new ArgumentNullException(nameof(topicTemplate));

            _topicTemplateRepository.Insert(topicTemplate);

            //event notification
        }

        /// <summary>
        /// Updates the topic template
        /// </summary>
        /// <param name="topicTemplate">Topic template</param>
        public virtual void UpdateTopicTemplate(TopicTemplate topicTemplate)
        {
            if (topicTemplate == null)
                throw new ArgumentNullException(nameof(topicTemplate));

            _topicTemplateRepository.Update(topicTemplate);

            //event notification
        }

        #endregion
    }
}