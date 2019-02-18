using System.Collections.Generic;
using Ags.Data.Domain.Common;

namespace Ags.Services.Common
{
    public interface ISectionService
    {
        /// <summary>
        /// gets getall section
        /// </summary>
        /// <returns></returns>
        IList<Section> GetAllSection();
        /// <summary>
        /// gets get description
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        string GetDescription(string sectionName);
        /// <summary>
        /// GetNewsSection
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        string[] GetNewsSection(string sectionName);
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="section"></param>
        void Insert(Section section);
        /// <summary>
        /// update
        /// </summary>
        /// <param name="section"></param>
        void Update(Section section);
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="section"></param>
        void Delete(Section section);
        /// <summary>
        /// gets getbysection
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        Section GetBySectionId(int sectionId);

        Section GetByName(string name);
    }
}