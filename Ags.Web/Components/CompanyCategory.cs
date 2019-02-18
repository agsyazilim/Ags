using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class CompanyCategoryViewComponent : AgsViewComponent
    {
        private readonly ICompanyFactoryModel _companyFactoryModel;


        public CompanyCategoryViewComponent(ICompanyFactoryModel companyFactoryModel)
        {
            _companyFactoryModel = companyFactoryModel;
        }

        public IViewComponentResult Invoke()
        {
            var model = _companyFactoryModel.PrePareCompanyCategoriListModels();

            return View(model);
        }
    }
}
