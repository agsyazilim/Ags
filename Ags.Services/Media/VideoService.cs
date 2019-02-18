using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Media;

namespace Ags.Services.Media
{
    public class VideoService:IVideoService
    {
        private readonly IRepository<Video> _videoRepository;
        private readonly IRepository<VideoGallery> _videoGalleryRepository;
        public VideoService(IRepository<Video> videoRepository,
            IRepository<VideoGallery> videoGalleryRepository)
        {
            this._videoRepository = videoRepository;
            this._videoGalleryRepository = videoGalleryRepository;

        }
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="video"></param>
        public void Insert(Video video)
        {
         if(video==null)
             throw new ArgumentNullException(nameof(video));
            _videoRepository.Insert(video);
        }
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="video"></param>
        public void Delete(Video video)
        {
            if (video == null)
                throw new ArgumentNullException(nameof(video));
            _videoRepository.Delete(video);
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="video"></param>
        public void Update(Video video)
        {
            if (video == null)
                throw new ArgumentNullException(nameof(video));
            _videoRepository.Update(video);
        }
        /// <summary>
        /// GetByVideo
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public Video GetByVideo(int videoId)
        {
            return _videoRepository.GetById(videoId);
        }
        /// <summary>
        /// GetVideoList
        /// </summary>
        /// <returns></returns>
        public IList<Video> GetVideoList()
        {
            var query = _videoRepository.Table;
            query = query.Where(x => x.Published & x.IsApproved);
            var result = query.ToList();
            return result;
        }

        public IPagedList<VideoGallery> GetAllVideoGallery(int videoGalleryId = 0, int pageIndex = 0, int pageSize = Int32.MaxValue,
            bool showHidden = false)
        {
            var query = _videoGalleryRepository.Table;
            if (videoGalleryId > 0)
            {
                query = query.Where(x => x.Id == videoGalleryId);
            }

            if (showHidden)
                query = query.Where(x => x.Published);
            var result = query.ToList();
            return new PagedList<VideoGallery>(result,pageIndex,pageSize);
        }

        public IPagedList<Video> GetAllVideo(int videoId = 0, int pageIndex = 0, int pageSize = Int32.MaxValue, bool showHidden = false)
        {
            var query = _videoRepository.Table;
            if (videoId > 0)
            {
                query = query.Where(x => x.Id == videoId);
            }

            if (showHidden)
                query = query.Where(x => x.Published);
            var result = query.ToList();
            return new PagedList<Video>(result, pageIndex, pageSize);
        }

        /// <summary>
        /// InsertVideo
        /// </summary>
        /// <param name="videoGallery"></param>
        public void InsertVideo(VideoGallery videoGallery)
        {
            if (videoGallery == null)
                throw new ArgumentNullException(nameof(videoGallery));

            _videoGalleryRepository.Insert(videoGallery);
        }
        /// <summary>
        /// DeleteVideo
        /// </summary>
        /// <param name="videoGallery"></param>
        public void DeleteVideo(VideoGallery videoGallery)
        {
            if (videoGallery == null)
                throw new ArgumentNullException(nameof(videoGallery));
            _videoGalleryRepository.Delete(videoGallery);
        }
        /// <summary>
        /// UpdateVideo
        /// </summary>
        /// <param name="videoGallery"></param>
        public void UpdateVideo(VideoGallery videoGallery)
        {
            if (videoGallery == null)
                throw new ArgumentNullException(nameof(videoGallery));
            _videoGalleryRepository.Update(videoGallery);
        }
        /// <summary>
        /// GetByVideoGallery
        /// </summary>
        /// <param name="videoGalleryId"></param>
        /// <returns></returns>
        public VideoGallery GetByVideoGallery(int videoGalleryId)
        {
            if(videoGalleryId==0)
                throw new ArgumentNullException(nameof(videoGalleryId));
            return _videoGalleryRepository.GetById(videoGalleryId);
        }
        /// <summary>
        /// GetVideoGalleryList
        /// </summary>
        /// <returns></returns>
        public IList<VideoGallery> GetVideoGalleryList()
        {
            var query = _videoGalleryRepository.Table;
            query = query.Where(x => x.Published);
            var result = query.ToList();
            return result;
        }
        /// <summary>
        /// GetVideoList
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public IList<Video> GetVideoList(int videoId)
        {
            if(videoId==0)
                throw new ArgumentNullException(nameof(videoId));
            var query = _videoRepository.Table;
            query = query.Where(x => x.Id == videoId);
            return query.ToList();
        }
    }
}