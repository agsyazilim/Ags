// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="SearchTermReportLine.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Ags.Data.Domain.Common
{
    /// <summary>
    /// Search term record (for statistics)
    /// </summary>
    public class SearchTermReportLine
    {
        /// <summary>
        /// Gets or sets the keyword
        /// </summary>
        /// <value>The keyword.</value>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets search count
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }
    }
}
