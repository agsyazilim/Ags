using Ags.Data.Domain;

namespace Ags.Web.Framework.Security.Captcha
{
    /// <summary>
    /// Captcha extensions
    /// </summary>
    public static class CaptchaSettingsExtension
    {
        /// <summary>
        /// Get warning message if a selected Captcha version is not supported
        /// </summary>
        /// <param name="captchaSettings"></param>
        /// <returns></returns>
        public static string GetWrongCaptchaMessage(this CaptchaSettings captchaSettings)
        {
            return "The reCAPTCHA response is invalid or malformed. Please try again.";
        }
    }
}