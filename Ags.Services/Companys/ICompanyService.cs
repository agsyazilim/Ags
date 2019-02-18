using System;
using System.Collections.Generic;
using Ags.Data.Domain.Catalog;

namespace Ags.Services.Companys
{
    public interface ICompanyService
    {
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="company"></param>
        void Insert(Company company);
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="company"></param>
        void Update(Company company);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="company"></param>
        void Delete(Company company);
        /// <summary>
        /// GetByCompanyId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Company GetByCompanyId(int id);
        /// <summary>
        /// GetCompayList
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="createTo"></param>
        /// <param name="startTo"></param>
        /// <param name="endTo"></param>
        /// <returns></returns>
        List<Company> GetCompayList(int categoryId = 0, DateTime? createTo = null, DateTime? startTo = null,
            DateTime? endTo = null);
        /// <summary>
        /// InsertCategory
        /// </summary>
        /// <param name="companyCategory"></param>
        void InsertCategory(CompanyCategory companyCategory);
        /// <summary>
        /// UpdateCategory
        /// </summary>
        /// <param name="companyCategory"></param>
        void UpdateCategory(CompanyCategory companyCategory);
        /// <summary>
        /// DeleteCategory
        /// </summary>
        /// <param name="companyCategory"></param>
        void DeleteCategory(CompanyCategory companyCategory);
        /// <summary>
        /// GetByCategoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        CompanyCategory GetByCategoryId(int categoryId);
        /// <summary>
        /// GetCompayCategoryList
        /// </summary>
        /// <returns></returns>
        List<CompanyCategory> GetCompayCategoryList();


    }
}