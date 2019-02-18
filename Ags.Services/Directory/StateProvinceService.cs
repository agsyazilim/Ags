using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Caching;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Directory;

namespace Ags.Services.Directory
{
    /// <summary>
    /// State province service
    /// </summary>
    public partial class StateProvinceService : IStateProvinceService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IRepository<StateProvince> _stateProvinceRepository;

        #endregion

        #region Ctor

        public StateProvinceService(ICacheManager cacheManager,


            IRepository<StateProvince> stateProvinceRepository)
        {
            this._cacheManager = cacheManager;
            this._stateProvinceRepository = stateProvinceRepository;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Deletes a state/province
        /// </summary>
        /// <param name="stateProvince">The state/province</param>
        public virtual void DeleteStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException(nameof(stateProvince));

            _stateProvinceRepository.Delete(stateProvince);

            _cacheManager.RemoveByPattern(AgsDirectoryDefaults.StateProvincesPatternCacheKey);

            //event notification
        }

        /// <summary>
        /// Gets a state/province
        /// </summary>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <returns>State/province</returns>
        public virtual StateProvince GetStateProvinceById(int stateProvinceId)
        {
            if (stateProvinceId == 0)
                return null;

            return _stateProvinceRepository.GetById(stateProvinceId);
        }

        /// <summary>
        /// Gets a state/province by abbreviation
        /// </summary>
        /// <param name="abbreviation">The state/province abbreviation</param>
        /// <param name="countryId">Country identifier; pass null to load the state regardless of a country</param>
        /// <returns>State/province</returns>
        public virtual StateProvince GetStateProvinceByAbbreviation(string abbreviation, int? countryId = null)
        {
            if (string.IsNullOrEmpty(abbreviation))
                return null;

            IQueryable<StateProvince> query = _stateProvinceRepository.Table.Where(state => state.Abbreviation == abbreviation);



            StateProvince stateProvince = query.FirstOrDefault();
            return stateProvince;
        }


        /// <summary>
        /// Gets a state/province collection by country identifier
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>States</returns>
        public virtual IList<StateProvince> GetStateProvincesByCountryId(bool showHidden = false)
        {
            string key = string.Format(AgsDirectoryDefaults.StateProvincesAllCacheKey,showHidden);
            return _cacheManager.Get(key, () =>
            {
                IQueryable<StateProvince> query = from sp in _stateProvinceRepository.Table
                            orderby sp.DisplayOrder, sp.Name
                            where (showHidden || sp.Published)
                            select sp;
                List<StateProvince> stateProvinces = query.ToList();



                return stateProvinces;
            });
        }

        /// <summary>
        /// Gets all states/provinces
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>States</returns>
        public virtual IList<StateProvince> GetStateProvinces(bool showHidden = false)
        {
            IQueryable<StateProvince> query = from sp in _stateProvinceRepository.Table
                        orderby  sp.DisplayOrder, sp.Name
                        where showHidden || sp.Published
                        select sp;
            List<StateProvince> stateProvinces = query.ToList();
            return stateProvinces;
        }

        /// <summary>
        /// Inserts a state/province
        /// </summary>
        /// <param name="stateProvince">State/province</param>
        public virtual void InsertStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException(nameof(stateProvince));

            _stateProvinceRepository.Insert(stateProvince);

            _cacheManager.RemoveByPattern(AgsDirectoryDefaults.StateProvincesPatternCacheKey);

            //event notification
        }

        /// <summary>
        /// Updates a state/province
        /// </summary>
        /// <param name="stateProvince">State/province</param>
        public virtual void UpdateStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException(nameof(stateProvince));

            _stateProvinceRepository.Update(stateProvince);

            _cacheManager.RemoveByPattern(AgsDirectoryDefaults.StateProvincesPatternCacheKey);

            //event notification
        }

        #endregion
    }
}