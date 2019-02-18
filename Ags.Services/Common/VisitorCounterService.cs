using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Common;
using Ags.Services.Events;

namespace Ags.Services.Common
{
    public class VisitorCounterService:IVisitorCounterService
    {
        private readonly IRepository<VisitorCounter> _vistorCounteRepository;
        private readonly IEventPublisher _eventPublisher;

        public VisitorCounterService(IRepository<VisitorCounter> vistorCounteRepository, IEventPublisher eventPublisher)
        {
            _vistorCounteRepository = vistorCounteRepository;
            _eventPublisher = eventPublisher;
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="visitorCounter"></param>
        public void Insert(VisitorCounter visitorCounter)
        {
            if(visitorCounter==null)
                throw new ArgumentNullException(nameof(visitorCounter));
            _vistorCounteRepository.Insert(visitorCounter);
            _eventPublisher.EntityInserted(visitorCounter);
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="visitorCounter"></param>
        public void Update(VisitorCounter visitorCounter)
        {
            if (visitorCounter == null)
                throw new ArgumentNullException(nameof(visitorCounter));
            _vistorCounteRepository.Update(visitorCounter);
            _eventPublisher.EntityUpdated(visitorCounter);
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="visitorCounter"></param>
        public void Delete(VisitorCounter visitorCounter)
        {
            if (visitorCounter == null)
                throw new ArgumentNullException(nameof(visitorCounter));
            _vistorCounteRepository.Delete(visitorCounter);
            _eventPublisher.EntityDeleted(visitorCounter);
        }

        public int GetCounterCount(DateTime? createDate = null)
        {
            if(!createDate.HasValue)
                throw new ArgumentNullException(nameof(createDate));
            var query = _vistorCounteRepository.Table;
            query = query.Where(x => x.CreateDate.Day == createDate.Value.Day);
            var result = query.FirstOrDefault();
            int sonuc = 0;
            if (result != null)
            {
                sonuc= result.Count;
            }

            return sonuc;
        }

        public VisitorCounter GetCounterNewsCounter(DateTime? createDate = null)
        {
            if (!createDate.HasValue)
                throw new ArgumentNullException(nameof(createDate));
            var query = _vistorCounteRepository.Table;
            query = query.Where(x => x.CreateDate.Day == createDate.Value.Day);
            var result = query.FirstOrDefault();
           return result;
        }

        public VisitorCounter GetById(int id)
        {
            if(id==0)
                throw new ArgumentNullException(nameof(id));
            return _vistorCounteRepository.GetById(id);
        }

        public VisitorCounter GetByDate(DateTime? createDateTime = null)
        {
            if (!createDateTime.HasValue)
                throw new ArgumentNullException(nameof(createDateTime));
            var query = _vistorCounteRepository.Table;
            query = query.Where(x => x.CreateDate.Day == createDateTime.Value.Day);
            var result = query.FirstOrDefault();
            return result ?? null;
        }

        public List<VisitorCounter> GetByListCounter()
        {
            IQueryable<VisitorCounter> query = _vistorCounteRepository.Table;
            return query.ToList();
        }
    }
}