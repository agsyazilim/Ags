using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Media;
using Ags.Services.Common;

namespace Ags.Services.Media
{
    public class SliderService : ISliderService
    {
        private readonly IRepository<Slider> _sliderPictrueRepository;
        private readonly IRepository<SliderPictureMapping> _slidePictureMapRepository;
        private readonly ISectionService _sectionService;

        public SliderService(IRepository<Slider> sliderPictrueRepository, IRepository<SliderPictureMapping> slidePictureMapRepository, ISectionService sectionService)
        {
            this._sliderPictrueRepository = sliderPictrueRepository;
            this._slidePictureMapRepository = slidePictureMapRepository;
            this._sectionService = sectionService;
        }
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="slider"></param>
        public void Delete(Slider slider)
        {
            if (slider == null)
            {
                throw new ArgumentNullException(nameof(slider));
            }
            _sliderPictrueRepository.Delete(slider);
        }
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="slider"></param>
        public void Insert(Slider slider)
        {
            if (slider == null)
            {
                throw new ArgumentNullException(nameof(slider));
            }

            _sliderPictrueRepository.Insert(slider);
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="slider"></param>
        public void Update(Slider slider)
        {
            if (slider == null)
            {
                throw new ArgumentNullException(nameof(slider));
            }

            _sliderPictrueRepository.Update(slider);
        }
        /// <summary>
        /// GetBySliderId
        /// </summary>
        /// <param name="sliderId"></param>
        /// <returns></returns>
        public Slider GetBySliderId(int sliderId)
        {
            if(sliderId==0)
                throw new ArgumentNullException(nameof(sliderId));
            return _sliderPictrueRepository.GetById(sliderId);
        }

        public IList<Slider> GetAllSlider()
        {
            var query = _sliderPictrueRepository.Table;
            return query.ToList();
        }

        /// <summary>
        /// GetBySectionById
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public Slider GetBySectionById(int sectionId)
        {
            if (sectionId == 0)
                return null;
            var query = _sliderPictrueRepository.Table;

            query = query.Where(x => x.SectionId == sectionId);
            var slider = query.FirstOrDefault();
            return slider;
        }
        /// <summary>
        /// DeleteSlidePicture
        /// </summary>
        /// <param name="sliderPictureMapping"></param>
        public void DeleteSlidePicture(SliderPictureMapping sliderPictureMapping)
        {
            if (sliderPictureMapping == null)
            {
                throw new ArgumentNullException(nameof(sliderPictureMapping));
            }

            _slidePictureMapRepository.Delete(sliderPictureMapping);
        }
        /// <summary>
        /// InsertSliderPicture
        /// </summary>
        /// <param name="sliderPictureMapping"></param>
        public void InsertSliderPicture(SliderPictureMapping sliderPictureMapping)
        {
            if (sliderPictureMapping == null)
            {
                throw new ArgumentNullException(nameof(sliderPictureMapping));
            }

            _slidePictureMapRepository.Delete(sliderPictureMapping);
        }
        /// <summary>
        /// UpdateSliderPicture
        /// </summary>
        /// <param name="sliderPictureMapping"></param>
        public void UpdateSliderPicture(SliderPictureMapping sliderPictureMapping)
        {
            if (sliderPictureMapping == null)
            {
                throw new ArgumentNullException(nameof(sliderPictureMapping));
            }

            _slidePictureMapRepository.Delete(sliderPictureMapping);
        }
        /// <summary>
        /// SliderPictureMappings
        /// </summary>
        /// <param name="sliderId"></param>
        /// <returns></returns>
        public IList<SliderPictureMapping> SliderPictureMappings(int sliderId)
        {
            if (sliderId == 0)
                return new List<SliderPictureMapping>();
            var query = _slidePictureMapRepository.Table;
            query = query.Where(x => x.SliderId == sliderId);
            return query.ToList();


        }
    }
}