using System.Collections.Generic;
using Ags.Data.Domain.Media;
using Ags.Services.Common;
using Ags.Services.Media;
using Ags.Web.Models.Media;

namespace Ags.Web.Factories
{
    public class SliderFactory:ISliderFactory
    {
        private readonly IPictureService _pictureService;
        private readonly ISliderService _sliderService;
        private readonly ISectionService _sectionService;

        public SliderFactory(IPictureService pictureService, ISliderService sliderService, ISectionService sectionService)
        {
            _pictureService = pictureService;
            _sliderService = sliderService;
            _sectionService = sectionService;
        }

        public SliderPictureModel PrePareSliderPictureModel(SliderPictureModel model, SliderPictureMapping sliderPicture)
        {
            if(sliderPicture==null)
                return new SliderPictureModel();
            model.PictureId = sliderPicture.PictureId;
            model.PictureTitle = sliderPicture.PictureTitle;
            model.SliderTitle = sliderPicture.Title;
            model.Url = sliderPicture.Url;
            model.Id = sliderPicture.Id;
            model.PictureModel = PreParePictureModel(sliderPicture.PictureId);

            return model;
        }

        public PictureModel PreParePictureModel(int pictureId)
        {
            if (pictureId == 0)
                return new PictureModel();
            var query = _pictureService.GetPictureById(pictureId);
            if(query==null)
                return new PictureModel();
            var model = new PictureModel
            {
                AlternateText = query.AltAttribute,
                Title = query.TitleAttribute,
                FullSizeImageUrl = _pictureService.GetPictureUrl(query.Id),
                ImageUrl = _pictureService.GetPictureUrl(query.Id, 1173),
                ThumbImageUrl = _pictureService.GetPictureUrl(query.Id, 400)
            };
            return model;
        }

        public List<SliderPictureModel> PrePareSliderPictureListModel(int sliderId)
        {
            if(sliderId==0)
                return new List<SliderPictureModel>();
            var query = _sliderService.SliderPictureMappings(sliderId);
            if(query==null)
                return new List<SliderPictureModel>();
            var model = new List<SliderPictureModel>();
            foreach (var mapping in query)
            {
                model.Add(PrePareSliderPictureModel(new SliderPictureModel(), mapping));
            }
            return model;

        }

        public SliderModel PrePareSliderModel(int sliderId)
        {
           if(sliderId==0)
               return new SliderModel();
            var query = _sliderService.GetBySliderId(sliderId);
            if(query==null)
                return new SliderModel();
            var model = new SliderModel();
            model.Name = query.Name;
            model.CreateDate = query.CreateDate;
            model.Id = query.Id;
            model.SliderPictureModels = PrePareSliderPictureListModel(model.Id);
            return model;
        }

        public SliderModel PrePareSliderSectionModel(int sectionId)
        {
            if(sectionId==0)
                return new SliderModel();
            var query = _sliderService.GetBySectionById(sectionId);
            if(query==null)
                return new SliderModel();
            var model = new SliderModel();
            model.Name = query.Name;
            model.CreateDate = query.CreateDate;
            model.Id = query.Id;
            model.SectionName = _sliderService.GetBySectionById(query.SectionId).Name;
            model.SliderPictureModels = PrePareSliderPictureListModel(model.Id);
            return model;
        }
    }
}