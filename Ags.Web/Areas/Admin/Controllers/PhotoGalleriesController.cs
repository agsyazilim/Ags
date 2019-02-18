using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain;
using Ags.Data.Domain.Media;
using Ags.Services.Configuration;
using Ags.Services.Media;
using Ags.Web.Areas.Admin.Models.Media.Galery;
using Ags.Web.Framework.Extensions;
using Ags.Web.Framework.Kendoui;
using Ags.Web.Framework.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Controllers
{

    public class PhotoGalleriesController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<PhotoGallery> _photoGalleryRepository;
        private readonly IRepository<PhotoGalleryMapping> _photoGalleryMapRepository;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;

        public PhotoGalleriesController(ApplicationDbContext context, IRepository<PhotoGallery> photoGalleryRepository, IRepository<PhotoGalleryMapping> photoGalleryMapRepository, IPictureService pictureService, ISettingService settingService)
        {
            _context = context;
            _photoGalleryRepository = photoGalleryRepository;
            _photoGalleryMapRepository = photoGalleryMapRepository;
            _pictureService = pictureService;
            _settingService = settingService;
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
        protected void PrepareGalleriesList(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Microsoft.EntityFrameworkCore.DbSet<PhotoGallery> availableEtitors = _context.PhotoGalleries;
            foreach (PhotoGallery item in availableEtitors)
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
            PhotoGallerySearchModel model = new PhotoGallerySearchModel();
            PrepareGalleriesList(model.AvailableGalleries, true, "Kategori Seçin");
            model.SetGridPageSize();

            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            PhotoGallery galery = _photoGalleryRepository.GetById(id);
            if (galery == null)
                return RedirectToAction("List");
            PhotoGalleryModel model = new PhotoGalleryModel
            {
                Id = galery.Id,
                Name = galery.Name,
                Published = galery.Published,

            };
            model.GalleryPictureSearchModel.GaleriId = galery.Id;
            model.GalleryPictureSearchModel.SetGridPageSize();
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult GalleryList(PhotoGallerySearchModel searchModel)
        {

            IQueryable<PhotoGallery> query = _photoGalleryRepository.Table;
            if (searchModel.SearchGalleryId != 0)
            {
                query = from g in query
                        join m in _photoGalleryMapRepository.Table on g.Id equals m.GalleryId
                        where m.GalleryId == searchModel.SearchGalleryId
                        select g;
            }

            if (!string.IsNullOrEmpty(searchModel.SearchName))
            {
                query = query.Where(x => x.Name.Contains(searchModel.SearchName));
            }

            List<PhotoGallery> pagelists = query.ToList();
            PagedList<PhotoGallery> pageList = new PagedList<PhotoGallery>(pagelists, searchModel.Page - 1, searchModel.PageSize);
            PhotoGalleryListModel model = new PhotoGalleryListModel
            {
                Data = pageList.PaginationByRequestModel(searchModel).Select(x =>
                {
                    PhotoGalleryModel galerModel = new PhotoGalleryModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        DisplayOrder = x.DisplayOrder,
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
        public IActionResult GalleryCreate(PhotoGalleryModel model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                PhotoGallery result = new PhotoGallery
                {
                    Name = model.Name,
                    Id = model.Id,
                    Published = model.Published,
                    DisplayOrder = model.DisplayOrder
                };
                _photoGalleryRepository.Insert(result);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult GalleryEdit(PhotoGalleryModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }
            PhotoGallery result = _photoGalleryRepository.GetById(model.Id);
            if (result == null) return new NullJsonResult();
            result.Name = model.Name;
            result.DisplayOrder = model.DisplayOrder;
            result.Published = model.Published;
            _photoGalleryRepository.Update(result);
            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult GalleryDelete(int? id)
        {
            PhotoGallery result = _photoGalleryRepository.GetById(id);
            if (result.Id > 0)
                _photoGalleryRepository.Delete(result);

            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult GaleriPictureList(int id)
        {

            PhotoGallery galeri = _photoGalleryRepository.GetById(id);
            if (galeri == null)
                throw new ArgumentException("No product picture found with the specified id");
            IOrderedQueryable<PhotoGalleryMapping> query = from pg in _context.PhotoGalleryMappings
                        where pg.GalleryId == galeri.Id
                        orderby pg.DisplayOrder, pg.Id
                        select pg;

            List<PhotoGalleryMapping> gridData = query.ToList();
            GalleryPictureListModel model = new GalleryPictureListModel
            {
                Data = gridData.Select(x =>
                {
                    GalleryPictureModel galeriPicturemodel = new GalleryPictureModel
                    {
                        Id = x.Id,
                        DisplayOrder = x.DisplayOrder,
                        GaleriId = x.GalleryId,
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
        [HttpPost]
        public virtual IActionResult GaleriPictureUpdate(GalleryPictureModel model)
        {

            PhotoGalleryMapping galeriPicture = _photoGalleryMapRepository.GetById(model.Id)
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
            _photoGalleryMapRepository.Update(galeriPicture);
            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult GaleriPictureDelete(int id)
        {
            //try to get a product picture with the specified id
            PhotoGalleryMapping galeriPicture = _photoGalleryMapRepository.GetById(id)
                                 ?? throw new ArgumentException("No product picture found with the specified id");
            int pictureId = galeriPicture.PictureId;
            _photoGalleryMapRepository.Delete(galeriPicture);
            //try to get a picture with the specified id
            Picture picture = _pictureService.GetPictureById(pictureId)
                          ?? throw new ArgumentException("No picture found with the specified id");
            _pictureService.DeletePicture(picture);
            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult GaleriPictureAdd(int pictureId, int displayOrder,
            string overrideAltAttribute, string overrideTitleAttribute, string url, int galeriId)
        {
            if (pictureId == 0)
                throw new ArgumentException();

            PhotoGallery galeri = _photoGalleryRepository.GetById(galeriId)
                         ?? throw new ArgumentException("No product found with the specified id");
            IOrderedQueryable<PhotoGalleryMapping> query = from pg in _photoGalleryMapRepository.Table
                        where pg.GalleryId == galeriId
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
            _photoGalleryMapRepository.Insert(new PhotoGalleryMapping
            {
                GalleryId = galeriId,
                DisplayOrder = displayOrder,
                PictureId = pictureId,
                Url = url
            });

            return Json(new { Result = true });
        }
    }
}
