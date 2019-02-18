using System;
using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Polls
{
    public partial class PollModel : BaseAgsEntityModel, ICloneable
    {
        public PollModel()
        {
            Answers = new List<PollAnswerModel>();
        }

        public string Name { get; set; }

        public bool AlreadyVoted { get; set; }

        public int TotalVotes { get; set; }

        public IList<PollAnswerModel> Answers { get; set; }

        public object Clone()
        {
            //we use a shallow copy (deep clone is not required here)
            return MemberwiseClone();
        }
    }

    public partial class PollAnswerModel : BaseAgsEntityModel
    {
        public string Name { get; set; }

        public int NumberOfVotes { get; set; }

        public double PercentOfTotalVotes { get; set; }
    }
}