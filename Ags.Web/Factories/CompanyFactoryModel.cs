using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Domain.Catalog;
using Ags.Services.Companys;
using Ags.Services.Directory;
using Ags.Services.Media;
using Ags.Services.Seo;
using Ags.Web.Models.Compay;

namespace Ags.Web.Factories
{
    public class CompanyFactoryModel:ICompanyFactoryModel
    {
        private readonly ICompanyService _companyService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IPictureService _pictureService;
        private readonly IUrlRecordService _urlRecordService;
      

        public CompanyFactoryModel(ICompanyService companyService, IStateProvinceService stateProvinceService, IPictureService pictureService, IUrlRecordService urlRecordService)
        {
            _companyService = companyService;
            _stateProvinceService = stateProvinceService;
            _pictureService = pictureService;
            _urlRecordService = urlRecordService;
        }

        public CompanyDetailModel PrePareCompanyDetailModel(CompanyDetailModel model,Company company)
        {
            if (company == null)
                return null;
            model.Name = company.Name;
            model.Address = company.Address;
            model.City = _stateProvinceService.GetStateProvinceById(company.StateProvincesId).Name;
            model.Description = company.Description;
            model.Gsm = company.Gsm;
            model.Phone = company.Phone;
            model.Url = _urlRecordService.GetSeName(company);
            model.Www = company.Www;
            model.Fax = company.Fax;
            model.VideoEmbedCode = company.VideoEmbedCode;
            model.PictureUrl = _pictureService.GetPictureUrl(company.PictureId, 400);
            model.CategoryName = _companyService.GetByCategoryId(company.CompanyCategoryId).Name;
            model.Id = company.Id;
            model.StartDate = company.StartDate;

            return model;
        }

        public List<CompanyDetailModel> PrePareCompanyDetailModels()
        {
            var query = _companyService.GetCompayList(endTo: DateTime.Now);
            
            var model = new List<CompanyDetailModel>();
            foreach (var company in query)
            {
              model.Add(PrePareCompanyDetailModel(new CompanyDetailModel(), company));    
            }

            return model;

        }

        public CompanyListModel PrePareCompanyListModel(CompanyCategory category)
        {
            throw new System.NotImplementedException();
        }


        public CompanyListModel PrePareCompanyListModel()
        {
            var model = new CompanyListModel {DetailModels = PrePareCompanyDetailModels()};
            return model;
        }

        public CompanyListModel PrePareCompanyListModel(int categoryId=0)
        {
            throw new System.NotImplementedException();
        }

        public List<CompanyDetailModel> PrepareCompanyDetailModel(int categoriId)
        {
            throw new System.NotImplementedException();
        }


        public List<CompanyDetailModel> PrepareCompanyDetailListModel(int categoriId)
        {
            if (categoriId == 0)
                return null;
            var query = _companyService.GetCompayList(categoryId: categoriId);
            var model = new List<CompanyDetailModel>();
            foreach (var company in query)
            {
                model.Add(PrePareCompanyDetailModel(new CompanyDetailModel(), company));
            }

            return model;
        }

        public List<CompanyCategoriModel> PrePareCompanyCategoriListModels()
        {
            var query = _companyService.GetCompayCategoryList();
            var model = query.Select(x => new CompanyCategoriModel
            {
                Id = x.Id,
                Name = x.Name,
                CompanyDetailModels = PrepareCompanyDetailListModel(x.Id)

            }).ToList();
            return model;

        }

        public CompanyCategoriModel PrepareCompanyCategoriModel(int categoryId = 0)
        {
            var  model = new CompanyCategoriModel();
            var category = _companyService.GetByCategoryId(categoryId);
            model.CompanyDetailModels = PrepareCompanyDetailListModel(categoryId);
            model.Name = category.Name;
            model.Id = category.Id;
            return model;
        }
    }
}