using System;
using System.Collections.Generic;
using System.Linq;
using AdminLTE.Services.Stores;
using Ags.Data.Core.Caching;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Stores;
using Ags.Services.Events;

namespace Ags.Services.Stores
{
    /// <summary>
    /// Store service
    /// </summary>
    public partial class StoreService : IStoreService
    {
        #region Fields

        private readonly IRepository<Store> _storeRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public StoreService(
            IRepository<Store> storeRepository,
            IStaticCacheManager cacheManager, IEventPublisher eventPublisher)
        {
            this._storeRepository = storeRepository;
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a store
        /// </summary>
        /// <param name="store">Store</param>
        public virtual void DeleteStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            if (store is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            IList<Store> allStores = GetAllStores();
            if (allStores.Count == 1)
                throw new Exception("You cannot delete the only configured store");
            _storeRepository.Delete(store);
            //event notification
            _eventPublisher.EntityDeleted(store);
            _cacheManager.RemoveByPattern(AgsStoreDefaults.StoresPatternCacheKey);
        }

        /// <summary>
        /// Gets all stores
        /// </summary>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Stores</returns>
        public virtual IList<Store> GetAllStores(bool loadCacheableCopy = true)
        {
            IList<Store> LoadStoresFunc()
            {
               var query = from s in _storeRepository.Table orderby s.DisplayOrder, s.Id select s;
                return query.ToList();
            }
            if (loadCacheableCopy)
            {
                //cacheable copy
                return _cacheManager.Get(AgsStoreDefaults.StoresAllCacheKey, () =>
                {
                    List<Store> result = new List<Store>();
                    foreach (Store store in LoadStoresFunc())
                        result.Add(new StoreForCaching(store));
                    return result;
                });
            }

            return LoadStoresFunc();
        }

        /// <summary>
        /// Gets a store
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Store</returns>
        public virtual Store GetStoreById(int storeId, bool loadCacheableCopy = true)
        {
            if (storeId == 0)
                return null;

            Store LoadStoreFunc()
            {
                return _storeRepository.GetById(storeId);
            }

            if (!loadCacheableCopy)
                return LoadStoreFunc();

            //cacheable copy
            string key = string.Format(AgsStoreDefaults.StoresByIdCacheKey, storeId);
            return _cacheManager.Get(key, () =>
            {
                Store store = LoadStoreFunc();
                if (store == null)
                    return null;
                return new StoreForCaching(store);
            });
        }

        /// <summary>
        /// Inserts a store
        /// </summary>
        /// <param name="store">Store</param>
        public virtual void InsertStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            if (store is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _storeRepository.Insert(store);
            //event notification
            _eventPublisher.EntityInserted(store);
            _cacheManager.RemoveByPattern(AgsStoreDefaults.StoresPatternCacheKey);
        }

        /// <summary>
        /// Updates the store
        /// </summary>
        /// <param name="store">Store</param>
        public virtual void UpdateStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            if (store is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _storeRepository.Update(store);
            _eventPublisher.EntityUpdated(store);
            _cacheManager.RemoveByPattern(AgsStoreDefaults.StoresPatternCacheKey);

            //event notification
        }

        /// <summary>
        /// Parse comma-separated Hosts
        /// </summary>
        /// <param name="store">Store</param>
        /// <returns>Comma-separated hosts</returns>
        public virtual string[] ParseHostValues(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            List<string> parsedValues = new List<string>();
            if (string.IsNullOrEmpty(store.Hosts))
                return parsedValues.ToArray();

            string[] hosts = store.Hosts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string host in hosts)
            {
                string tmp = host.Trim();
                if (!string.IsNullOrEmpty(tmp))
                    parsedValues.Add(tmp);
            }

            return parsedValues.ToArray();
        }

        /// <summary>
        /// Indicates whether a store contains a specified host
        /// </summary>
        /// <param name="store">Store</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        public virtual bool ContainsHostValue(Store store, string host)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            if (string.IsNullOrEmpty(host))
                return false;

            bool contains = this.ParseHostValues(store).Any(x => x.Equals(host, StringComparison.InvariantCultureIgnoreCase));

            return contains;
        }

        #endregion
    }
}