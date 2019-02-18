using System.Linq;
using Ags.Services.Companys;
using Ags.Web.Factories;
using Ags.Web.Models.Compay;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    [Route("Firmalar")]
    public class CompaniesController : BasePublicController
    {
        private readonly ICompanyFactoryModel _companyFactoryModel;
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyFactoryModel companyFactoryModel, ICompanyService companyService)
        {
            _companyFactoryModel = companyFactoryModel;
            _companyService = companyService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        [HttpGet("Kategori")]
        public IActionResult List()
        {
            var category = _companyService.GetCompayCategoryList().FirstOrDefault();
            var model = _companyFactoryModel.PrepareCompanyCategoriModel(category.Id);
            return View(model);
        }
        [HttpGet("Kategori/{id}")]
        public IActionResult List(int id)
        {
             var model = _companyFactoryModel.PrepareCompanyCategoriModel(id);

            return View(model);
        }

        [HttpGet("FirmaDetay/{id}")]
        public IActionResult Details(int id)
        {
            var company = _companyService.GetByCompanyId(id);
            var model = _companyFactoryModel.PrePareCompanyDetailModel(new CompanyDetailModel(), company);

            return View(model);
        }
        [HttpPost]

        public IActionResult AddCompany(AddCompanyModel model)
        {




            return RedirectToAction("List");
        }
    }
}