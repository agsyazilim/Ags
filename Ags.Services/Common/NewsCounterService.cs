using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Common;
using Ags.Services.Events;

namespace Ags.Services.Common
{
    public class NewsCounterService:INewsCounterService
    {
        private readonly IRepository<NewsCounter> _newsCounterRepository;
        private readonly IEventPublisher _eventPublisher;
        /// <summary>
        /// NewsCounterService
        /// </summary>
        /// <param name="newsCounterRepository"></param>
        public NewsCounterService(IRepository<NewsCounter> newsCounterRepository, IEventPublisher eventPublisher)
        {
            _newsCounterRepository = newsCounterRepository;
            _eventPublisher = eventPublisher;
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="newsCounter"></param>
        public void Insert(NewsCounter newsCounter)
        {
            if(newsCounter==null)
                throw new ArgumentNullException(nameof(newsCounter));
            _newsCounterRepository.Insert(newsCounter);
            _eventPublisher.EntityInserted(newsCounter);
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="newsCounter"></param>
        public void Update(NewsCounter newsCounter)
        {
            if(newsCounter==null)
                throw new ArgumentNullException(nameof(newsCounter));
            _newsCounterRepository.Update(newsCounter);
            _eventPublisher.EntityUpdated(newsCounter);

        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="newsCounter"></param>
        public void Delete(NewsCounter newsCounter)
        {
            if(newsCounter==null)
                throw new ArgumentNullException(nameof(newsCounter));
            _newsCounterRepository.Delete(newsCounter);
            _eventPublisher.EntityDeleted(newsCounter);
        }
        /// <summary>
        /// GetCounterCount
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="createDate"></param>
        /// <returns></returns>
        public int GetCounterCount(int entityId, string entityName, DateTime? createDate = null)
        {
            if(entityId==0)
                throw new ArgumentNullException(nameof(entityId));
            if(entityName==null)
                throw new ArgumentNullException(nameof(entityName));
            var result = 0;
            NewsCounter newsCounter;
            var query = _newsCounterRepository.Table;
            query = query.Where(x => x.EntityId == entityId & entityName.Contains(entityName));
            if (createDate.HasValue)
            {
                query = query.Where(x =>x.CreateDate.Day ==createDate.Value.Day);
                newsCounter = query.FirstOrDefault();
                if(newsCounter!=null)
                result = newsCounter.TotalVisitor;
            }

            newsCounter = query.FirstOrDefault();
            if (newsCounter == null)
                return result;
            result = newsCounter.TotalVisitor;
            return result;
        }
        /// <summary>
        /// GetCounterNewsCounter
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="createDate"></param>
        /// <returns></returns>
        public NewsCounter GetCounterNewsCounter(int entityId, string entityName, DateTime? createDate = null)
        {
            if (entityId == 0)
                throw new ArgumentNullException(nameof(entityId));
            if (entityName == null)
                throw new ArgumentNullException(nameof(entityName));
            NewsCounter newsCounter;
            var query = _newsCounterRepository.Table;
            query = query.Where(x => x.EntityId == entityId & entityName.Contains(entityName));
            if (createDate.HasValue)
            {
                query = query.Where(x => x.CreateDate.Day == createDate.Value.Day);
                newsCounter = query.FirstOrDefault();
                return newsCounter;
            }
            newsCounter = query.FirstOrDefault();
            if (newsCounter == null)
                return null;
            return newsCounter;
        }
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NewsCounter GetById(int id)
        {
            if(id==0)
                throw new ArgumentNullException(nameof(id));
            return _newsCounterRepository.GetById(id);
        }
        /// <summary>
        /// GetByDate
        /// </summary>
        /// <param name="createDateTime"></param>
        /// <returns></returns>
        public NewsCounter GetByDate(DateTime? createDateTime=null)
        {
            if(!createDateTime.HasValue)
                throw new ArgumentNullException(nameof(createDateTime));
            var query = _newsCounterRepository.Table.Where(x => x.CreateDate.Day == createDateTime.Value.Day);
            var result = query.FirstOrDefault();
            return result;
        }
        /// <summary>
        /// GetByListCounter
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="createDate"></param>
        /// <returns></returns>
        public List<NewsCounter> GetByListCounter(int entityId=0, string entityName=null, DateTime? createDate = null)
        {
           var query = _newsCounterRepository.Table;
            if (entityId != 0)
                query = query.Where(x => x.EntityId == entityId);
            if (entityName != null)
                query = query.Where(x => x.EntityName.Contains(entityName));
            if (createDate.HasValue)
                query = query.Where(x => x.CreateDate.Day == createDate.Value.Day);
            var result = query.ToList();
            return result;

        }
    }
}