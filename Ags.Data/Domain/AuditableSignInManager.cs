using System;
using System.Linq;
using System.Threading.Tasks;
using Ags.Data.Domain.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ags.Data.Domain
{
    /// <summary>
    /// AuditableSignInManager
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class AuditableSignInManager<TUser> : SignInManager<TUser> where TUser : class
    {
        private readonly UserManager<TUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// AuditableSignInManager
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="claimsFactory"></param>
        /// <param name="optionsAccessor"></param>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuditableSignInManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, ApplicationDbContext dbContext)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, null)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (contextAccessor == null)
            {
                throw new ArgumentNullException(nameof(contextAccessor));
            }

            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _db = dbContext;
        }



        /// <summary>
        /// PasswordSignInAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <param name="lockoutOnFailure"></param>
        /// <returns></returns>
        public override async Task<SignInResult> PasswordSignInAsync(TUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            SignInResult result = await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure).ConfigureAwait(false);

            ApplicationUser appUser = user as ApplicationUser;

            if (appUser != null) // We can only log an audit record if we can access the user object and it's ID
            {
                string ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                UserAudit auditRecord = null;
                Customer customer = null;
                switch (result.ToString())
                {
                    case "Succeeded":
                        auditRecord = UserAudit.CreateAuditEvent(appUser.Id, UserAuditEventType.Login, ip);
                        if (!_db.Customers.Any(x => x.OwnerId.Contains(appUser.Id)))
                        customer = Customer.CreateCustomerEvent(ContactStatus.Rejected, appUser.FirstName + " " + appUser.LastName, appUser.Id, appUser.Email,appUser.AvatarURL);
                        break;

                    case "Failed":
                        auditRecord = UserAudit.CreateAuditEvent(appUser.Id, UserAuditEventType.FailedLogin, ip);
                        break;
                }

                if (auditRecord != null)
                {
                    _db.UserAuditEvents.Add(auditRecord);
                     await _db.SaveChangesAsync().ConfigureAwait(false);

                }

                if (customer != null)
                {
                    _db.Customers.Add(customer);
                    await _db.SaveChangesAsync().ConfigureAwait(false);
                }
            }


            return result;
        }

        /// <summary>
        /// SignOutAsync
        /// </summary>
        /// <returns></returns>
        public override async Task SignOutAsync()
        {
            await base.SignOutAsync().ConfigureAwait(false);

            IdentityUser user = await _userManager.FindByIdAsync(_userManager.GetUserId(_contextAccessor.HttpContext.User)).ConfigureAwait(false) as IdentityUser;

            if (user != null)
            {
                string ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                UserAudit auditRecord = UserAudit.CreateAuditEvent(user.Id, UserAuditEventType.LogOut, ip);
                _db.UserAuditEvents.Add(auditRecord);
                await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
