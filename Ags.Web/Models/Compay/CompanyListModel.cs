using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Compay
{
    public class CompanyListModel:BaseAgsModel
    {


        public List<CompanyDetailModel> DetailModels { get; set; }
    }
}