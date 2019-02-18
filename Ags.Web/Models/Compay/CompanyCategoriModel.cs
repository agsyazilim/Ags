using System.Collections.Generic;
using Ags.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Models.Compay
{
    public class CompanyCategoriModel:BaseAgsEntityModel
    {
        public CompanyCategoriModel()
        {
           
            AvailableCategorys = new List<SelectListItem>();
            CompanyDetailModels = new List<CompanyDetailModel>();
        }
       
        public string Name { get; set; }
        public List<SelectListItem> AvailableCategorys { get; set; }
        public AddCompanyModel AddCompanyModel { get; set; }
        public List<CompanyDetailModel> CompanyDetailModels { get; set; }
      
    }
}