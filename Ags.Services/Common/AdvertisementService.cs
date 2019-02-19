using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Common;

namespace Ags.Services.Common
{
    public partial class AdvertisementService:IAdvertisementService
    {
        private readonly IRepository<Advertisement> _advertisementRepository;
        public AdvertisementService(IRepository<Advertisement> advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
        }
        public void Insert(Advertisement advertisement)
        {
           if(advertisement==null)
               throw new ArgumentNullException(nameof(advertisement));
            _advertisementRepository.Insert(advertisement);
        }

        public void Update(Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));
            _advertisementRepository.Update(advertisement);
        }

        public void Delete(Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));
            _advertisementRepository.Delete(advertisement);
        }

        public Advertisement GetById(int advertisementId)
        {
            return _advertisementRepository.GetById(advertisementId);
        }

        public Advertisement GetBySectionId(int sectionId)
        {
            var query = _advertisementRepository.Table;
            query = query.Where(x =>x.IsApproved==true);
            query = query.Where(x => x.SectionId == sectionId);
            query = query.Where(x =>x.EndDate.HasValue || x.EndDate >= DateTime.UtcNow);
            var result = query.FirstOrDefault();
            return result;
        }

        public List<Advertisement> GetAdvertisementsList()
        {
            var query = _advertisementRepository.Table;
            var result = query.ToList();
            return result;
        }
    }
}