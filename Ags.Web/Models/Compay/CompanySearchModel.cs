using System.Collections.Generic;
using Ags.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Models.Compay
{
    public class CompanySearchModel:BaseSearchModel
    {
        public CompanySearchModel()
        {
            AvailableCategorys = new List<SelectListItem>();
        }
        public string FirmaAdı { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> AvailableCategorys { get; set; }
    }
}