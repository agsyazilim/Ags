using System.Collections.Generic;
using System.Linq;
using Ags.Data.Domain;
using Ags.Services.Catalog;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class MainPageViewComponent : AgsViewComponent
    {

        private readonly ICategoryService _categoryService;
        private readonly ICatalogModelFactory _catalogModelFactory;
        private readonly ApplicationDbContext _context;

        public MainPageViewComponent( ICategoryService categoryService, ICatalogModelFactory catalogModelFactory, ApplicationDbContext context)
        {
            _categoryService = categoryService;
            _catalogModelFactory = catalogModelFactory;
            _context = context;
        }

        public IViewComponentResult Invoke(string[] name, string title)
        {
            var categorys = _categoryService.GetAllCategoriesDisplayedOnHomePage();
            var categoryList = categorys.Where(x => name.Contains(x.Name)&x.Published&!x.Deleted).ToList();
            var model = new List<CategoriModel>();
            foreach (var category in categoryList)
            {
               model.Add(_catalogModelFactory.PrepareCategoryModel(new CategoriModel(), category));
            }
            return View(model);
        }



    }
}