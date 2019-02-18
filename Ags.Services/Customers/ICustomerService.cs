using System.Collections.Generic;
using Ags.Data.Core.Pages;
using Ags.Data.Domain;
using Ags.Data.Domain.Customers;
using Microsoft.AspNetCore.Identity;

namespace Ags.Services.Customers
{
    public partial interface ICustomerService
    {
        #region Customer
        /// <summary>
        /// GetAllCustomers
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Customer> GetAllCustomers(string email = null,int pageIndex = 0, int pageSize = int.MaxValue);
        /// <summary>
        /// DeleteCustomer
        /// </summary>
        /// <param name="customer"></param>
        void DeleteCustomer(Customer customer);
        /// <summary>
        /// GetCustomerById
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Customer GetCustomerById(int customerId);
        /// <summary>
        /// GetCustomerByIds
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        IList<Customer> GetCustomerByIds(int[] customerIds);
        /// <summary>
        /// GetCustomerByEmail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Customer GetCustomerByEmail(string email);
        /// <summary>
        /// InsertCustomer
        /// </summary>
        /// <param name="customer"></param>
        void InsertCustomer(Customer customer);
        /// <summary>
        /// UpdateCustomer
        /// </summary>
        /// <param name="customer"></param>
        void UpdateCustomer(Customer customer);
        /// <summary>
        /// GetCustomerFullName
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        string GetCustomerFullName(Customer customer);
        /// <summary>
        /// GetCustomerByAppId
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Customer GetCustomerByAppId(string ownerId);


        #endregion

        #region ApplicationUser
        /// <summary>
        /// GetAllApplicationUser
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<ApplicationUser> GetAllApplicationUser(string email = null,
            string userName = null,
            string firstName = null,int pageIndex = 0, int pageSize = int.MaxValue);
        /// <summary>
        /// DeleteApplicationUser
        /// </summary>
        /// <param name="applicationUser"></param>
        void DeleteApplicationUser(ApplicationUser applicationUser);
        /// <summary>
        /// GetApplicationUserById
        /// </summary>
        /// <param name="applicationUserId"></param>
        /// <returns></returns>
        ApplicationUser GetApplicationUserById(string applicationUserId);
        /// <summary>
        /// GetApplicationUserByEmail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        ApplicationUser GetApplicationUserByEmail(string email);
        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationUser"></param>
        void InsertApplicationUser(ApplicationUser applicationUser);
        /// <summary>
        /// UpdateApplicationUser
        /// </summary>
        /// <param name="applicationUser"></param>
        void UpdateApplicationUser(ApplicationUser applicationUser);
        /// <summary>
        /// GetApplicationUserFullName
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <returns></returns>
        string GetApplicationUserFullName(ApplicationUser applicationUser);
        /// <summary>
        /// GetRolesApplicationUsers
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        IList<Customer> GetRolesApplicationUsers(string roles);


        #endregion

        #region ApplicationRole
        /// <summary>
        /// DeleteRole
        /// </summary>
        /// <param name="role"></param>
        void DeleteRole(IdentityRole role);
        /// <summary>
        /// GetRoleById
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        IdentityRole GetRoleById(string roleId);
        /// <summary>
        /// GetRoleByIds
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        IList<IdentityRole> GetRoleByIds(int[] customerIds);
        /// <summary>
        /// GetRoleByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IdentityRole GetRoleByName(string name);
        /// <summary>
        /// InsertRole
        /// </summary>
        /// <param name="role"></param>
        void InsertRole(IdentityRole role);
        /// <summary>
        /// UpdateRole
        /// </summary>
        /// <param name="role"></param>
        void UpdateRole(IdentityRole role);
        #endregion

        #region Claim
        /// <summary>
        /// GetUserClaim
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<IdentityUserClaim<string>> GetUserClaim(string id);


        #endregion
    }
}