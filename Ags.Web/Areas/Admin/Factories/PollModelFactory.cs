using System;
using System.Linq;
using AdminLTE.Areas.Admin.Factories;
using Ags.Data.Domain.Polls;
using Ags.Services.Polls;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Polls;
using Ags.Web.Framework.Extensions;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the poll model factory implementation
    /// </summary>
    public partial class PollModelFactory : IPollModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IPollService _pollService;

        #endregion

        #region Ctor

        public PollModelFactory(IBaseAdminModelFactory baseAdminModelFactory,

            IPollService pollService)
        {
            this._baseAdminModelFactory = baseAdminModelFactory;

            this._pollService = pollService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare poll answer search model
        /// </summary>
        /// <param name="searchModel">Poll answer search model</param>
        /// <param name="poll">Poll</param>
        /// <returns>Poll answer search model</returns>
        protected virtual PollAnswerSearchModel PreparePollAnswerSearchModel(PollAnswerSearchModel searchModel, Poll poll)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (poll == null)
                throw new ArgumentNullException(nameof(poll));

            searchModel.PollId = poll.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare poll search model
        /// </summary>
        /// <param name="searchModel">Poll search model</param>
        /// <returns>Poll search model</returns>
        public virtual PollSearchModel PreparePollSearchModel(PollSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));



            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged poll list model
        /// </summary>
        /// <param name="searchModel">Poll search model</param>
        /// <returns>Poll list model</returns>
        public virtual PollListModel PreparePollListModel(PollSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get polls
            var polls = _pollService.GetPolls(showHidden: true,
                storeId: 0, pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            PollListModel model = new PollListModel
            {
                Data = polls.Select(poll =>
                {
                    //fill in model values from the entity
                    PollModel pollModel = poll.ToModel<PollModel>();

                    //convert dates to the user time
                    if (poll.StartDateUtc.HasValue)
                        pollModel.StartDate = poll.StartDateUtc.Value;
                    if (poll.EndDateUtc.HasValue)
                        pollModel.EndDate = poll.EndDateUtc.Value;

                    return pollModel;
                }),
                Total = polls.TotalCount
            };

            return model;
        }

        /// <summary>
        /// Prepare poll model
        /// </summary>
        /// <param name="model">Poll model</param>
        /// <param name="poll">Poll</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Poll model</returns>
        public virtual PollModel PreparePollModel(PollModel model, Poll poll, bool excludeProperties = false)
        {
            if (poll != null)
            {
                //fill in model values from the entity
                model = model ?? poll.ToModel<PollModel>();

                model.StartDate = poll.StartDateUtc;
                model.EndDate = poll.EndDateUtc;

                //prepare nested search model
                PreparePollAnswerSearchModel(model.PollAnswerSearchModel, poll);
            }

            //set default values for the new model
            if (poll == null)
            {
                model.Published = true;
                model.ShowOnHomePage = true;
            }

            return model;
        }

        /// <summary>
        /// Prepare paged poll answer list model
        /// </summary>
        /// <param name="searchModel">Poll answer search model</param>
        /// <param name="poll">Poll</param>
        /// <returns>Poll answer list model</returns>
        public virtual PollAnswerListModel PreparePollAnswerListModel(PollAnswerSearchModel searchModel, Poll poll)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (poll == null)
                throw new ArgumentNullException(nameof(poll));

            //get poll answers
            System.Collections.Generic.List<PollAnswer> pollAnswers = poll.PollAnswers.OrderBy(pollAnswer => pollAnswer.DisplayOrder).ToList();

            //prepare list model
            PollAnswerListModel model = new PollAnswerListModel
            {
                //fill in model values from the entity
                Data = pollAnswers.PaginationByRequestModel(searchModel).Select(pollAnswer => new PollAnswerModel
                {
                    Id = pollAnswer.Id,
                    PollId = pollAnswer.PollId,
                    Name = pollAnswer.Name,
                    NumberOfVotes = pollAnswer.NumberOfVotes,
                    DisplayOrder = pollAnswer.DisplayOrder
                }),
                Total = pollAnswers.Count
            };

            return model;
        }

        #endregion
    }
}