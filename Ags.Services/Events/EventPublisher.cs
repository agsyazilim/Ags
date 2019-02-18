using System;
using System.Linq;
using Ags.Data.Core;
using Ags.Services.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Ags.Services.Events
{
    /// <summary>
    /// Evnt publisher
    /// </summary>
    public class EventPublisher : IEventPublisher
    {
        #region Fields

        private readonly ISubscriptionService _subscriptionService;

        #endregion

        #region Ctor

        public EventPublisher(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Publish to cunsumer
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="x">Event consumer</param>
        /// <param name="eventMessage">Event message</param>
        protected virtual void PublishToConsumer<T>(IConsumer<T> x, T eventMessage)
        {
            try
            {
                x.HandleEvent(eventMessage);
            }
            catch (Exception exc)
            {
                //log error
                ILogger logger = ServiceProviderFactory.ServiceProvider.GetRequiredService<ILogger>();
                //we put in to nested try-catch to prevent possible cyclic (if some error occurs)
                try
                {
                    logger.Error(exc.Message, exc);
                }
                catch (Exception)
                {
                    //do nothing
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Publish event
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="eventMessage">Event message</param>
        public virtual void Publish<T>(T eventMessage)
        {
            //get all event subscribers, excluding from not installed plugins
            System.Collections.Generic.List<IConsumer<T>> subscribers = _subscriptionService.GetSubscriptions<T>().ToList();

            //publish event to subscribers
            subscribers.ForEach(subscriber => PublishToConsumer(subscriber, eventMessage));
        }

        #endregion
    }
}