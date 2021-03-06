﻿using System.Linq;
using Ags.Services.Catalog;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class MainPageDownViewComponent : AgsViewComponent
    {

        private readonly ICategoryService _categoryService;
        private readonly ICatalogModelFactory _catalogModelFactory;

        public MainPageDownViewComponent(
            ICategoryService categoryService,
             ICatalogModelFactory catalogModelFactory)
        {
            _categoryService = categoryService;
            _catalogModelFactory = catalogModelFactory;
        }

        public IViewComponentResult Invoke(string name)
        {
            var category = _categoryService.GetAllCategories(categoryName: name).FirstOrDefault();
            var model = _catalogModelFactory.PrepareMainPageDownModel(category);
            return View(model);
        }
    }
}