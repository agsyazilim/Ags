using System;
using System.Linq;
using Ags.Data.Core;
using Ags.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ags.Web.Framework.Mvc.ModelBinding
{
    /// <summary>
    /// Represents model binder provider for the creating NopModelBinder
    /// </summary>
    public class AgsModelBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Creates a nop model binder based on passed context
        /// </summary>
        /// <param name="context">Model binder provider context</param>
        /// <returns>Model binder</returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            Type modelType = context.Metadata.ModelType;
            if (!typeof(BaseAgsModel).IsAssignableFrom(modelType))
                return null;
            //use NopModelBinder as a ComplexTypeModelBinder for BaseAgsModel
            if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType)
            {
                //create binders for all model properties
                System.Collections.Generic.Dictionary<ModelMetadata, IModelBinder> propertyBinders = context.Metadata.Properties
                    .ToDictionary(modelProperty => modelProperty, modelProperty => context.CreateBinder(modelProperty));
                return new AgsModelBinder(propertyBinders, ServiceProviderFactory.ServiceProvider.GetService<ILoggerFactory>());
            }
            //or return null to further search for a suitable binder
            return null;
        }
    }
}
