using System;
using AdminLTE.Areas.Admin.Factories;
using Ags.Data.Domain.Stores;
using Ags.Services.Stores;
using Ags.Web.Areas.Admin.Models.Stores;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the store model factory implementation
    /// </summary>
    public partial class StoreModelFactory : IStoreModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor

        public StoreModelFactory(IBaseAdminModelFactory baseAdminModelFactory,
            IStoreService storeService)
        {
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._storeService = storeService;
        }

        #endregion

        #region Methods


        #endregion

        public StoreSearchModel PrepareStoreSearchModel(StoreSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public StoreListModel PrepareStoreListModel(StoreSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public StoreModel PrepareStoreModel(StoreModel model, Store store, bool excludeProperties = false)
        {
            throw new NotImplementedException();
        }
    }
}