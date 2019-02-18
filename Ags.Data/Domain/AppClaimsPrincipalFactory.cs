using System.Security.Claims;
using System.Threading.Tasks;
using Ags.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Ags.Data.Domain
{
    /// <summary>
    /// AppClaimsPrincipalFactory
    /// </summary>
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        /// <summary>
        /// AppClaimsPrincipalFactory
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="optionsAccessor"></param>
        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
        }

        /// <summary>
        /// CreateAsync
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            ClaimsPrincipal principal = await base.CreateAsync(user);
            ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim(CustomClaimTypes.Name, user.FirstName),
                new Claim(CustomClaimTypes.Surname, user.LastName),
                new Claim(CustomClaimTypes.PictureId, user.AvatarURL),
                new Claim(CustomClaimTypes.Position, user.Position),
                new Claim(CustomClaimTypes.NickName, user.NickName),
                new Claim(CustomClaimTypes.DateRegistered, user.DateRegistered),


            });
            return principal;
        }

    }
}