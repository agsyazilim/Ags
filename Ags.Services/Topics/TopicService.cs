using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Caching;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Topics;
using Ags.Services.Events;

namespace Ags.Services.Topics
{
    /// <summary>
    /// Topic service
    /// </summary>
    public partial class TopicService : ITopicService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Topic> _topicRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly string _entityName;

        #endregion

        #region Ctor

        public TopicService(
            ICacheManager cacheManager,
            IRepository<Topic> topicRepository, IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._topicRepository = topicRepository;
            this._eventPublisher = eventPublisher;
            this._entityName = typeof(Topic).Name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="topic">Topic</param>
        public virtual void DeleteTopic(Topic topic)
        {
            if (topic == null)
                throw new ArgumentNullException(nameof(topic));

            _topicRepository.Delete(topic);
            _eventPublisher.EntityDeleted(topic);
            //cache
            _cacheManager.RemoveByPattern(AgsTopicDefaults.TopicsPatternCacheKey);

            //event notification
        }

        /// <summary>
        /// Gets a topic
        /// </summary>
        /// <param name="topicId">The topic identifier</param>
        /// <returns>Topic</returns>
        public virtual Topic GetTopicById(int topicId)
        {
            if (topicId == 0)
                return null;
            string key = string.Format(AgsTopicDefaults.TopicsByIdCacheKey, topicId);
            return _cacheManager.Get(key, () => _topicRepository.GetById(topicId));
        }

        /// <summary>
        /// Gets a topic
        /// </summary>
        /// <param name="systemName">The topic system name</param>
        /// <param name="storeId">Store identifier; pass 0 to ignore filtering by store and load the first one</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Topic</returns>
        public virtual Topic GetTopicBySystemName(string systemName, int storeId = 0, bool showHidden = false)
        {
            if (string.IsNullOrEmpty(systemName))
                return null;

            IQueryable<Topic> query = _topicRepository.Table;
            query = query.Where(t => t.SystemName == systemName);
            if (!showHidden)
                query = query.Where(c => c.Published);
            query = query.OrderBy(t => t.Id);
            List<Topic> topics = query.ToList();
            return topics.FirstOrDefault();
        }

        /// <summary>
        /// Gets all topics
        /// </summary>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <param name="ignorAcl">A value indicating whether to ignore ACL rules</param>
        /// <param name="showHidden">A value indicating whether to show hidden topics</param>
        /// <returns>Topics</returns>
        public virtual IList<Topic> GetAllTopics(int storeId, bool ignorAcl = false, bool showHidden = false)
        {
            string key = string.Format(AgsTopicDefaults.TopicsAllCacheKey, storeId, ignorAcl, showHidden);
            return _cacheManager.Get(key, () =>
            {
                IQueryable<Topic> query = _topicRepository.Table;
                query = query.OrderBy(t => t.DisplayOrder).ThenBy(t => t.SystemName);

                if (!showHidden)
                    query = query.Where(t => t.Published);



                return query.ToList();
            });
        }

        /// <summary>
        /// Inserts a topic
        /// </summary>
        /// <param name="topic">Topic</param>
        public virtual void InsertTopic(Topic topic)
        {
            if (topic == null)
                throw new ArgumentNullException(nameof(topic));

            _topicRepository.Insert(topic);
            _eventPublisher.EntityInserted(topic);
            //cache
            _cacheManager.RemoveByPattern(AgsTopicDefaults.TopicsPatternCacheKey);

            //event notification
        }

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="topic">Topic</param>
        public virtual void UpdateTopic(Topic topic)
        {
            if (topic == null)
                throw new ArgumentNullException(nameof(topic));
            _topicRepository.Update(topic);
            _eventPublisher.EntityUpdated(topic);
            //cache
            _cacheManager.RemoveByPattern(AgsTopicDefaults.TopicsPatternCacheKey);

            //event notification
        }

        #endregion
    }
}