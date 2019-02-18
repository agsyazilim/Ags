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


        public MainPageViewComponent( ICategoryService categoryService, ICatalogModelFactory catalogModelFactory)
        {
            _categoryService = categoryService;
            _catalogModelFactory = catalogModelFactory;

        }

        public IViewComponentResult Invoke(string[] name, string title)
        {
            var categorys = _categoryService.GetAllCategoriesDisplayedOnHomePage();
            var categoryList = categorys.Where(x => name.Contains(x.Name)&x.Published&!x.Deleted).ToList();
            var model = _catalogModelFactory.PrepareMainPageModel(categoryList);
          
            return View(model);
        }



    }
}