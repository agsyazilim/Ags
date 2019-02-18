using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Media;
using Ags.Services.Common;
using Ags.Services.Events;

namespace Ags.Services.Media
{
    public partial class GalleryService:IGalleryService
    {
        private readonly IRepository<PhotoGallery> _photoGalleryRepository;
        private readonly IRepository<PhotoGalleryMapping> _photoGalleryMapRepository;
        private readonly ISectionService _sectionService;
        private readonly IEventPublisher _eventPublisher;

        public GalleryService(IRepository<PhotoGallery> photoGalleryRepository, IRepository<PhotoGalleryMapping> photoGalleryMapRepository, ISectionService sectionService, IEventPublisher eventPublisher)
        {
            this._photoGalleryRepository = photoGalleryRepository;
            this._photoGalleryMapRepository = photoGalleryMapRepository;
            this._sectionService = sectionService;
            this._eventPublisher = eventPublisher;
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="photoGallery"></param>
        public void Insert(PhotoGallery photoGallery)
        {
            if(photoGallery==null)
                throw new ArgumentNullException(nameof(photoGallery));
            _photoGalleryRepository.Insert(photoGallery);
            _eventPublisher.EntityInserted(photoGallery);

        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="photoGallery"></param>
        public void Delete(PhotoGallery photoGallery)
        {
            if (photoGallery == null)
                throw new ArgumentNullException(nameof(photoGallery));
            _photoGalleryRepository.Delete(photoGallery);
            _eventPublisher.EntityDeleted(photoGallery);
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="photoGallery"></param>
        public void Update(PhotoGallery photoGallery)
        {
            if (photoGallery == null)
                throw new ArgumentNullException(nameof(photoGallery));
            _photoGalleryRepository.Update(photoGallery);
            _eventPublisher.EntityUpdated(photoGallery);
        }
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PhotoGallery GetById(int id)
        {
            return _photoGalleryRepository.GetById(id);
        }
        /// <summary>
        /// GetPhotoGalleries
        /// </summary>
        /// <returns></returns>
        public IList<PhotoGallery> GetPhotoGalleries()
        {
            var query = _photoGalleryRepository.Table;
            query = query.Where(x => x.Published);
            var result = query.ToList();
            return result;
        }
        /// <summary>
        /// GetAllPhotoGalleries
        /// </summary>
        /// <param name="photoGaleryId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public IPagedList<PhotoGalleryMapping> GetAllPhotoGalleries(int photoGaleryId = 0, int pageIndex = 0, int pageSize = Int32.MaxValue,
            bool showHidden = false)
        {
            var query = _photoGalleryMapRepository.Table;
            if (photoGaleryId > 0)
            {
                //instagram dahil değil
                query = query.Where(x => x.GalleryId != photoGaleryId);
            }
            if (!showHidden)
            {
                query = query.Where(x => x.Gallery.Published);

            }

            var result = query.ToList();
            return new PagedList<PhotoGalleryMapping>(result,pageIndex,pageSize);
        }
        /// <summary>
        /// InsertPhotoGalleryMap
        /// </summary>
        /// <param name="photoGalleryMapping"></param>
        public void InsertPhotoGalleryMap(PhotoGalleryMapping photoGalleryMapping)
        {
           if(photoGalleryMapping==null)
               throw new ArgumentNullException(nameof(photoGalleryMapping));
            _photoGalleryMapRepository.Insert(photoGalleryMapping);
        }
        /// <summary>
        /// DeletePhotoGalleryMap
        /// </summary>
        /// <param name="photoGalleryMapping"></param>
        public void DeletePhotoGalleryMap(PhotoGalleryMapping photoGalleryMapping)
        {
            if (photoGalleryMapping == null)
                throw new ArgumentNullException(nameof(photoGalleryMapping));
            _photoGalleryMapRepository.Delete(photoGalleryMapping);
        }
        /// <summary>
        /// UpdatePhotoGalleryMap
        /// </summary>
        /// <param name="photoGalleryMapping"></param>
        public void UpdatePhotoGalleryMap(PhotoGalleryMapping photoGalleryMapping)
        {
            if (photoGalleryMapping == null)
                throw new ArgumentNullException(nameof(photoGalleryMapping));
            _photoGalleryMapRepository.Update(photoGalleryMapping);
        }
        /// <summary>
        /// GetGalleryMappingList
        /// </summary>
        /// <param name="galeryId"></param>
        /// <returns></returns>
        public IList<PhotoGalleryMapping> GetGalleryMappingList(int galeryId)
        {
           if(galeryId==0)
               throw new ArgumentNullException(nameof(galeryId));
            var query = _photoGalleryMapRepository.Table;
            query = query.Where(x => x.GalleryId == galeryId);

            return query.ToList();
        }
        /// <summary>
        /// GetGalleryMappingList
        /// </summary>
        /// <returns></returns>
        public IList<PhotoGalleryMapping> GetGalleryMappingList()
        {
            var query = _photoGalleryMapRepository.Table;
            return query.ToList();
        }
    }
}