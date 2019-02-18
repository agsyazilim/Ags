using System.Threading.Tasks;
using Ags.Data.Domain.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Ags.Web.Framework.Authorization
{
    public class CustomerManagerAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, Customer>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, Customer resource)
        {

            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.Name != CustomerRole.Constants.ApproveOperationName &&
                requirement.Name != CustomerRole.Constants.RejectOperationName)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(CustomerRole.Constants.CustomerManagersRole))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}