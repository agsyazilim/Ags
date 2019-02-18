using System;
using System.Collections.Generic;
using Ags.Data.Domain.Common;

namespace Ags.Services.Common
{
    public interface INewsCounterService
    {
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="newsCounter"></param>
        void Insert(NewsCounter newsCounter);
        /// <summary>
        /// update
        /// </summary>
        /// <param name="newsCounter"></param>
        void Update(NewsCounter newsCounter);
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="newsCounter"></param>
        void Delete(NewsCounter newsCounter);
        /// <summary>
        /// getcounter
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="createDate"></param>
        /// <returns></returns>
        int GetCounterCount(int entityId,string entityName,DateTime? createDate = null);
        /// <summary>
        /// getcounter news
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="createDate"></param>
        /// <returns></returns>
        NewsCounter GetCounterNewsCounter(int entityId, string entityName, DateTime? createDate = null);
        /// <summary>
        /// gets getbyId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        NewsCounter GetById(int id);
        /// <summary>
        /// gets getbydate
        /// </summary>
        /// <param name="createDateTime"></param>
        /// <returns></returns>
        NewsCounter GetByDate(DateTime? createDateTime=null);
        /// <summary>
        /// gets getbylist counter
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="createDate"></param>
        /// <returns></returns>
        List<NewsCounter> GetByListCounter(int entityId = 0, string entityName=null, DateTime? createDate = null);


    }
}