using System.Collections.Generic;
using Ags.Data.Domain.Media;
using Ags.Web.Models.Media;

namespace Ags.Web.Factories
{
    public interface IGalleryFactory
    {
        PhotoGalleryModel PreParePhotoGalleryModel(PhotoGalleryModel model,PhotoGalleryMapping photoGallery);
        PictureModel PreParePictureModel(int pictureId);
        List<PhotoGalleryModel> PreParePhotoGalleryListModel(int photoGallery);
        GalleryModel PrePareGalleryModel(int galleryId);
        PhotoGalleryListModel PrePareGalleryListModel(PhotoGalleryPagingFilteringModel command);
        PhotoGalleryPagingFilteringModel PrepareGalleryPagingFilteringModel(int? filterItem);



    }
}