using Ags.Services;
using Ags.Services.Customers;
using Ags.Services.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class SecurityController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public SecurityController(ICustomerService customerService,
            ILogger logger)
        {
            this._customerService = customerService;
            this._logger = logger;
        }

        #endregion

        #region Methods

        public virtual IActionResult AccessDenied(string pageUrl)
        {
           var customer = _customerService.GetCustomerByEmail(User.Identity.Name);
            if (customer==null || User.IsMember())
            {
                _logger.Information($"Access denied to anonymous request on {pageUrl}");
                return View();
            }

            _logger.Information($"Access denied to user #{customer.Email} '{customer.Email}' on {pageUrl}");

            return View();
        }
        #endregion
    }
}