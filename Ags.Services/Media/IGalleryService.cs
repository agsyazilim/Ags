using System.Collections.Generic;
using Ags.Data.Core.Pages;
using Ags.Data.Domain.Media;

namespace Ags.Services.Media
{
    public interface IGalleryService
    {
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="photoGallery"></param>
        void Insert(PhotoGallery photoGallery);
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="photoGallery"></param>
        void Delete(PhotoGallery photoGallery);
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="photoGallery"></param>
        void Update(PhotoGallery photoGallery);
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PhotoGallery GetById(int id);
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IList<PhotoGallery> GetPhotoGalleries();
        /// <summary>
        /// GetAllPhotoGalleries
        /// </summary>
        /// <param name="photoGaleryId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        IPagedList<PhotoGalleryMapping> GetAllPhotoGalleries(int photoGaleryId = 0, int pageIndex = 0,
            int pageSize = int.MaxValue, bool showHidden = false);
        /// <summary>
        /// InsertPhotoGalleryMap
        /// </summary>
        /// <param name="photoGalleryMapping"></param>
        void InsertPhotoGalleryMap(PhotoGalleryMapping photoGalleryMapping);
        /// <summary>
        /// DeletePhotoGalleryMap
        /// </summary>
        /// <param name="photoGalleryMapping"></param>
        void DeletePhotoGalleryMap(PhotoGalleryMapping photoGalleryMapping);
        /// <summary>
        /// UpdatePhotoGalleryMap
        /// </summary>
        /// <param name="photoGalleryMapping"></param>
        void UpdatePhotoGalleryMap(PhotoGalleryMapping photoGalleryMapping);
        /// <summary>
        /// GetGalleryMappingList
        /// </summary>
        /// <param name="galeryId"></param>
        /// <returns></returns>
        IList<PhotoGalleryMapping> GetGalleryMappingList(int galeryId);
        /// <summary>
        /// GetGalleryMappingList
        /// </summary>
        /// <returns></returns>
        IList<PhotoGalleryMapping> GetGalleryMappingList();


    }
}