using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Ags.Services.Events
{
    /// <summary>
    /// Event subscription service
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        #region Methods

        /// <summary>
        /// Get subscriptions
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Event consumers</returns>
        public IList<IConsumer<T>> GetSubscriptions<T>()
        {
            return  ServiceProviderFactory.ServiceProvider.GetServices<IConsumer<T>>().ToList();
        }

        #endregion
    }
}