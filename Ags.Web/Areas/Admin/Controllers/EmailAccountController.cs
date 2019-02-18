using System;
using Ags.Data.Common;
using Ags.Data.Core;
using Ags.Data.Domain;
using Ags.Data.Domain.Message;
using Ags.Services.Configuration;
using Ags.Services.Logging;
using Ags.Services.Message;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Email;
using Ags.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EmailAccountSettings = Ags.Data.Domain.EmailAccountSettings;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class EmailAccountController : BaseAdminController
    {
        #region Fields


        private readonly ICustomerActivityService _customerActivityService;
        private readonly IEmailAccountModelFactory _emailAccountModelFactory;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IEmailSender _emailSender;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Ctor

        public EmailAccountController(
            ICustomerActivityService customerActivityService,
            IEmailAccountModelFactory emailAccountModelFactory,
            IEmailAccountService emailAccountService,
            IEmailSender emailSender,
            ISettingService settingService,
            IStoreContext storeContext,  UserManager<ApplicationUser> userManager)
        {

            this._customerActivityService = customerActivityService;
            this._emailAccountModelFactory = emailAccountModelFactory;
            this._emailAccountService = emailAccountService;
            this._emailSender = emailSender;
            this._settingService = settingService;
            this._storeContext = storeContext;

            _userManager = userManager;
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {


            //prepare model
            EmailAccountSearchModel model = _emailAccountModelFactory.PrepareEmailAccountSearchModel(new EmailAccountSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(EmailAccountSearchModel searchModel)
        {


            //prepare model
            EmailAccountListModel model = _emailAccountModelFactory.PrepareEmailAccountListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult MarkAsDefaultEmail(int id)
        {


            EmailAccount defaultEmailAccount = _emailAccountService.GetEmailAccountById(id);
            if (defaultEmailAccount == null)
                return RedirectToAction("List");
            EmailAccountSettings emailAccountSettings = _settingService.LoadSetting<EmailAccountSettings>();
            emailAccountSettings.DefaultEmailAccountId = defaultEmailAccount.Id;
            _settingService.SaveSetting(emailAccountSettings);

            return RedirectToAction("List");
        }

        public virtual IActionResult Create()
        {


            //prepare model
            EmailAccountModel model = _emailAccountModelFactory.PrepareEmailAccountModel(new EmailAccountModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(EmailAccountModel model, bool continueEditing)
        {


            if (ModelState.IsValid)
            {
                EmailAccount emailAccount = model.ToEntity<EmailAccount>();

                //set password manually
                emailAccount.Password = model.Password;
                _emailAccountService.InsertEmailAccount(emailAccount);

                //activity log
                _customerActivityService.InsertActivity("AddNewEmailAccount",
                    string.Format("AddNewEmailAccount{0}", emailAccount.Id), emailAccount);

                SuccessNotification("Added");

                return continueEditing ? RedirectToAction("Edit", new { id = emailAccount.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _emailAccountModelFactory.PrepareEmailAccountModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {


            //try to get an email account with the specified id
            EmailAccount emailAccount = _emailAccountService.GetEmailAccountById(id);
            if (emailAccount == null)
                return RedirectToAction("List");

            //prepare model
            EmailAccountModel model = _emailAccountModelFactory.PrepareEmailAccountModel(null, emailAccount);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(EmailAccountModel model, bool continueEditing)
        {

            //try to get an email account with the specified id
            EmailAccount emailAccount = _emailAccountService.GetEmailAccountById(model.Id);
            if (emailAccount == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                emailAccount = model.ToEntity(emailAccount);
                _emailAccountService.UpdateEmailAccount(emailAccount);

                //activity log
                _customerActivityService.InsertActivity("EditEmailAccount",
                    string.Format("EditEmailAccount{0}", emailAccount.Id), emailAccount);

                SuccessNotification("Updated");

                return continueEditing ? RedirectToAction("Edit", new { id = emailAccount.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _emailAccountModelFactory.PrepareEmailAccountModel(model, emailAccount, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public virtual IActionResult ChangePassword(EmailAccountModel model)
        {


            //try to get an email account with the specified id
            EmailAccount emailAccount = _emailAccountService.GetEmailAccountById(model.Id);
            if (emailAccount == null)
                return RedirectToAction("List");

            //do not validate model
            emailAccount.Password = model.Password;
            _emailAccountService.UpdateEmailAccount(emailAccount);

            SuccessNotification("PasswordChanged");

            return RedirectToAction("Edit", new { id = emailAccount.Id });
        }

        [HttpPost, ActionName("Edit")]
        public virtual IActionResult SendTestEmail(EmailAccountModel model)
        {

            //try to get an email account with the specified id
            EmailAccount emailAccount = _emailAccountService.GetEmailAccountById(model.Id);
            if (emailAccount == null)
                return RedirectToAction("List");

            if (!CommonHelper.IsValidEmail(model.SendTestEmailTo))
            {
                ErrorNotification("WrongEmail", false);
                return View(model);
            }

            try
            {
                if (string.IsNullOrWhiteSpace(model.SendTestEmailTo))
                    throw new AgsException("Enter test email address");

                string subject = _storeContext.CurrentStore.Name + ". Testing email functionality.";
                string body = "Email works fine.";
                _emailSender.SendEmailAsync(emailAccount, subject, body, emailAccount.Email, emailAccount.DisplayName, model.SendTestEmailTo, null);

                SuccessNotification("Mail Gönderildi", false);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message, false);
            }

            //prepare model
            model = _emailAccountModelFactory.PrepareEmailAccountModel(model, emailAccount, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {


            //try to get an email account with the specified id
            EmailAccount emailAccount = _emailAccountService.GetEmailAccountById(id);
            if (emailAccount == null)
                return RedirectToAction("List");

            try
            {
                _emailAccountService.DeleteEmailAccount(emailAccount);

                //activity log
                _customerActivityService.InsertActivity("DeleteEmailAccount",
                    string.Format("DeleteEmailAccount{0}", emailAccount.Id), emailAccount);

                SuccessNotification("Deleted");

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = emailAccount.Id });
            }
        }

        #endregion
    }
}