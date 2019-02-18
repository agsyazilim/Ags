using System.Collections.Generic;
using Ags.Data.Domain.Common;

namespace Ags.Services.Common
{
    public interface IAdvertisementService
    {
        void Insert(Advertisement advertisement);
        void Update(Advertisement advertisement);
        void Delete(Advertisement advertisement);
        Advertisement GetById(int advertisementId);
        Advertisement GetBySectionId(int sectionId);
        List<Advertisement> GetAdvertisementsList();

    }
}