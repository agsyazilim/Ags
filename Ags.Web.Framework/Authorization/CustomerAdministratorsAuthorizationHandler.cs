using System.Threading.Tasks;
using Ags.Data.Domain.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Ags.Web.Framework.Authorization
{
    public class CustomerAdministratorsAuthorizationHandler
    : AuthorizationHandler<OperationAuthorizationRequirement, Customer>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                  OperationAuthorizationRequirement requirement,
                                   Customer resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Administrators can do anything.
            if (context.User.IsInRole(CustomerRole.Constants.CustomerAdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}