﻿using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class FooterSiteViewComponent: AgsViewComponent
    {
        public IViewComponentResult Invoke()
        {

             return View(null);
        }
    }
}