using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ags.Data.Common;
using Ags.Data.Core;
using Ags.Data.Domain;
using Ags.Data.Domain.Customers;
using Ags.Data.Domain.Media;
using Ags.Services.Customers;
using Ags.Services.Media;
using Ags.Web.Areas.Admin.Models.Customers;
using Ags.Web.Framework.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class CustomerController : BaseAdminController
    {
        #region Ctor

      
        public CustomerController(
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context, RoleManager<IdentityRole> roleManager, 
            ICustomerService customerService, 
            IPictureService pictureService, StoreInformationSettings storeInformationSettings, IWebHelper webHelper)
        {

            _authorizationService = authorizationService;
            _userManager = userManager;
            _context = context;
            this._roleManager = roleManager;
            _customerService = customerService;
            _pictureService = pictureService;
            _storeInformationSettings = storeInformationSettings;
            _webHelper = webHelper;
        }

        #endregion

        #region Fields

        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomerService _customerService;
        private readonly IPictureService _pictureService;
        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Utilities
        protected virtual void PrepareDefaultItem(IList<SelectListItem> items, bool withSpecialDefaultItem, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //whether to insert the first special item for the default value
            if (!withSpecialDefaultItem)
                return;

            //at now we use "0" as the default value
            const string value = "0";

            //prepare item text
            defaultItemText = defaultItemText ?? "Hepsi";

            //insert this default item at first
            items.Insert(0, new SelectListItem { Text = defaultItemText, Value = value });
        }
        protected void PrepareCompanyList(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            List<IdentityRole> availableEtitors = _roleManager.Roles.ToList();
            foreach (IdentityRole item in availableEtitors)
            {
                items.Add(new SelectListItem { Value = item.Id, Text = item.Name });
            }
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }


        #endregion

        #region Customers

        public IActionResult Index()
        {
            var users = _customerService.GetAllApplicationUser();
            
            var model = new List<ApplicationUserModel>();
            foreach (var user in users)
            {
                var userModel = new ApplicationUserModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateRegistered = user.DateRegistered,
                    NickName = user.NickName,
                    AvatarURL = _pictureService.GetPictureUrl(Convert.ToInt32(user.AvatarURL), 120,
                        defaultPictureType: PictureType.Avatar)??$"{_webHelper.GetStoreLocation()}images/default-avatar.jpg",
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Id = user.Id
                };

                model.Add(userModel);
            }
            return View(model);
        }

        /// <summary>
        /// create
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            AdminRegisterViewModel model = new AdminRegisterViewModel();
            List<IdentityRole> roles = _roleManager.Roles.ToList();
            PrepareCompanyList(model.IdentityRoles, true, "Kullanıcı Rol Seçiniz");

            IQueryable<string> selectedRoles = (from ur in _context.UserRoles
                                                join u in _context.Users on ur.UserId equals u.Id
                                                join r in roles on ur.RoleId equals r.Id
                                                select r.Id);

            model.SelectedUserRoleId = selectedRoles.FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminRegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateRegistered = DateTime.UtcNow.ToLongDateString(),
                    Position = "",
                    NickName = model.NickName,
                    AvatarURL =model.AvatarId.ToString(),
                    PhoneNumber = model.Phone

                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    IdentityRole role = await _roleManager.FindByIdAsync(model.SelectedUserRoleId);
                    Customer.CreateCustomerEvent(ContactStatus.Approved, user.FirstName, user.Id, user.Email, model.AvatarId.ToString());
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Name, user.FirstName));
                    IdentityResult ıdentityResult = await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Surname, user.LastName));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.FacebookLink, model.FacebookLink ?? ""));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.TwitterLink, model.TwitterLink ?? ""));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.InstagramLink, model.InstagramLink ?? ""));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Email, model.Email ?? ""));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Role, role.Name ?? ""));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.PictureId, model.AvatarId.ToString() ?? ""));
                    if (role.Name == "Editor")
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.CustomerManagersRole));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.ReadOperationName));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.CreateOperationName));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.DeleteOperationName));
                    if (role.Name == "Admin")
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.CustomerAdministratorsRole));
                    await _userManager.AddToRoleAsync(user, role.Name);
                    SuccessNotification("Kullanıcı Oluşturuldu.");
                    return RedirectToAction("Edit",new{id=user.Id});
                }
                ErrorNotification(result.Errors.ToString());
            }
            PrepareCompanyList(model.IdentityRoles, true, "Kullanıcı Rol Seçiniz");
            return View(model);
        }

        public IActionResult Edit(string id)
        {
            ApplicationUser user = _userManager.FindByIdAsync(id).Result;
            AdminRegisterViewModel model = new AdminRegisterViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                NickName = user.NickName,
                Id = user.Id
            };
            IList<Claim> claim = _userManager.GetClaimsAsync(user).Result;
            foreach (Claim item in claim)
            {
                if (item.Type == "PictureId")
                {
                    model.AvatarId = Convert.ToInt32(item.Value);
                    model.AvatarUrl = _pictureService.GetPictureUrl(pictureId: Convert.ToInt32(item.Value),
                        defaultPictureType: PictureType.Avatar,
                        targetSize: _storeInformationSettings.AvatarPictureSize);
                    model.AvatarId = Convert.ToInt32(item.Value);
                }

                if (item.Type == "FacebookLink")
                    model.FacebookLink = item.Value;
                if (item.Type == "TwitterLink")
                    model.TwitterLink = item.Value;
                if (item.Type == "InstagramLink")
                    model.InstagramLink = item.Value;
            }
            List<IdentityRole> roles = _roleManager.Roles.ToList();
            PrepareCompanyList(model.IdentityRoles, true, "Rol Seçiniz");
            string selectedRoles = (from ur in _context.UserRoles
                                    join u in _context.Users on ur.UserId equals u.Id
                                    join r in roles on ur.RoleId equals r.Id
                                    where ur.UserId == user.Id
                                    select r.Id).FirstOrDefault();

            model.SelectedUserRoleId = selectedRoles;
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AdminRegisterViewModel model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.Phone;
            user.NickName = model.NickName;
            user.AvatarURL = model.AvatarId.ToString();

            var update = await _userManager.UpdateAsync(user);
            if (update.Succeeded)
            {
                IList<string> role = await _userManager.GetRolesAsync(user);
                IdentityResult remove = await _userManager.RemoveFromRolesAsync(user, role);
                IdentityRole roles = await _roleManager.FindByIdAsync(model.SelectedUserRoleId);
                if (remove.Succeeded)
                    await _userManager.AddToRoleAsync(user, roles.Name);
                IList<Claim> claim = await _userManager.GetClaimsAsync(user);
                if (claim.Any())
                {
                    IdentityResult removeClaim = await _userManager.RemoveClaimsAsync(user, claim);
                    if (removeClaim.Succeeded)
                    {

                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Name, user.FirstName));
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Surname, user.LastName));
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.FacebookLink, model.FacebookLink ?? ""));
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.TwitterLink, model.TwitterLink ?? ""));
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.InstagramLink, model.InstagramLink ?? ""));
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.PictureId, model.AvatarId.ToString() ?? ""));
                        if (roles.Name == "Editor")
                            await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.CustomerManagersRole));
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.ReadOperationName));
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.CreateOperationName));
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.DeleteOperationName));
                        if (roles.Name == "Admin")
                            await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.CustomerAdministratorsRole));
                    }
                }

            }

            SuccessNotification("Kayıt Güncellendi");
            return RedirectToAction("Index");
        }
        // GET: Admin/user/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            AddPageHeader("Site Ayarları", "Site Sil");
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            AdminRegisterViewModel model = new AdminRegisterViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email

            };
            return View(model);
        }

        // POST: Admin/Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            ApplicationUser user = await _userManager.FindByIdAsync(id);
            Task<IList<Claim>> claimUser = _userManager.GetClaimsAsync(user);
            IdentityResult sonuc = await _userManager.RemoveClaimsAsync(user, claimUser.Result);
            IdentityResult result = await _userManager.DeleteAsync(user);
            ErrorNotification("Kayıt Silindi");
            return RedirectToAction(nameof(Index));
        }


        #endregion


    }
}