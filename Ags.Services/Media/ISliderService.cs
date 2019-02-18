using System.Collections.Generic;
using Ags.Data.Domain.Media;

namespace Ags.Services.Media
{
    public interface ISliderService
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="slider"></param>
        void Delete(Slider slider);
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="slider"></param>
        void Insert(Slider slider);
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="slider"></param>
        void Update(Slider slider);
        /// <summary>
        /// GetBySliderId
        /// </summary>
        /// <param name="sliderId"></param>
        /// <returns></returns>
        Slider GetBySliderId(int sliderId);
        /// <summary>
        /// get all slider
        /// </summary>
        /// <returns></returns>
        IList<Slider> GetAllSlider();

        /// <summary>
        /// GetBySectionById
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        Slider GetBySectionById(int sectionId);
        /// <summary>
        /// DeleteSlidePicture
        /// </summary>
        /// <param name="sliderPictureMapping"></param>
        void DeleteSlidePicture(SliderPictureMapping sliderPictureMapping);
        /// <summary>
        /// InsertSliderPicture
        /// </summary>
        /// <param name="sliderPictureMapping"></param>
        void InsertSliderPicture(SliderPictureMapping sliderPictureMapping);
        /// <summary>
        /// UpdateSliderPicture
        /// </summary>
        /// <param name="sliderPictureMapping"></param>
        void UpdateSliderPicture(SliderPictureMapping sliderPictureMapping);
        /// <summary>
        /// SliderPictureMappings
        /// </summary>
        /// <param name="sliderId"></param>
        /// <returns></returns>
        IList<SliderPictureMapping> SliderPictureMappings(int sliderId);


    }
}