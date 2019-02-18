using Ags.Services.Topics;
using Ags.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public partial class TopicController : BasePublicController
    {
        #region Fields

        private readonly ITopicModelFactory _topicModelFactory;
        private readonly ITopicService _topicService;

        #endregion

        #region Ctor

        public TopicController(
            ITopicModelFactory topicModelFactory,
            ITopicService topicService)
        {
            this._topicModelFactory = topicModelFactory;
            this._topicService = topicService;
        }

        #endregion

        #region Methods

        public virtual IActionResult TopicDetails(int topicId)
        {
            var model = _topicModelFactory.PrepareTopicModelById(topicId);
            //access to Topics preview
            if (model == null || !model.Published)
                return RedirectToRoute("HomePage");
            //template
            string templateViewPath = _topicModelFactory.PrepareTemplateViewPath(model.TopicTemplateId);
            return View(templateViewPath, model);
        }

        public virtual IActionResult TopicDetailsPopup(string systemName)
        {
           var model = _topicModelFactory.PrepareTopicModelBySystemName(systemName);
            if (model == null)
                return RedirectToRoute("HomePage");

            ViewBag.IsPopup = true;

            //template
            string templateViewPath = _topicModelFactory.PrepareTemplateViewPath(model.TopicTemplateId);
            return PartialView(templateViewPath, model);
        }

        #endregion
    }
}