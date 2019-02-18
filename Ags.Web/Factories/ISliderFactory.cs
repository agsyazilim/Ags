using System.Collections.Generic;
using Ags.Data.Domain.Media;
using Ags.Web.Models.Media;

namespace Ags.Web.Factories
{
    public interface ISliderFactory
    {
        SliderPictureModel PrePareSliderPictureModel(SliderPictureModel model, SliderPictureMapping sliderPicture);
        PictureModel PreParePictureModel(int pictureId);
        List<SliderPictureModel> PrePareSliderPictureListModel(int sliderId);
        SliderModel PrePareSliderModel(int sliderId);
        SliderModel PrePareSliderSectionModel(int sectionId);
    }
}