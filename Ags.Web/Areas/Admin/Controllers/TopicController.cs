using Ags.Data.Domain.Topics;
using Ags.Services.Customers;
using Ags.Services.Logging;
using Ags.Services.Seo;
using Ags.Services.Stores;
using Ags.Services.Topics;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Topics;
using Ags.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class TopicController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICustomerService _customerService;
        private readonly IStoreService _storeService;
        private readonly ITopicModelFactory _topicModelFactory;
        private readonly ITopicService _topicService;
        private readonly IUrlRecordService _urlRecordService;

        #endregion Fields

        #region Ctor

        public TopicController(
            ICustomerActivityService customerActivityService,
            ICustomerService customerService,
            IStoreService storeService,
            ITopicModelFactory topicModelFactory,
            ITopicService topicService,
            IUrlRecordService urlRecordService)
        {
            this._customerActivityService = customerActivityService;
            this._customerService = customerService;
            this._storeService = storeService;
            this._topicModelFactory = topicModelFactory;
            this._topicService = topicService;
            this._urlRecordService = urlRecordService;
        }

        #endregion



        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {


            //prepare model
            TopicSearchModel model = _topicModelFactory.PrepareTopicSearchModel(new TopicSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(TopicSearchModel searchModel)
        {

            //prepare model
            TopicListModel model = _topicModelFactory.PrepareTopicListModel(searchModel);

            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete

        public virtual IActionResult Create()
        {


            //prepare model
            TopicModel model = _topicModelFactory.PrepareTopicModel(new TopicModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(TopicModel model, bool continueEditing)
        {


            if (ModelState.IsValid)
            {
                if (!model.IsPasswordProtected)
                    model.Password = null;

                Topic topic = model.ToEntity<Topic>();
                _topicService.InsertTopic(topic);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(topic, model.SeName, topic.Title ?? topic.SystemName, true);
                _urlRecordService.SaveSlug(topic, model.SeName);

                SuccessNotification("İçerik Eklendi");

                //activity log
                _customerActivityService.InsertActivity("AddNewTopic",
                    string.Format("AddNewTopic{0}", topic.Title ?? topic.SystemName), topic);

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = topic.Id });
            }

            //prepare model
            model = _topicModelFactory.PrepareTopicModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {


            //try to get a topic with the specified id
            Topic topic = _topicService.GetTopicById(id);
            if (topic == null)
                return RedirectToAction("List");

            //prepare model
            TopicModel model = _topicModelFactory.PrepareTopicModel(null, topic);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(TopicModel model, bool continueEditing)
        {


            //try to get a topic with the specified id
            Topic topic = _topicService.GetTopicById(model.Id);
            if (topic == null)
                return RedirectToAction("List");

            if (!model.IsPasswordProtected)
                model.Password = null;

            if (ModelState.IsValid)
            {
                topic = model.ToEntity(topic);
                _topicService.UpdateTopic(topic);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(topic, model.SeName, topic.Title ?? topic.SystemName, true);
                _urlRecordService.SaveSlug(topic, model.SeName);



                SuccessNotification("İçerik Güncelendi");

                //activity log
                _customerActivityService.InsertActivity("EditTopic",
                    string.Format("EditTopic{0}", topic.Title ?? topic.SystemName), topic);

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = topic.Id });
            }

            //prepare model
            model = _topicModelFactory.PrepareTopicModel(model, topic, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {


            //try to get a topic with the specified id
            Topic topic = _topicService.GetTopicById(id);
            if (topic == null)
                return RedirectToAction("List");

            _topicService.DeleteTopic(topic);

            SuccessNotification("İçerik Sİlindi");

            //activity log
            _customerActivityService.InsertActivity("DeleteTopic",
                string.Format("DeleteTopic{0}", topic.Title ?? topic.SystemName), topic);

            return RedirectToAction("List");
        }

        #endregion
    }
}