using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Common;
using Ags.Data.Domain.Polls;
using Ags.Services.Polls;
using Ags.Web.Models.Polls;

namespace Ags.Web.Factories
{
    public class PollModelFactory: IPollModelFactory
    {
        private readonly IPollService _pollService;
        private readonly IStoreContext _storeContext;

        public PollModelFactory(IPollService pollService,IStoreContext storeContext)
        {
            _pollService = pollService;
            _storeContext = storeContext;
        }

        public PollModel PreparePollModel(Poll poll, bool setAlreadyVotedProperty)
        {
            if (poll == null)
                throw new ArgumentNullException(nameof(poll));

            var model = new PollModel
            {
                Id = poll.Id,
                AlreadyVoted = setAlreadyVotedProperty&&_pollService.AlreadyVoted(poll.Id),
                Name = poll.Name
            };
            var answers = poll.PollAnswers.OrderBy(x => x.DisplayOrder);
            foreach (var answer in answers)
                model.TotalVotes += answer.NumberOfVotes;
            foreach (var pa in answers)
            {
                model.Answers.Add(new PollAnswerModel
                {
                    Id = pa.Id,
                    Name = pa.Name,
                    NumberOfVotes = pa.NumberOfVotes,
                    PercentOfTotalVotes = model.TotalVotes > 0 ? ((Convert.ToDouble(pa.NumberOfVotes) / Convert.ToDouble(model.TotalVotes)) * Convert.ToDouble(100)) : 0,
                });
            }

            return model;
        }

        public PollModel PreparePollModelBySystemName(string systemKeyword)
        {
            if (string.IsNullOrWhiteSpace(systemKeyword))
                return null;
            var poll = _pollService
                .GetPolls(_storeContext.CurrentStore.Id, systemKeyword: systemKeyword)
                .FirstOrDefault();

            //we do not cache nulls. that's why let's return an empty record (ID = 0)
            if (poll == null)
                return new PollModel { Id = 0 };

            var model = PreparePollModel(poll, true);

            //"AlreadyVoted" property of "PollModel" object depends on the current customer. Let's update it.
            //But first we need to clone the cached model (the updated one should not be cached)
           
            model.AlreadyVoted = _pollService.AlreadyVoted(model.Id);
            return model;
        }

        public List<PollModel> PrepareHomePagePollModels()
        {
          
            var cachedPolls = _pollService.GetPolls(_storeContext.CurrentStore.Id, loadShownOnHomePageOnly: true)
                    .Select(poll => PreparePollModel(poll, true)).ToList();

            //"AlreadyVoted" property of "PollModel" object depends on the current customer. Let's update it.
            //But first we need to clone the cached model (the updated one should not be cached)
            var model = new List<PollModel>();
            foreach (var poll in cachedPolls)
            {
                var pollModel = (PollModel)poll.Clone();
                pollModel.AlreadyVoted = _pollService.AlreadyVoted(pollModel.Id);
                model.Add(pollModel);
            }

            return model;
        }
    }
}