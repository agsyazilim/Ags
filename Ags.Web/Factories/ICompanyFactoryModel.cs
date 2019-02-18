using System.Collections.Generic;
using Ags.Data.Domain.Catalog;
using Ags.Web.Models.Compay;

namespace Ags.Web.Factories
{
    public interface ICompanyFactoryModel
    {
        CompanyDetailModel PrePareCompanyDetailModel(CompanyDetailModel model,Company company);
        List<CompanyDetailModel> PrePareCompanyDetailModels();
        CompanyListModel PrePareCompanyListModel();
        CompanyListModel PrePareCompanyListModel(int categoryId=0);
        List<CompanyDetailModel> PrepareCompanyDetailModel(int categoriId);
        List<CompanyCategoriModel> PrePareCompanyCategoriListModels();
        CompanyCategoriModel PrepareCompanyCategoriModel(int categoryId=0);


    }
}