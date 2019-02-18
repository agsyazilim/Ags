using System.Collections.Generic;
using Ags.Data.Core.Pages;
using Ags.Data.Domain.Media;

namespace Ags.Services.Media
{
    public interface IVideoService
    {
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="video"></param>
        void Insert(Video video);
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="video"></param>
        void Delete(Video video);
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="video"></param>
        void Update(Video video);
        /// <summary>
        /// GetByVideo
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        Video GetByVideo(int videoId);
        /// <summary>
        /// GetVideoList
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        IList<Video> GetVideoList(int videoId);
        /// <summary>
        /// GetVideoList
        /// </summary>
        /// <returns></returns>
        IList<Video> GetVideoList();

        IPagedList<VideoGallery> GetAllVideoGallery(int videoGalleryId = 0, int pageIndex = 0,
            int pageSize = int.MaxValue, bool showHidden = false);

        IPagedList<Video> GetAllVideo(int videoId = 0, int pageIndex = 0,
            int pageSize = int.MaxValue, bool showHidden = false);
        /// <summary>
        /// InsertVideo
        /// </summary>
        /// <param name="videoGallery"></param>
        void InsertVideo(VideoGallery videoGallery);
        /// <summary>
        /// DeleteVideo
        /// </summary>
        /// <param name="videoGallery"></param>
        void DeleteVideo(VideoGallery videoGallery);
        /// <summary>
        /// UpdateVideo
        /// </summary>
        /// <param name="videoGallery"></param>
        void UpdateVideo(VideoGallery videoGallery);
        /// <summary>
        /// GetByVideoGallery
        /// </summary>
        /// <param name="videoGalleryId"></param>
        /// <returns></returns>
        VideoGallery GetByVideoGallery(int videoGalleryId);
        /// <summary>
        /// GetVideoGalleryList
        /// </summary>
        /// <returns></returns>
        IList<VideoGallery> GetVideoGalleryList();

    }
}