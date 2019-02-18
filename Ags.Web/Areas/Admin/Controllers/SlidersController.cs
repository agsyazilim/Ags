using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain;
using Ags.Data.Domain.Media;
using Ags.Services.Media;
using Ags.Web.Areas.Admin.Models.Media.Galery;
using Ags.Web.Areas.Admin.Models.Media.Slider;
using Ags.Web.Framework.Extensions;
using Ags.Web.Framework.Kendoui;
using Ags.Web.Framework.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Controllers
{

    public class SlidersController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Slider> _sliderRepository;
        private readonly IRepository<SliderPictureMapping> _sliderPicturRepository;
        private readonly IPictureService _pictureService;

        public SlidersController(ApplicationDbContext context, IRepository<Slider> sliderRepository, IRepository<SliderPictureMapping> sliderPicturRepository, IPictureService pictureService)
        {
            _context = context;
            _sliderRepository = sliderRepository;
            _sliderPicturRepository = sliderPicturRepository;
            _pictureService = pictureService;
        }

        protected virtual void PrepareDefaultItem(IList<SelectListItem> items, bool withSpecialDefaultItem, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //whether to insert the first special item for the default value
            if (!withSpecialDefaultItem)
                return;

            //at now we use "0" as the default value
            const string value = "0";

            //prepare item text
            defaultItemText = defaultItemText ?? "Hepsi";

            //insert this default item at first
            items.Insert(0, new SelectListItem { Text = defaultItemText, Value = value });
        }
        protected void PrepareSliderList(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Microsoft.EntityFrameworkCore.DbSet<Slider> availableEtitors = _context.Sliders;
            foreach (Slider item in availableEtitors)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }
        // GET: Admin/PhotoGalleries
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            SliderSearchModel model = new SliderSearchModel();
            PrepareSliderList(model.AvailableGalleries, true, "Kategori Seçin");
            model.SetGridPageSize();

            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            Slider galery = _sliderRepository.GetById(id);
            if (galery == null)
                return RedirectToAction("List");
            SliderModel model = new SliderModel
            {
                Id = galery.Id,
                Name = galery.Name,
                Published = galery.Published,

            };
            model.SliderSearchModel.SearchGalleryId = galery.Id;
            model.SliderSearchModel.SetGridPageSize();
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult GalleryList(SliderSearchModel searchModel)
        {

            IQueryable<Slider> query = _sliderRepository.Table;
            if (searchModel.SearchGalleryId != 0)
            {
                query = from g in query
                        join m in _sliderPicturRepository.Table on g.Id equals m.SliderId
                        where m.SliderId == searchModel.SearchGalleryId
                        select g;
            }

            if (!string.IsNullOrEmpty(searchModel.SearchName))
            {
                query = query.Where(x => x.Name.Contains(searchModel.SearchName));
            }

            List<Slider> pagelists = query.ToList();
            PagedList<Slider> pageList = new PagedList<Slider>(pagelists, searchModel.Page - 1, searchModel.PageSize);
            SliderListModel model = new SliderListModel
            {
                Data = pageList.PaginationByRequestModel(searchModel).Select(x =>
                {
                    SliderModel galerModel = new SliderModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Published = x.Published
                    };
                    return galerModel;
                }),
                Total = pageList.Count

            };
            return Json(model);
        }

        // GET: Admin/PhotoGalleries/Create
        [HttpPost]
        public IActionResult GalleryCreate(SliderModel model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                Slider result = new Slider
                {
                    Name = model.Name,
                    Id = model.Id,
                    Published = model.Published,
                    CreateDate = DateTime.Now,
                    SectionId = model.SectionId

                };
                _sliderRepository.Insert(result);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult GalleryEdit(SliderModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }
            Slider result = _sliderRepository.GetById(model.Id);
            if (result == null) return new NullJsonResult();
            result.Name = model.Name;
            result.Published = model.Published;
            _sliderRepository.Update(result);
            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult GalleryDelete(int? id)
        {
            Slider result = _sliderRepository.GetById(id);
            if (result.Id > 0)
                _sliderRepository.Delete(result);

            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult GaleriPictureList(int id)
        {

            Slider galeri = _sliderRepository.GetById(id);
            if (galeri == null)
                throw new ArgumentException("No product picture found with the specified id");
            IOrderedQueryable<SliderPictureMapping> query = from pg in _sliderPicturRepository.Table
                        where pg.SliderId == galeri.Id
                        orderby pg.DisplayOrder, pg.Id
                        select pg;

            List<SliderPictureMapping> gridData = query.ToList();
            GalleryPictureListModel model = new GalleryPictureListModel
            {
                Data = gridData.Select(x =>
                {
                    GalleryPictureModel galeriPicturemodel = new GalleryPictureModel
                    {
                        Id = x.Id,
                        DisplayOrder = x.DisplayOrder,
                        GaleriId = x.SliderId,
                        PictureId = x.PictureId
                    };
                    Picture picture = _pictureService.GetPictureById(galeriPicturemodel.PictureId) ??
                                  throw new Exception("Resim Yok");
                    galeriPicturemodel.OverrideAltAttribute = picture.AltAttribute;
                    galeriPicturemodel.OverrideTitleAttribute = picture.TitleAttribute;
                    galeriPicturemodel.PictureUrl = _pictureService.GetPictureUrl(picture);
                    return galeriPicturemodel;

                }),
                Total = query.Count()
            };

            return Json(model);
        }

        public virtual IActionResult GaleriPictureUpdate(GalleryPictureModel model)
        {

            SliderPictureMapping galeriPicture = _sliderPicturRepository.GetById(model.Id)
                                ?? throw new ArgumentException("No product picture found with the specified id");

            Picture picture = _pictureService.GetPictureById(galeriPicture.PictureId)
                          ?? throw new ArgumentException("No picture found with the specified id");

            _pictureService.UpdatePicture(picture.Id,
                _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                model.OverrideAltAttribute,
                model.OverrideTitleAttribute);

            galeriPicture.DisplayOrder = model.DisplayOrder;
            _sliderPicturRepository.Update(galeriPicture);
            return new NullJsonResult();
        }

        public virtual IActionResult GaleriPictureDelete(int id)
        {
            //try to get a product picture with the specified id
            SliderPictureMapping galeriPicture = _sliderPicturRepository.GetById(id)
                                 ?? throw new ArgumentException("No product picture found with the specified id");
            int pictureId = galeriPicture.PictureId;
            _sliderPicturRepository.Delete(galeriPicture);
            //try to get a picture with the specified id
            Picture picture = _pictureService.GetPictureById(pictureId)
                          ?? throw new ArgumentException("No picture found with the specified id");
            _pictureService.DeletePicture(picture);
            return new NullJsonResult();
        }

        public virtual IActionResult GaleriPictureAdd(int pictureId, int displayOrder,
            string overrideAltAttribute, string overrideTitleAttribute, string url, int galeriId)
        {
            if (pictureId == 0)
                throw new ArgumentException();

            Slider galeri = _sliderRepository.GetById(galeriId)
                         ?? throw new ArgumentException("No product found with the specified id");
            IOrderedQueryable<SliderPictureMapping> query = from pg in _sliderPicturRepository.Table
                        where pg.SliderId == galeriId
                        orderby pg.DisplayOrder, pg.Id
                        select pg;
            if (query.Any(p => p.PictureId == pictureId))
                return Json(new { Result = false });
            Picture picture = _pictureService.GetPictureById(pictureId)
                          ?? throw new ArgumentNullException("No Picture found with th spec id");
            _pictureService.UpdatePicture(picture.Id,
                                        _pictureService.LoadPictureBinary(picture),
                                        picture.MimeType,
                                        picture.SeoFilename,
                                        overrideAltAttribute,
                                        overrideTitleAttribute
                                    );
            _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(galeri.Name));
            _sliderPicturRepository.Insert(new SliderPictureMapping
            {
                SliderId = galeriId,
                DisplayOrder = displayOrder,
                PictureId = pictureId,
                Url = url,
                PictureTitle = overrideTitleAttribute,
                Title = overrideAltAttribute
            });

            return Json(new { Result = true });
        }
    }
}
