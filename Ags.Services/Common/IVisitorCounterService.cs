using System;
using System.Collections.Generic;
using Ags.Data.Domain.Common;

namespace Ags.Services.Common
{
    public interface IVisitorCounterService
    {
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="visitorCounter"></param>
        void Insert(VisitorCounter visitorCounter);
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="visitorCounter"></param>
        void Update(VisitorCounter visitorCounter);
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="visitorCounter"></param>
        void Delete(VisitorCounter visitorCounter);
        /// <summary>
        /// GetCounterCount
        /// </summary>
        /// <param name="createDate"></param>
        /// <returns></returns>
        int GetCounterCount(DateTime? createDate = null);
        /// <summary>
        /// GetCounterNewsCounter
        /// </summary>
        /// <param name="createDate"></param>
        /// <returns></returns>
        VisitorCounter GetCounterNewsCounter(DateTime? createDate = null);
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        VisitorCounter GetById(int id);
        /// <summary>
        /// GetByDate
        /// </summary>
        /// <param name="createDateTime"></param>
        /// <returns></returns>
        VisitorCounter GetByDate(DateTime? createDateTime = null);
        /// <summary>
        /// GetByListCounter        /// </summary>
        /// <returns></returns>
        List<VisitorCounter> GetByListCounter();
    }
}