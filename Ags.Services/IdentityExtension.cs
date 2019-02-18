using System;
using System.Linq;
using System.Security.Claims;
using Ags.Data.Core;
using Ags.Data.Domain;
using Ags.Data.Domain.Customers;
using Ags.Services.Customers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Ags.Services
{
    public static class IdentityExtension
    {
        /// <summary>
        /// //Use CustomClaimTypes when using this method
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claimType">Use [CustomClaimTypes] when using this method</param>
        /// <returns></returns>
        public static string GetUserProperty(this ClaimsPrincipal user, string claimType)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == claimType)?.Value ?? string.Empty;
            }

            return string.Empty;
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.IsInRole("Admin");
            }

            return false;
        }
        public static bool IsEditor(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.IsInRole("Editor");
            }

            return false;
        }
        public static bool IsMember(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.IsInRole("Member");
            }
            return false;
        }

        public static bool IsInCustomerRole(this Customer customer,string customerRoleSystemName)
        {
            ApplicationDbContext context = ServiceProviderFactory.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            ApplicationUser query = (from u in context.Users
                         join ur in context.UserRoles on u.Id equals ur.UserId
                         join r in context.Roles on ur.RoleId equals r.Id
                         where r.Name == customerRoleSystemName
                         select u).FirstOrDefault();

            return query != null;
        }
        public static string[] GetCustomerRoleIds(this Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            ApplicationDbContext context = ServiceProviderFactory.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            string[] customerRolesIds = (from u in context.Users
                                    join ur in context.UserRoles on u.Id equals ur.UserId
                                    join r in context.Roles on ur.RoleId equals r.Id
                                    select ur.RoleId).ToArray();

            return customerRolesIds;
        }

        public static Customer GetCustomer(this ClaimsPrincipal user,UserManager<ApplicationUser> userManager,ICustomerService customerService)
        {
            ApplicationUser appUser = userManager.FindByNameAsync(user.Identity.Name).Result;
            Customer customer = customerService.GetCustomerByAppId(appUser.Id);
            return customer;
        }
    }
}
