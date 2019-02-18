using System;
using System.Linq;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Polls;

namespace Ags.Services.Polls
{
    /// <summary>
    /// Poll service
    /// </summary>
    public partial class PollService : IPollService
    {
        #region Fields

        private readonly IRepository<Poll> _pollRepository;
        private readonly IRepository<PollAnswer> _pollAnswerRepository;
        private readonly IRepository<PollVotingRecord> _pollVotingRecordRepository;

        #endregion

        #region Ctor

        public PollService(

            IRepository<Poll> pollRepository,
            IRepository<PollAnswer> pollAnswerRepository,
            IRepository<PollVotingRecord> pollVotingRecordRepository)
        {
            this._pollRepository = pollRepository;
            this._pollAnswerRepository = pollAnswerRepository;
            this._pollVotingRecordRepository = pollVotingRecordRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a poll
        /// </summary>
        /// <param name="pollId">The poll identifier</param>
        /// <returns>Poll</returns>
        public virtual Poll GetPollById(int pollId)
        {
            if (pollId == 0)
                return null;

            return _pollRepository.GetById(pollId);
        }

        /// <summary>
        /// Gets polls
        /// </summary>
        /// <param name="storeId">The store identifier; pass 0 to load all records</param>
        /// <param name="languageId">Language identifier; pass 0 to load all records</param>
        /// <param name="showHidden">Whether to show hidden records (not published, not started and expired)</param>
        /// <param name="loadShownOnHomePageOnly">Retrieve only shown on home page polls</param>
        /// <param name="systemKeyword">The poll system keyword; pass null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Polls</returns>
        public virtual IPagedList<Poll> GetPolls(int storeId, int languageId = 0, bool
            showHidden = false, bool loadShownOnHomePageOnly = false, string systemKeyword = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            IQueryable<Poll> query = _pollRepository.Table;

            //whether to load not published, not started and expired polls
            if (!showHidden)
            {
                DateTime utcNow = DateTime.UtcNow;
                query = query.Where(poll => poll.Published);
                query = query.Where(poll => !poll.StartDateUtc.HasValue || poll.StartDateUtc <= utcNow);
                query = query.Where(poll => !poll.EndDateUtc.HasValue || poll.EndDateUtc >= utcNow);
            }

            //load homepage polls only
            if (loadShownOnHomePageOnly)
                query = query.Where(poll => poll.ShowOnHomePage);



            //filter by system keyword
            if (!string.IsNullOrEmpty(systemKeyword))
                query = query.Where(poll => poll.SystemKeyword == systemKeyword);



            //order records by display order
            query = query.OrderBy(poll => poll.DisplayOrder).ThenBy(poll => poll.Id);

            //return paged list of polls
            return new PagedList<Poll>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Deletes a poll
        /// </summary>
        /// <param name="poll">The poll</param>
        public virtual void DeletePoll(Poll poll)
        {
            if (poll == null)
                throw new ArgumentNullException(nameof(poll));

            _pollRepository.Delete(poll);

            //event notification
        }

        /// <summary>
        /// Inserts a poll
        /// </summary>
        /// <param name="poll">Poll</param>
        public virtual void InsertPoll(Poll poll)
        {
            if (poll == null)
                throw new ArgumentNullException(nameof(poll));

            _pollRepository.Insert(poll);

            //event notification
        }

        /// <summary>
        /// Updates the poll
        /// </summary>
        /// <param name="poll">Poll</param>
        public virtual void UpdatePoll(Poll poll)
        {
            if (poll == null)
                throw new ArgumentNullException(nameof(poll));

            _pollRepository.Update(poll);

            //event notification
        }

        /// <summary>
        /// Gets a poll answer
        /// </summary>
        /// <param name="pollAnswerId">Poll answer identifier</param>
        /// <returns>Poll answer</returns>
        public virtual PollAnswer GetPollAnswerById(int pollAnswerId)
        {
            if (pollAnswerId == 0)
                return null;

            return _pollAnswerRepository.GetById(pollAnswerId);
        }

        /// <summary>
        /// Deletes a poll answer
        /// </summary>
        /// <param name="pollAnswer">Poll answer</param>
        public virtual void DeletePollAnswer(PollAnswer pollAnswer)
        {
            if (pollAnswer == null)
                throw new ArgumentNullException(nameof(pollAnswer));

            _pollAnswerRepository.Delete(pollAnswer);

            //event notification
        }

        /// <summary>
        /// Gets a value indicating whether customer already voted for this poll
        /// </summary>
        /// <param name="pollId">Poll identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Result</returns>
        public virtual bool AlreadyVoted(int pollId)
        {
            if (pollId == 0 )
                return false;

            bool result = (from pa in _pollAnswerRepository.Table
                          join pvr in _pollVotingRecordRepository.Table on pa.Id equals pvr.PollAnswerId
                          where pa.PollId == pollId
                          select pvr).Any();
            return result;
        }

        public void InsertPollVoit(PollVotingRecord pollVotingRecord)
        {
           if(pollVotingRecord==null)
               throw new ArgumentNullException(nameof(pollVotingRecord));
           _pollVotingRecordRepository.Insert(pollVotingRecord);
        }

        #endregion
    }
}