using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class CompanyViewComponent : AgsViewComponent
    {

        private readonly ICompanyFactoryModel _companyFactoryModel;


        public CompanyViewComponent(ICompanyFactoryModel companyFactoryModel)
        {
            _companyFactoryModel = companyFactoryModel;
        }

        public IViewComponentResult Invoke()
        {
            var model = _companyFactoryModel.PrePareCompanyDetailModels();

            return View(model);
        }
    }
}
