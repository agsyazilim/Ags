using System;
using System.Linq;
using Ags.Data.Common;
using Ags.Data.Domain.Stores;
using Ags.Services.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Ags.Web.Framework
{
    /// <summary>
    /// Store context for web application
    /// </summary>
    public partial class WebStoreContext : IStoreContext
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStoreService _storeService;

        private Store _cachedStore;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="storeService">Store service</param>
        public WebStoreContext(
            IHttpContextAccessor httpContextAccessor,
            IStoreService storeService)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._storeService = storeService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current store
        /// </summary>
        public virtual Store CurrentStore
        {
            get
            {
                if (_cachedStore != null)
                    return _cachedStore;

                //try to determine the current store by HOST header
                string host = _httpContextAccessor.HttpContext?.Request?.Headers[HeaderNames.Host];

                System.Collections.Generic.IList<Store> allStores = _storeService.GetAllStores();
                Store store = allStores.FirstOrDefault(s => _storeService.ContainsHostValue(s, host));

                if (store == null)
                {
                    //load the first found store
                    store = allStores.FirstOrDefault();
                }

                _cachedStore = store ?? throw new Exception("No store could be loaded");

                return _cachedStore;
            }
        }



        #endregion
    }
}