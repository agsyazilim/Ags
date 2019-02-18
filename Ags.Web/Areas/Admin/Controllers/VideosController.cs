using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain;
using Ags.Data.Domain.Media;
using Ags.Web.Areas.Admin.Models.Videos;
using Ags.Web.Framework.Extensions;
using Ags.Web.Framework.Kendoui;
using Ags.Web.Framework.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Controllers
{

    public class VideosController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Video> _videoRepository;
        private readonly IRepository<VideoGallery> _videoGalleryRepository;

        public VideosController(ApplicationDbContext context, IRepository<VideoGallery> videoGalleryRepository, IRepository<Video> videoRepository)
        {
            _context = context;
            _videoGalleryRepository = videoGalleryRepository;
            _videoRepository = videoRepository;
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

            Microsoft.EntityFrameworkCore.DbSet<VideoGallery> availableEtitors = _context.VideoGalleries;
            foreach (VideoGallery item in availableEtitors)
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
            VideoKategoriSearchModel model = new VideoKategoriSearchModel();
            PrepareSliderList(model.AvailableVideoCategorys, true, "Kategori Seçin");
            model.SetGridPageSize();

            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            VideoGallery galery = _videoGalleryRepository.GetById(id);
            if (galery == null)
                return RedirectToAction("List");
            VideoKategoriModel model = new VideoKategoriModel
            {
                Id = galery.Id,
                Name = galery.Name,
                Published = galery.Published
            };
            model.VideoSearchModel.GaleryId = galery.Id;
            model.VideoSearchModel.SetGridPageSize();
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult GalleryList(VideoKategoriSearchModel searchModel)
        {

            IQueryable<VideoGallery> query = _videoGalleryRepository.Table;
            if (searchModel.SearchCategoriId != 0)
            {
                query = from g in query
                        where g.Id == searchModel.SearchCategoriId
                        select g;
            }

            List<VideoGallery> pagelists = query.ToList();
            PagedList<VideoGallery> pageList = new PagedList<VideoGallery>(pagelists, searchModel.Page - 1, searchModel.PageSize);
            VideoKategoriListModel model = new VideoKategoriListModel
            {
                Data = pageList.PaginationByRequestModel(searchModel).Select(x =>
                {
                    VideoKategoriModel galerModel = new VideoKategoriModel
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
        public IActionResult GalleryCreate(VideoKategoriModel model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                VideoGallery result = new VideoGallery
                {
                    Name = model.Name,
                    Published = model.Published
                };
                _videoGalleryRepository.Insert(result);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult GalleryEdit(VideoKategoriModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }
            VideoGallery result = _videoGalleryRepository.GetById(model.Id);
            if (result == null) return new NullJsonResult();
            result.Name = model.Name;
            result.Published = model.Published;
            _videoGalleryRepository.Update(result);
            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult GalleryDelete(int? id)
        {
            VideoGallery result = _videoGalleryRepository.GetById(id);
            if (result.Id > 0)
                _videoGalleryRepository.Delete(result);

            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult GaleriPictureList(int id)
        {

            VideoGallery galeri = _videoGalleryRepository.GetById(id);
            if (galeri == null)
                throw new ArgumentException("No product picture found with the specified id");
            List<Video> gridData = _videoRepository.Table.Where(x => x.VideoGalleryId == id).ToList();
            VideoListModel model = new VideoListModel
            {
                Data = gridData.Select(x =>
                {
                    VideoModel galeriPicturemodel = new VideoModel
                    {
                        Id = x.Id,
                        DisplayOrder = x.DisplayOrder,
                        Published = x.Published,
                        IsApproved = x.IsApproved,
                        EmbedCode = x.EmbedCode,
                        Descriptions = x.Descriptions,
                        VideoGalleryId = x.VideoGalleryId
                    };
                    return galeriPicturemodel;

                }),
                Total = gridData.Count()
            };

            return Json(model);
        }
        [HttpPost]
        public virtual IActionResult GaleriPictureUpdate(VideoModel model)
        {

            Video galeriPicture = _videoRepository.GetById(model.Id)
                                ?? throw new ArgumentException("No product picture found with the specified id");

            galeriPicture.Descriptions = model.Descriptions;
            galeriPicture.EmbedCode = model.EmbedCode;
            galeriPicture.IsApproved = model.IsApproved;
            galeriPicture.Published = model.Published;
            galeriPicture.DisplayOrder = model.DisplayOrder;
            _videoRepository.Update(galeriPicture);
            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult GaleriPictureDelete(int id)
        {
            //try to get a product picture with the specified id
            Video galeriPicture = _videoRepository.GetById(id)
                                 ?? throw new ArgumentException("No product picture found with the specified id");
            _videoRepository.Delete(galeriPicture);
            //try to get a picture with the specified id
            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult GaleriPictureAdd(int pictureId,string pictureUrl,int videoId, int displayOrder, string description, string embedcode, bool published, bool isaproved, int galeriId)
        {
            if (galeriId == 0)
                throw new ArgumentException();

            VideoGallery galeri = _videoGalleryRepository.GetById(galeriId)
                         ?? throw new ArgumentException("No product found with the specified id");
            Video addVideo = new Video
            {
                Descriptions = description,
                DisplayOrder = displayOrder,
                EmbedCode = embedcode,
                IsApproved = isaproved,
                Published = published,
                VideoGalleryId = galeriId,
                PictureId = pictureId,
                PictureUrl = pictureUrl
            };
            _videoRepository.Insert(addVideo);

            return Json(new { Result = true });
        }
    }
}
