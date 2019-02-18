using System.Collections.Generic;
using Ags.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Models.Common
{
    public class StateProvidenceModel:BaseAgsModel
    {

        public List<SelectListItem> StateList { get; set; }
        public int SelectStateId { get; set; }

    }
}