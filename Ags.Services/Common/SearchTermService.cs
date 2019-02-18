using System;
using System.Linq;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Common;
using Ags.Services.Events;

namespace Ags.Services.Common
{
    /// <summary>
    /// Search term service
    /// </summary>
    public partial class SearchTermService : ISearchTermService
    {
        #region Fields

        private readonly IRepository<SearchTerm> _searchTermRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public SearchTermService(
            IRepository<SearchTerm> searchTermRepository, IEventPublisher eventPublisher)
        {
            this._searchTermRepository = searchTermRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a search term record
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        public virtual void DeleteSearchTerm(SearchTerm searchTerm)
        {
            if (searchTerm == null)
                throw new ArgumentNullException(nameof(searchTerm));

            _searchTermRepository.Delete(searchTerm);
            _eventPublisher.EntityDeleted(searchTerm);

        }

        /// <summary>
        /// Gets a search term record by identifier
        /// </summary>
        /// <param name="searchTermId">Search term identifier</param>
        /// <returns>Search term</returns>
        public virtual SearchTerm GetSearchTermById(int searchTermId)
        {
            if (searchTermId == 0)
                return null;

            return _searchTermRepository.GetById(searchTermId);

        }

        /// <summary>
        /// Gets a search term record by keyword
        /// </summary>
        /// <param name="keyword">Search term keyword</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Search term</returns>
        public virtual SearchTerm GetSearchTermByKeyword(string keyword, int storeId)
        {
            if (string.IsNullOrEmpty(keyword))
                return null;

            IOrderedQueryable<SearchTerm> query = from st in _searchTermRepository.Table
                        where st.Keyword == keyword
                        orderby st.Id
                        select st;
            SearchTerm searchTerm = query.FirstOrDefault();
            return searchTerm;
        }

        /// <summary>
        /// Gets a search term statistics
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>A list search term report lines</returns>
        public virtual IPagedList<SearchTermReportLine> GetStats(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            IQueryable<SearchTermReportLine> query = (from st in _searchTermRepository.Table
                         group st by st.Keyword into groupedResult
                         select new
                         {
                             Keyword = groupedResult.Key,
                             Count = groupedResult.Sum(o => o.Count)
                         })
                        .OrderByDescending(m => m.Count)
                        .Select(r => new SearchTermReportLine
                        {
                            Keyword = r.Keyword,
                            Count = r.Count
                        });

            PagedList<SearchTermReportLine> result = new PagedList<SearchTermReportLine>(query, pageIndex, pageSize);
            return result;
        }

        /// <summary>
        /// Inserts a search term record
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        public virtual void InsertSearchTerm(SearchTerm searchTerm)
        {
            if (searchTerm == null)
                throw new ArgumentNullException(nameof(searchTerm));

            _searchTermRepository.Insert(searchTerm);
            _eventPublisher.EntityInserted(searchTerm);


        }

        /// <summary>
        /// Updates the search term record
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        public virtual void UpdateSearchTerm(SearchTerm searchTerm)
        {
            if (searchTerm == null)
                throw new ArgumentNullException(nameof(searchTerm));

            _searchTermRepository.Update(searchTerm);
            _eventPublisher.EntityUpdated(searchTerm);
        }

        #endregion
    }
}