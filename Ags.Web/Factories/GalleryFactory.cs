using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core;
using Ags.Data.Domain.Media;
using Ags.Services.Media;
using Ags.Web.Models.Media;

namespace Ags.Web.Factories
{
    public class GalleryFactory:IGalleryFactory
    {
        private readonly IGalleryService _galleryService;
        private readonly IPictureService _pictureService;
        private readonly IWebHelper _webHelper;

        public GalleryFactory(IGalleryService galleryService, IPictureService pictureService, IWebHelper webHelper)
        {
            _galleryService = galleryService;
            _pictureService = pictureService;
            _webHelper = webHelper;
        }
       public PhotoGalleryModel PreParePhotoGalleryModel(PhotoGalleryModel model,PhotoGalleryMapping photoGallery)
        {
            if(photoGallery==null)
                return new PhotoGalleryModel();
            model.Id = photoGallery.Id;
            model.DisplayOrder = photoGallery.DisplayOrder;
            model.Url = photoGallery.Url;
            model.PictureModels = PreParePictureModel(photoGallery.PictureId);
            return model;
        }

        public PictureModel PreParePictureModel(int pictureId)
        {
            if(pictureId==0)
                return new PictureModel();
            var picture = _pictureService.GetPictureById(pictureId);
            var model = new PictureModel
            {
                AlternateText = picture.AltAttribute,
                Title = picture.TitleAttribute,
                FullSizeImageUrl = _pictureService.GetPictureUrl(picture,1920),
                ImageUrl = _pictureService.GetPictureUrl(picture,1300),
                ThumbImageUrl = _pictureService.GetPictureUrl(picture,400)

            };
            return model;

        }

        public List<PhotoGalleryModel> PreParePhotoGalleryListModel(int photoGallery)
        {
            if(photoGallery==0)
                return new List<PhotoGalleryModel>();
            var query = _galleryService.GetGalleryMappingList(photoGallery);
            var model = new List<PhotoGalleryModel>();
            foreach (var photoGalleryMapping in query)
            {
                model.Add(PreParePhotoGalleryModel(new PhotoGalleryModel(), photoGalleryMapping));
            }
            return model;
        }

        public GalleryModel PrePareGalleryModel(int galleryId)
        {
           if(galleryId==0)
               return new GalleryModel();

            var query = _galleryService.GetById(galleryId);
            var model = new GalleryModel();
            model.Name = query.Name;
            model.DisplayOrder = query.DisplayOrder;
            model.Published = query.Published;
            model.Id = query.Id;
            model.PhotoGalleryModels = PreParePhotoGalleryListModel(model.Id);
            return model;
        }

        public PhotoGalleryListModel PrePareGalleryListModel(PhotoGalleryPagingFilteringModel command)
        {
           var model = new  PhotoGalleryListModel();
            if (command.PageSize <= 0)
            {
                command.PageSize = 15;
            }

            if (command.PageNumber <= 0)
            {
                command.PageNumber = 1;
            }
          
            var galeries = _galleryService.GetAllPhotoGalleries(photoGaleryId:3,pageIndex: command.PageNumber - 1, pageSize: command.PageSize);
            model.PhotoGalleryPagingFilteringModel.LoadPagedList(galeries);
            model.PhotoGalleryModels = galeries.Select(x =>
            {
                var photoGalleriModel = new PhotoGalleryModel();
                PreParePhotoGalleryModel(photoGalleriModel, x);
                return photoGalleriModel;
            }).ToList();
            var galeri = _galleryService.GetPhotoGalleries().Where(x=>x.Id!=3);
            model.GalleryModels = galeri.Select(x =>
            {
                var galleryModel = PrePareGalleryModel(x.Id);
                return galleryModel;
            }).ToList();
            return model;
        }

        public PhotoGalleryPagingFilteringModel PrepareGalleryPagingFilteringModel(int? filterItem)
        {
            throw new System.NotImplementedException();
        }
    }
}