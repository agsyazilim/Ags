using System;
using Ags.Data.Domain;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class CopyRightViewComponent:AgsViewComponent
    {
        private readonly StoreInformationSettings _storeInformationSettings;

        public CopyRightViewComponent(StoreInformationSettings storeInformationSettings)
        {
            _storeInformationSettings = storeInformationSettings;
        }

        public IViewComponentResult Invoke()
        {
            string copyrigt = _storeInformationSettings.CopyRigth;
            Tuple<string> copp =  Tuple.Create(copyrigt);
             return View(copp);
        }
    }
}