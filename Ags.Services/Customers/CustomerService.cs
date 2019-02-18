using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain;
using Ags.Data.Domain.Customers;
using Ags.Services.Events;
using Microsoft.AspNetCore.Identity;

namespace Ags.Services.Customers
{
    public partial class CustomerService : ICustomerService
    {
        #region Field

        private readonly IRepository<Customer> _customerRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _identityRoleManager;
        private readonly IEventPublisher _eventPublisher;
        #endregion

        #region Ctor

        public CustomerService(IRepository<Customer> customerRepository,ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> identityRoleManager, IEventPublisher eventPublisher)
        {
            this._customerRepository = customerRepository;
            this._applicationDbContext = applicationDbContext;
            this._userManager = userManager;
            this._identityRoleManager = identityRoleManager;
            this._eventPublisher = eventPublisher;
        }
        #endregion

        #region Customers
        /// <summary>
        /// GetAllCustomers
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IPagedList<Customer> GetAllCustomers(string email = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            IQueryable<Customer> query = _customerRepository.Table;
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(x => x.Email == email);
            }

            query = query.OrderByDescending(c => c.Name);
            PagedList<Customer> customers = new PagedList<Customer>(query, pageIndex, pageSize);
            return customers;

        }
        /// <summary>
        /// DeleteCustomer
        /// </summary>
        /// <param name="customer"></param>
        public virtual void DeleteCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }
            _customerRepository.Delete(customer);
            _eventPublisher.EntityDeleted(customer);
        }
        /// <summary>
        /// GetCustomerById
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public virtual Customer GetCustomerById(int customerId)
        {
            if (customerId == 0)
            {
                return null;
            }
            return _customerRepository.GetById(customerId);
        }
        /// <summary>
        /// GetCustomerByIds
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        public virtual IList<Customer> GetCustomerByIds(int[] customerIds)
        {
            if (customerIds == null || customerIds.Length == 0)
            {
                return new List<Customer>();
            }

            IQueryable<Customer> query = from c in _customerRepository.Table
                        where customerIds.Contains(c.Id)
                        select c;
            List<Customer> customers = query.ToList();
            List<Customer> sortedCustomer = new List<Customer>();
            foreach (int id in customerIds)
            {
                Customer customer = customers.Find(x => x.Id == id);
                if (customer != null)
                {
                    sortedCustomer.Add(customer);
                }
            }

            return sortedCustomer;
        }
        /// <summary>
        /// GetCustomerByEmail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public virtual Customer GetCustomerByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            IQueryable<Customer> query = from c in _customerRepository.Table
                        where c.Email == email
                        select c;

            Customer customers = query.FirstOrDefault();
            return customers;

        }
        /// <summary>
        /// InsertCustomer
        /// </summary>
        /// <param name="customer"></param>
        public virtual void InsertCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }
            _customerRepository.Insert(customer);
            _eventPublisher.EntityInserted(customer);
        }
        /// <summary>
        /// UpdateCustomer
        /// </summary>
        /// <param name="customer"></param>
        public virtual void UpdateCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            _customerRepository.Update(customer);
            _eventPublisher.EntityUpdated(customer);
        }
        /// <summary>
        /// GetCustomerFullName
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public virtual string GetCustomerFullName(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            ApplicationUser customers = _userManager.FindByIdAsync(customer.OwnerId).Result;
            string fullName = customers.FirstName + " " + customers.LastName;
            return fullName;
        }
        /// <summary>
        /// GetCustomerByAppId
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public virtual Customer GetCustomerByAppId(string ownerId)
        {
            IQueryable<Customer> query = _customerRepository.Table;
            Customer customer = query.FirstOrDefault(x => x.OwnerId == ownerId);
            return customer;

        }

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
        public virtual IPagedList<ApplicationUser> GetAllApplicationUser(string email = null,
            string userName = null,
            string firstName = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            IQueryable<ApplicationUser> query = _userManager.Users;
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(x => x.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(x => x.UserName.Contains(userName));
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(x => x.FirstName.Contains(firstName));
            }

            query = query.Where(x => !x.UserName.Contains("omer@gmail.com"));
            List<ApplicationUser> appUser = query.ToList();
            PagedList<ApplicationUser> user = new PagedList<ApplicationUser>(appUser, pageIndex, pageSize);
            return user;
        }
        /// <summary>
        /// DeleteApplicationUser
        /// </summary>
        /// <param name="applicationUser"></param>
        public virtual void DeleteApplicationUser(ApplicationUser applicationUser)
        {
            if (applicationUser == null)
            {
                throw new ArgumentNullException(nameof(applicationUser));
            }
            _userManager.DeleteAsync(applicationUser);

        }
        /// <summary>
        /// GetApplicationUserById
        /// </summary>
        /// <param name="applicationUserId"></param>
        /// <returns></returns>
        public virtual ApplicationUser GetApplicationUserById(string applicationUserId)
        {
            if (string.IsNullOrEmpty(applicationUserId))
            {
                return null;
            }

            var appUser = _applicationDbContext.Users.Find(applicationUserId);
            if (appUser == null)
                return null;
            return appUser;
        }
        /// <summary>
        /// GetApplicationUserByEmail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public virtual ApplicationUser GetApplicationUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            return _userManager.FindByEmailAsync(email).Result;
        }
        /// <summary>
        /// InsertApplicationUser
        /// </summary>
        /// <param name="applicationUser"></param>
        public virtual void InsertApplicationUser(ApplicationUser applicationUser)
        {
            if (applicationUser == null)
            {
                throw new ArgumentNullException(nameof(applicationUser));
            }

            IdentityResult user = _userManager.CreateAsync(applicationUser).Result;

        }
        /// <summary>
        /// UpdateApplicationUser
        /// </summary>
        /// <param name="applicationUser"></param>
        public virtual void UpdateApplicationUser(ApplicationUser applicationUser)
        {
            if (applicationUser == null)
            {
                throw new ArgumentNullException(nameof(applicationUser));
            }

            IdentityResult user = _userManager.UpdateAsync(applicationUser).Result;


        }
        /// <summary>
        /// GetApplicationUserFullName
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <returns></returns>
        public virtual string GetApplicationUserFullName(ApplicationUser applicationUser)
        {
            if (applicationUser == null)
            {
                throw new ArgumentNullException(nameof(applicationUser));
            }

            ApplicationUser user = _userManager.FindByEmailAsync(applicationUser.Email).Result;
            string nameSurName = user.FirstName + " " + user.LastName;
            return nameSurName;
        }
        /// <summary>
        /// GetRolesApplicationUsers
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public IList<Customer> GetRolesApplicationUsers(string roles)
        {

            List<Customer> userRolesList = (from c in _applicationDbContext.Customers
                                 join u in _applicationDbContext.Users on c.OwnerId equals u.Id
                                 join ur in _applicationDbContext.UserRoles on u.Id equals ur.UserId
                                 join r in _applicationDbContext.Roles on ur.RoleId equals r.Id
                                 where r.Name == roles
                                 select c).ToList();
            return userRolesList;
        }

        #endregion

        #region Role

        /// <summary>
        /// DeleteRole
        /// </summary>
        /// <param name="role"></param>
        public virtual void DeleteRole(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            _identityRoleManager.DeleteAsync(role);


        }
        /// <summary>
        /// GetRoleById
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual IdentityRole GetRoleById(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                throw new ArgumentNullException(nameof(roleId));
            }

            return _identityRoleManager.FindByIdAsync(roleId).Result;
        }
        /// <summary>
        /// GetRoleByIds
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        public virtual IList<IdentityRole> GetRoleByIds(int[] customerIds)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// GetRoleByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IdentityRole GetRoleByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _identityRoleManager.FindByNameAsync(name).Result;
        }
        /// <summary>
        /// InsertRole
        /// </summary>
        /// <param name="role"></param>
        public virtual void InsertRole(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            IdentityResult roles = _identityRoleManager.CreateAsync(role).Result;

        }
        /// <summary>
        /// UpdateRole
        /// </summary>
        /// <param name="role"></param>
        public virtual void UpdateRole(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            IdentityResult roles = _identityRoleManager.UpdateAsync(role).Result;

        }
        /// <summary>
        /// GetUserClaim
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<IdentityUserClaim<string>> GetUserClaim(string id)
        {
          var claims =  _applicationDbContext.UserClaims.Where(x => x.UserId.Contains(id)).ToList();
          return claims;
        }

        #endregion

    }
}