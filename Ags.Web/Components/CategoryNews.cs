using System.Linq;
using Ags.Data.Domain;
using Ags.Services.Catalog;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class CategoryNewsViewComponent : AgsViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly ICatalogModelFactory _catalogModelFactory;
        private readonly ApplicationDbContext _context;

        public CategoryNewsViewComponent(
            ICategoryService categoryService, 
            ICatalogModelFactory catalogModelFactory, ApplicationDbContext context)
        {
            _categoryService = categoryService;
            _catalogModelFactory = catalogModelFactory;
            _context = context;
        }

        public IViewComponentResult Invoke(string name)
        {

            var category = _categoryService.GetAllCategories(categoryName: name).FirstOrDefault();
            var model = _catalogModelFactory.PrepareCategoryModel(new CategoriModel(), category);
           
            return View(model);
        }
    }
}