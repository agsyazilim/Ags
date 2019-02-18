using System;
using Ags.Data.Domain;
using Ags.Data.Domain.Polls;
using Ags.Services;
using Ags.Services.Customers;
using Ags.Services.Polls;
using Ags.Web.Factories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public class PollController : BasePublicController
    {
        private readonly IPollService _pollService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerService _customerService;
        private readonly IPollModelFactory _pollModelFactory;

        public PollController(IPollService pollService, UserManager<ApplicationUser> userManager, ICustomerService customerService, IPollModelFactory pollModelFactory)
        {
            _pollService = pollService;
            _userManager = userManager;
            _customerService = customerService;
            _pollModelFactory = pollModelFactory;
        }

        

        #region Methods

        [HttpPost]
        public virtual IActionResult Vote(int id)
        {
            PollAnswer pollAnswer = _pollService.GetPollAnswerById(id);
            if (pollAnswer == null)
                return Json(new { error = "No poll answer found with the specified id" });

            Poll poll = pollAnswer.Poll;
            if (!poll.Published)
                return Json(new { error = "Poll is not available" });

            if (!poll.AllowGuestsToVote)
                return Json(new { error = "Yanlız Kayıtlı Kullanıcılar Oylayabilirler" });

            bool alreadyVoted = _pollService.AlreadyVoted(poll.Id);
            if (!alreadyVoted)
            {
                //vote
                var pollVoitRecord = new PollVotingRecord
                {
                    PollAnswerId = pollAnswer.Id,
                    CustomerId = User.GetCustomer(_userManager, _customerService) == null
                        ? 5
                        : User.GetCustomer(_userManager, _customerService).Id,
                    CreatedOnUtc = DateTime.UtcNow
                };
                _pollService.InsertPollVoit(pollVoitRecord);
                //update totals
                pollAnswer.NumberOfVotes = pollAnswer.PollVotingRecords.Count;
                _pollService.UpdatePoll(poll);
            }

            return Json(new
            {
                html = RenderPartialViewToString("_Poll", _pollModelFactory.PreparePollModel(poll, true))
            });
        }

        #endregion
    }
}