using System;
using System.Linq;
using AdminLTE.Areas.Admin.Factories;
using Ags.Data.Domain.Message;
using Ags.Services.Configuration;
using Ags.Services.Message;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Email;
using Ags.Web.Framework.Extensions;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the email account model factory implementation
    /// </summary>
    public partial class EmailAccountModelFactory : IEmailAccountModelFactory
    {
        #region Fields

        private readonly ISettingService _settingService;
        private readonly IEmailAccountService _emailAccountService;

        #endregion

        #region Ctor

        public EmailAccountModelFactory(
            IEmailAccountService emailAccountService, ISettingService settingService)
        {
            this._emailAccountService = emailAccountService;
            _settingService = settingService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare email account search model
        /// </summary>
        /// <param name="searchModel">Email account search model</param>
        /// <returns>Email account search model</returns>
        public virtual EmailAccountSearchModel PrepareEmailAccountSearchModel(EmailAccountSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged email account list model
        /// </summary>
        /// <param name="searchModel">Email account search model</param>
        /// <returns>Email account list model</returns>
        public virtual EmailAccountListModel PrepareEmailAccountListModel(EmailAccountSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get email accounts
            System.Collections.Generic.IList<EmailAccount> emailAccounts = _emailAccountService.GetAllEmailAccounts();
            EmailAccountSettings emailAccountSettings = _settingService.LoadSetting<EmailAccountSettings>();
            //prepare grid model
            EmailAccountListModel model = new EmailAccountListModel
            {
                Data = emailAccounts.PaginationByRequestModel(searchModel).Select(emailAccount =>
                {
                    //fill in model values from the entity
                    EmailAccountModel emailAccountModel = emailAccount.ToModel<EmailAccountModel>();

                    //fill in additional values (not existing in the entity)
                    emailAccountModel.IsDefaultEmailAccount = emailAccount.Id == emailAccountSettings.DefaultEmailAccountId;

                    return emailAccountModel;
                }),
                Total = emailAccounts.Count
            };

            return model;
        }



        /// <summary>
        /// Prepare email account model
        /// </summary>
        /// <param name="model">Email account model</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Email account model</returns>
        public virtual EmailAccountModel PrepareEmailAccountModel(EmailAccountModel model,
            EmailAccount emailAccount, bool excludeProperties = false)
        {
            //fill in model values from the entity
            if (emailAccount != null)
                model = model ?? emailAccount.ToModel<EmailAccountModel>();

            //set default values for the new model
            if (emailAccount == null)
                model.Port = 25;

            return model;
        }

        #endregion
    }
}