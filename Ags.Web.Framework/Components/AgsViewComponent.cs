﻿using System.Collections.Generic;
using Ags.Data.Core;
using Ags.Services.Events;
using Ags.Web.Framework.Events;
using Ags.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;

namespace Ags.Web.Framework.Components
{
    /// <summary>
    /// Base class for ViewComponent in nopCommerce
    /// </summary>
    public abstract class AgsViewComponent : ViewComponent
    {
        private void PublishModelPrepared<TModel>(TModel model)
        {
            //Components are not part of the controller life cycle.
            //Hence, we could no longer use Action Filters to intercept the Models being returned
            //as we do in the /Nop.Web.Framework/Mvc/Filters/PublishModelEventsAttribute.cs for controllers

            //model prepared event
            if (model is BaseAgsModel)
            {
                IEventPublisher eventPublisher = ServiceProviderFactory.ServiceProvider.GetRequiredService<IEventPublisher>();

                //we publish the ModelPrepared event for all models as the BaseAgsModel,
                //so you need to implement IConsumer<ModelPrepared<BaseAgsModel>> interface to handle this event
                eventPublisher.ModelPrepared(model as BaseAgsModel);
            }

            if (model is IEnumerable<BaseAgsModel> modelCollection)
            {
                IEventPublisher eventPublisher = ServiceProviderFactory.ServiceProvider.GetRequiredService<IEventPublisher>();

                //we publish the ModelPrepared event for collection as the IEnumerable<BaseAgsModel>,
                //so you need to implement IConsumer<ModelPrepared<IEnumerable<BaseAgsModel>>> interface to handle this event
                eventPublisher.ModelPrepared(modelCollection);
            }
        }
        /// <summary>
        /// Returns a result which will render the partial view with name <paramref name="viewName"/>.
        /// </summary>
        /// <param name="viewName">The name of the partial view to render.</param>
        /// <param name="model">The model object for the view.</param>
        /// <returns>A <see cref="ViewViewComponentResult"/>.</returns>
        public new ViewViewComponentResult View<TModel>(string viewName, TModel model)
        {
            PublishModelPrepared(model);

            //invoke the base method
            return base.View<TModel>(viewName, model);
        }

        /// <summary>
        /// Returns a result which will render the partial view
        /// </summary>
        /// <param name="model">The model object for the view.</param>
        /// <returns>A <see cref="ViewViewComponentResult"/>.</returns>
        public new ViewViewComponentResult View<TModel>(TModel model)
        {
            PublishModelPrepared(model);

            //invoke the base method
            return base.View<TModel>(model);
        }

        /// <summary>
        ///  Returns a result which will render the partial view with name viewName
        /// </summary>
        /// <param name="viewName">The name of the partial view to render.</param>
        /// <returns>A <see cref="ViewViewComponentResult"/>.</returns>
        public new ViewViewComponentResult View(string viewName)
        {
            //invoke the base method
            return base.View(viewName);
        }
    }
}