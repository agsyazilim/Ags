namespace Ags.Web.Framework.Mvc.Razor
{
    /// <summary>
    /// Web view page
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    public abstract class AgsRazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {

    }

    /// <summary>
    /// Web view page
    /// </summary>
    public abstract class AgsRazorPage : AgsRazorPage<dynamic>
    {
    }
}