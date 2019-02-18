// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Events.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Ags.Data.Domain.News
{
    /// <summary>
    /// News comment approved event
    /// </summary>
    public class NewsCommentApprovedEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="newsComment">News comment</param>
        public NewsCommentApprovedEvent(NewsComment newsComment)
        {
            this.NewsComment = newsComment;
        }

        /// <summary>
        /// News comment
        /// </summary>
        /// <value>The news comment.</value>
        public NewsComment NewsComment { get; }
    }
}