using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Common;
using Ags.Services.Events;

namespace Ags.Services.Common
{
    public class SectionService:ISectionService
    {
        private readonly IRepository<Section> _sectionRepository;
        private readonly IEventPublisher _eventPublisher;

        public SectionService(IRepository<Section> sectionRepository, IEventPublisher eventPublisher)
        {
            _sectionRepository = sectionRepository;
            _eventPublisher = eventPublisher;
        }
        /// <summary>
        /// GetAllSection
        /// </summary>
        /// <returns></returns>
        public IList<Section> GetAllSection()
        {
            IQueryable<Section> query = _sectionRepository.Table;

            List<Section> sectionList = query.ToList();
            return sectionList;

        }
        /// <summary>
        /// GetDescription
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public string GetDescription(string sectionName)
        {
            IQueryable<Section> query = _sectionRepository.Table;
            string sonuc = query.FirstOrDefault(x => x.Name.Contains(sectionName))?.Decription;
            return sonuc;

        }
        /// <summary>
        /// GetNewsSection
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public string[] GetNewsSection(string sectionName)
        {
            if (sectionName == null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }
            IQueryable<Section> query = _sectionRepository.Table;
            List<Section> filter = (from n in query
                where n.Name.Contains(sectionName)
                select n).ToList();

            string des = filter[0].Decription;
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] list = des.Split(delimiterChars);
            return list;
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="section"></param>
        public void Insert(Section section)
        {
            if(section==null)
                throw new ArgumentNullException(nameof(section));
            _sectionRepository.Insert(section);
            _eventPublisher.EntityInserted(section);
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="section"></param>
        public void Update(Section section)
        {
            if(section==null)
                throw new ArgumentNullException(nameof(section));
            _sectionRepository.Update(section);
            _eventPublisher.EntityUpdated(section);
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="section"></param>
        public void Delete(Section section)
        {
            if(section==null)
                throw new ArgumentNullException(nameof(section));
            _sectionRepository.Delete(section);
            _eventPublisher.EntityDeleted(section);
        }
        /// <summary>
        /// GetBySectionId
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public Section GetBySectionId(int sectionId)
        {
            if (sectionId == 0)
                return null;
            return _sectionRepository.GetById(sectionId);
        }

        public Section GetByName(string name)
        {
            var query = _sectionRepository.Table;
            query = query.Where(x => x.Name.Contains(name));
            var result = query.FirstOrDefault();
            return result;
        }
    }
}