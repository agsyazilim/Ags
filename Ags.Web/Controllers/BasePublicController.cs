using Ags.Web.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public abstract class BasePublicController : BaseController
    {
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }
    }
}