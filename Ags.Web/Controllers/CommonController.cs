using Ags.Data.Domain;
using Ags.Data.Html;
using Ags.Services.Logging;
using Ags.Services.Media;
using Ags.Services.Message;
using Ags.Services.News;
using Ags.Web.Factories;
using Ags.Web.Framework.Mvc.Filters;
using Ags.Web.Models.Common;
using Ags.Web.Models.Media;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ags.Web.Controllers
{
    public partial class CommonController : BasePublicController
    {
        #region Fields

        private readonly CommonSettings _commonSettings;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        private readonly IEmailAccountService _emailAccountService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly INewsService _newsService;
        private readonly INewsModelFactory _newsModelFactory;
        private readonly IPictureService _pictureService;
        private readonly IGalleryFactory _galleryFactory;
        private readonly IGalleryService _galleryService;
        private string _urlVakit = "https://www.yenisafak.com/";
        #endregion

        #region Ctor

        public CommonController(
            CommonSettings commonSettings,
            ICommonModelFactory commonModelFactory,
            ICustomerActivityService customerActivityService,
            ILogger logger, IEmailSender emailSender, IEmailAccountService emailAccountService, EmailAccountSettings emailAccountSettings, INewsService newsService, INewsModelFactory newsModelFactory, IPictureService pictureService, IGalleryFactory galleryFactory, IGalleryService galleryService)
        {
            this._commonSettings = commonSettings;
            this._commonModelFactory = commonModelFactory;
            this._customerActivityService = customerActivityService;
            this._logger = logger;
            _emailSender = emailSender;
            _emailAccountService = emailAccountService;
            _emailAccountSettings = emailAccountSettings;
            _newsService = newsService;
            _newsModelFactory = newsModelFactory;
            _pictureService = pictureService;
            _galleryFactory = galleryFactory;
            _galleryService = galleryService;
        }

        #endregion

        #region Utilites

        protected string GetContent(string urlAddress)
        {
            Uri url = new Uri(urlAddress);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string html = client.DownloadString(url);
            return html;
        }
        protected HttpClient ClientOlustur()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
        #endregion
        #region Methods

        //page not found
        public virtual IActionResult PageNotFound()
        {
            if (_commonSettings.Log404Errors)
            {
                IStatusCodeReExecuteFeature statusCodeReExecuteFeature = HttpContext?.Features?.Get<IStatusCodeReExecuteFeature>();
                //TODO add locale resource
                _logger.Error($"Error 404. The requested page ({statusCodeReExecuteFeature?.OriginalPath}) was not found");
            }

            Response.StatusCode = 404;
            Response.ContentType = "text/html";

            return View();
        }
        //contact us page
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        public virtual IActionResult ContactUs()
        {
            ContactUsModel model = new ContactUsModel();
            model = _commonModelFactory.PrepareContactUsModel(model, false);
            return View(model);
        }

        [HttpPost, ActionName("ContactUs")]
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        public virtual IActionResult ContactUsSend(ContactUsModel model, bool captchaValid)
        {

            model = _commonModelFactory.PrepareContactUsModel(model, true);

            if (ModelState.IsValid)
            {
                string subject = _commonSettings.SubjectFieldOnContactUsForm ? model.Subject : null;
                string body = HtmlHelper.FormatText(model.Enquiry, false, true, false, false, false, false);
                var emailAccount =
                     _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
                _emailSender.SendEmailAsync(emailAccount, subject, body, model.Email.Trim(), model.FullName, emailAccount.Email, emailAccount.DisplayName);
                model.SuccessfullySent = true;
                model.Result = "Mesajınız gönderildi";

                //activity log
                _customerActivityService.InsertActivity("PublicStore.ContactUs",
                    "ActivityLog.PublicStore.ContactUs");

                return View(model);
            }

            return View(model);
        }


        public virtual IActionResult GenericUrl()
        {
            var result = HttpContext.Request;
            //seems that no entity was found
            return InvokeHttp404();
        }

        //store is closed
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        public virtual IActionResult StoreClosed()
        {
            return View();
        }

        //helper method to redirect users. Workaround for GenericPathRoute class where we're not allowed to do it
        public virtual IActionResult InternalRedirect(string url, bool permanentRedirect)
        {
            //ensure it's invoked from our GenericPathRoute class
            if (HttpContext.Items["ags.RedirectFromGenericPathRoute"] == null ||
                !Convert.ToBoolean(HttpContext.Items["ags.RedirectFromGenericPathRoute"]))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            //home page
            if (string.IsNullOrEmpty(url))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            //prevent open redirection attack
            if (!Url.IsLocalUrl(url))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            if (permanentRedirect)
            {
                return RedirectPermanent(url);
            }

            return Redirect(url);
        }

        [HttpPost]
        public async Task<JsonResult> HavaDurumu(string id)
        {
            string urlAddress = "http://api.openweathermap.org/data/2.5/weather?q=" + id + "&appid=91e938ec61f2965bd5164917569617c8&units=metric";
            Uri url = new Uri(urlAddress);
            HttpClient client = ClientOlustur();
            var result = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<HavaDurumuModel>(result);
            return Json(data);
        }
        /// <summary>
        /// Vakitler the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;JsonResult&gt;.</returns>
        [HttpPost]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> Vakitler(string id)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _urlVakit = _urlVakit + id + "-namaz-vakitleri";
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
            htmldoc.LoadHtml(GetContent(_urlVakit));
            var imsak = htmldoc.DocumentNode.SelectNodes("//*[@id=\"time1\"]");
            var gunes = htmldoc.DocumentNode.SelectNodes("//*[@id=\"time2\"]");
            var oglen = htmldoc.DocumentNode.SelectNodes("//*[@id=\"time3\"]");
            var ikinidi = htmldoc.DocumentNode.SelectNodes("//*[@id=\"time4\"]");
            var aksam = htmldoc.DocumentNode.SelectNodes("//*[@id=\"time5\"]");
            var yatsi = htmldoc.DocumentNode.SelectNodes("//*[@id=\"time6\"]");

            var vakit = new VakitModel();
            vakit.Imsak = imsak[0].InnerHtml;
            vakit.Gunes = gunes[0].InnerHtml;
            vakit.Ikindi = ikinidi[0].InnerHtml;
            vakit.Oglen = oglen[0].InnerHtml;
            vakit.Aksam = aksam[0].InnerHtml;
            vakit.Yatsi = yatsi[0].InnerHtml;

            return Json(vakit);
        }
        [HttpPost]
#pragma warning disable 1998
        public async Task<JsonResult> GetBreakingNews()
#pragma warning restore 1998
        {
            var newsItems = _newsService.GetBreakingNews().Take(5);
            var model = _newsModelFactory.PrepareNewsOverviewModel(newsItems);
            return Json(model);
        }
        [HttpPost]
#pragma warning disable 1998
        public async Task<JsonResult> GetMainBannerList()
#pragma warning restore 1998
        {
            var query = _newsService.GetMainBannerList();
            var model = _newsModelFactory.PrepareNewsOverviewModel(query);
            return Json(model);
        }
        #endregion
        [HttpPost]
        public IActionResult GetInstagram()
        {
            var gallery = _galleryService.GetPhotoGalleries().FirstOrDefault(x => x.Name.Contains("InstagtamGaleri"));
            var model = new GalleryModel();
            if (gallery != null)
            {
                model = _galleryFactory.PrePareGalleryModel(gallery.Id);
                return Json(model);
            }
            else
            {
                return Json(model);
            }
        }

        [HttpPost]
        public IActionResult GetMainSlider()
        {
            var query = _newsService.GetMainBannerList();
            var model = _newsModelFactory.PrepareNewsSliderOverviewModel(query);
            return Json(model);
        }

        [HttpPost]
        public IActionResult GetRightSlider()
        {
            var query = _newsService.GetUpBannerList();
            var model = _newsModelFactory.PrepareNewsSliderOverviewModel(query);
            return Json(model);
        }

        [HttpPost]
        public IActionResult GetRigthTwoSlider()
        {
            var query = _newsService.GetTwoBannerList();
            var model = _newsModelFactory.PrepareNewsSliderOverviewModel(query);
            return Json(model);
        }
    }
}