using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Catalog;
using Ags.Services.Events;

namespace Ags.Services.Companys
{
    public class CompanyService:ICompanyService
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<CompanyCategory> _companyCategoryRepository;
        private readonly IEventPublisher _eventPublisher;
        public CompanyService(IRepository<Company> companyRepository, IRepository<CompanyCategory> companyCategory, IEventPublisher eventPublisher)
        {
            _companyRepository = companyRepository;
            _companyCategoryRepository = companyCategory;
            _eventPublisher = eventPublisher;
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="company"></param>
        public void Insert(Company company)
        {
            if(company==null)
                throw new ArgumentNullException(nameof(company));
            _companyRepository.Insert(company);
            _eventPublisher.EntityInserted(company);
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="company"></param>
        public void Update(Company company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));
            _companyRepository.Update(company);
            _eventPublisher.EntityUpdated(company);
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="company"></param>
        public void Delete(Company company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));
            _companyRepository.Delete(company);
            _eventPublisher.EntityDeleted(company);
        }
        /// <summary>
        /// GetByCompanyId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Company GetByCompanyId(int id)
        {
            if (id == 0)
                return null;
            return _companyRepository.GetById(id);
        }
        /// <summary>
        /// GetCompayList
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="createTo"></param>
        /// <param name="startTo"></param>
        /// <param name="endTo"></param>
        /// <returns></returns>
        public List<Company> GetCompayList(int categoryId = 0, DateTime? createTo = null, DateTime? startTo = null, DateTime? endTo = null)
        {
            var query = _companyRepository.Table;
            if (categoryId > 0)
                query = query.Where(x => x.CompanyCategoryId == categoryId);
            if (startTo.HasValue)
                query = query.Where(x => startTo.Value <= (x.StartDate));
            if (endTo.HasValue)
                query = query.Where(x => endTo.Value <= (x.EndDate));
            var companyLst = query.ToList();
            return companyLst;
        }
        /// <summary>
        /// InsertCategory
        /// </summary>
        /// <param name="companyCategory"></param>
        public void InsertCategory(CompanyCategory companyCategory)
        {
            if(companyCategory==null)
                throw new ArgumentNullException(nameof(companyCategory));
            _companyCategoryRepository.Insert(companyCategory);
        }
        /// <summary>
        /// UpdateCategory
        /// </summary>
        /// <param name="companyCategory"></param>
        public void UpdateCategory(CompanyCategory companyCategory)
        {
            if (companyCategory == null)
                throw new ArgumentNullException(nameof(companyCategory));
            _companyCategoryRepository.Update(companyCategory);
        }
        /// <summary>
        /// DeleteCategory
        /// </summary>
        /// <param name="companyCategory"></param>
        public void DeleteCategory(CompanyCategory companyCategory)
        {
            if (companyCategory == null)
                throw new ArgumentNullException(nameof(companyCategory));
            _companyCategoryRepository.Delete(companyCategory);
        }
        /// <summary>
        /// GetByCategoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public CompanyCategory GetByCategoryId(int categoryId)
        {
            if (categoryId == 0)
                return null;
            return _companyCategoryRepository.GetById(categoryId);
        }
        /// <summary>
        /// GetCompayCategoryList
        /// </summary>
        /// <returns></returns>
        public List<CompanyCategory> GetCompayCategoryList()
        {
            var query = _companyCategoryRepository.Table;
            var list = query.ToList();
            return list;

        }
    }
}