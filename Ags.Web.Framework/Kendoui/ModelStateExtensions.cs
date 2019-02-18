﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace Ags.Web.Framework.Kendoui
{
    /// <summary>
    /// ModelState extensions
    /// </summary>
    public static class ModelStateExtensions
    {
        private static Dictionary<string, object> SerializeModelState(ModelStateEntry modelState)
        {
            List<string> errors = new List<string>();
            for (int i = 0; i < modelState.Errors.Count; i++)
            {
                ModelError modelError = modelState.Errors[i];
                string errorText = ValidationHelpers.GetModelErrorMessageOrDefault(modelError);

                if (!string.IsNullOrEmpty(errorText))
                {
                    errors.Add(errorText);
                }
            }

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["errors"] = errors.ToArray();
            return dictionary;
        }

        /// <summary>
        /// Serialize errors
        /// </summary>
        /// <param name="modelStateDictionary">ModelStateDictionary</param>
        /// <returns>Result</returns>
        public static object SerializeErrors(this ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Where(entry => entry.Value.Errors.Any())
                .ToDictionary(entry => entry.Key, entry => SerializeModelState(entry.Value));
        }

        /// <summary>
        /// Serialized ModelStateDictionary errors
        /// </summary>
        /// <param name="modelState">ModelStateDictionary</param>
        /// <returns>Result</returns>
        public static object ToDataSourceResult(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.SerializeErrors();
            }
            return null;
        }
    }
}