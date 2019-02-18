
namespace Ags.Data.Core.Http
{
    /// <summary>
    /// Represents default values related to cookies
    /// </summary>
    public static partial class AgsCookieDefaults
    {
        /// <summary>
        /// Gets the cookie name prefix
        /// </summary>
        public static string Prefix => ".ags";

        /// <summary>
        /// Gets a cookie name of the customer
        /// </summary>
        public static string CustomerCookie => ".haber";

        /// <summary>
        /// Gets a cookie name of the antiforgery
        /// </summary>
        public static string AntiforgeryCookie => ".Antiforgery";

        /// <summary>
        /// Gets a cookie name of the session state
        /// </summary>
        public static string SessionCookie => ".Session";

        /// <summary>
        /// Gets a cookie name of the temp data
        /// </summary>
        public static string TempDataCookie => ".TempData";
       

        /// <summary>
        /// Gets a cookie name of the authentication
        /// </summary>
        public static string AuthenticationCookie => ".Authentication";

     
    }
}