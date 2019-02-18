using System;
using Ags.Data.Domain;
using Ags.Data.Domain.Polls;
using Ags.Services.Polls;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Polls;
using Ags.Web.Framework.Kendoui;
using Ags.Web.Framework.Mvc;
using Ags.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class PollController : BaseAdminController
    {
        #region Fields

        private readonly IPollModelFactory _pollModelFactory;
        private readonly IPollService _pollService;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Ctor

        public PollController(
            IPollModelFactory pollModelFactory,
            IPollService pollService, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager)
        {
            this._pollModelFactory = pollModelFactory;
            this._pollService = pollService;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        #endregion



        #region Polls

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {


            //prepare model
            PollSearchModel model = _pollModelFactory.PreparePollSearchModel(new PollSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(PollSearchModel searchModel)
        {


            //prepare model
            PollListModel model = _pollModelFactory.PreparePollListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {


            //prepare model
            PollModel model = _pollModelFactory.PreparePollModel(new PollModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(PollModel model, bool continueEditing)
        {

            if (ModelState.IsValid)
            {
                Poll poll = model.ToEntity<Poll>();
                poll.StartDateUtc = model.StartDate;
                poll.EndDateUtc = model.EndDate;
                _pollService.InsertPoll(poll);



                SuccessNotification("Yeni Anket Eklendi");

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = poll.Id });
            }

            //prepare model
            model = _pollModelFactory.PreparePollModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {


            //try to get a poll with the specified id
            Poll poll = _pollService.GetPollById(id);
            if (poll == null)
                return RedirectToAction("List");

            //prepare model
            PollModel model = _pollModelFactory.PreparePollModel(null, poll);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(PollModel model, bool continueEditing)
        {

            //try to get a poll with the specified id
            Poll poll = _pollService.GetPollById(model.Id);
            if (poll == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                poll = model.ToEntity(poll);
                poll.StartDateUtc = model.StartDate;
                poll.EndDateUtc = model.EndDate;
                _pollService.UpdatePoll(poll);



                SuccessNotification("Anket Güncellendi");

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = poll.Id });
            }

            //prepare model
            model = _pollModelFactory.PreparePollModel(model, poll, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {


            //try to get a poll with the specified id
            Poll poll = _pollService.GetPollById(id);
            if (poll == null)
                return RedirectToAction("List");

            _pollService.DeletePoll(poll);

            SuccessNotification("Anket Silindi");

            return RedirectToAction("List");
        }

        #endregion

        #region Poll answer

        [HttpPost]
        public virtual IActionResult PollAnswers(PollAnswerSearchModel searchModel)
        {


            //try to get a poll with the specified id
            Poll poll = _pollService.GetPollById(searchModel.PollId)
                ?? throw new ArgumentException("No poll found with the specified id");

            //prepare model
            PollAnswerListModel model = _pollModelFactory.PreparePollAnswerListModel(searchModel, poll);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult PollAnswerUpdate(PollAnswerModel model)
        {


            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            //try to get a poll answer with the specified id
            PollAnswer pollAnswer = _pollService.GetPollAnswerById(model.Id)
                ?? throw new ArgumentException("No poll answer found with the specified id");

            pollAnswer.Name = model.Name;
            pollAnswer.DisplayOrder = model.DisplayOrder;
            _pollService.UpdatePoll(pollAnswer.Poll);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult PollAnswerAdd(int pollId, PollAnswerModel model)
        {


            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            //try to get a poll with the specified id
            Poll poll = _pollService.GetPollById(pollId)
                ?? throw new ArgumentException("No poll found with the specified id", nameof(pollId));

            poll.PollAnswers.Add(new PollAnswer
            {
                Name = model.Name,
                DisplayOrder = model.DisplayOrder
            });
            _pollService.UpdatePoll(poll);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult PollAnswerDelete(int id)
        {


            //try to get a poll answer with the specified id
            PollAnswer pollAnswer = _pollService.GetPollAnswerById(id)
                ?? throw new ArgumentException("No poll answer found with the specified id", nameof(id));

            _pollService.DeletePollAnswer(pollAnswer);

            return new NullJsonResult();
        }

        #endregion
    }
}