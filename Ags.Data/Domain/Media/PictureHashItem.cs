// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="PictureHashItem.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Ags.Data.Domain.Media
{
    /// <summary>
    /// Helper class for making pictures hashes from DB
    /// Implements the <see cref="System.IComparable" />
    /// Implements the <see cref="System.IComparable{Nop.Core.Domain.Media.PictureHashItem}" />
    /// </summary>
    /// <seealso cref="System.IComparable" />
    /// <seealso cref="System.IComparable{Nop.Core.Domain.Media.PictureHashItem}" />
    public partial class PictureHashItem : IComparable, IComparable<PictureHashItem>
    {
        /// <summary>
        /// Gets or sets the picture identifier.
        /// </summary>
        /// <value>The picture identifier.</value>
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public byte[] Hash { get; set; }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This instance precedes <paramref name="obj">obj</paramref> in the sort order.
        /// Zero
        /// This instance occurs in the same position in the sort order as <paramref name="obj">obj</paramref>.
        /// Greater than zero
        /// This instance follows <paramref name="obj">obj</paramref> in the sort order.</returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as PictureHashItem);
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public int CompareTo(PictureHashItem other)
        {
            return other == null ? -1 : PictureId.CompareTo(other.PictureId);
        }
    }
}