using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ags.Data.Domain;
using Ags.Data.Domain.Common;
using Ags.Services.Media;
using Ags.Web.Areas.Admin.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertisementsController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;
        private readonly IPictureService _pictureService;

        public AdvertisementsController(ApplicationDbContext context, IPictureService pictureService)
        {
            _context = context;
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
        protected void PrepareCompanyList(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            DbSet<Section> availableEtitors = _context.Sections;
            foreach (Section item in availableEtitors)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }
        // GET: Admin/Advertisements
        public async Task<IActionResult> Index()
        {
            List<AdvertisementModel> model = new List<AdvertisementModel>();
            List<Advertisement> query = await _context.Advertisements.ToListAsync();
            foreach (Advertisement item in query)
            {
                AdvertisementModel adv = new AdvertisementModel
                {
                    CodeFlash = item.CodeFlash,
                    CreateDate = item.CreateDate,
                    EndDate = item.EndDate,
                    FlashCode = item.FlashCode,
                    IsApproved = item.IsApproved,
                    PictureId = item.PictureId,
                    StartDate = item.StartDate,
                    TargetId = item.TargetId,
                    UrlAddress = item.UrlAddress,
                    Id = item.Id,
                    SectionId = item.SectionId

                };
                if (query.Count > 0)
                {
                    adv.PictureUrl = adv.PictureId == 0 ? _pictureService.GetDefaultPictureUrl(100) : _pictureService.GetPictureUrl(_pictureService.GetPictureById(item.PictureId), 100);
                }

                PrepareCompanyList(adv.AvailableSections,true,"Reklemın Çıkacağı yeri Seçiniz");
                model.Add(adv);
            }
            return View(model);
        }


        // GET: Admin/Advertisements/Create
        public IActionResult Create()
        {
            AdvertisementModel model = new AdvertisementModel();
            PrepareCompanyList(model.AvailableSections, true, "Reklemın Çıkacağı yeri Seçiniz");

            return View(model);
        }

        // POST: Admin/Advertisements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdvertisementModel model)
        {
            if (ModelState.IsValid)
            {
                Advertisement advertisement = new Advertisement
                {
                    CodeFlash = model.CodeFlash,
                    CreateDate = DateTime.Now,
                    EndDate = model.EndDate,
                    FlashCode = model.FlashCode,
                    IsApproved = model.IsApproved,
                    PictureId = model.PictureId,
                    SectionId = model.SectionId,
                    StartDate = model.StartDate,
                    TargetId = model.TargetId,
                    UrlAddress = model.UrlAddress
                };
                _context.Add(advertisement);
                await _context.SaveChangesAsync();
                SuccessNotification("Kayıt Eklendi");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/Advertisements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Advertisement advertisement = await _context.Advertisements.FindAsync(id);
            AdvertisementModel model = new AdvertisementModel
            {
                Id = advertisement.Id,
                StartDate = advertisement.StartDate,
                EndDate = advertisement.EndDate,
                PictureId = advertisement.PictureId,
                CodeFlash = advertisement.CodeFlash,
                CreateDate = advertisement.CreateDate,
                SectionId = advertisement.SectionId,
                FlashCode = advertisement.FlashCode,
                IsApproved = advertisement.IsApproved,
                TargetId = advertisement.TargetId,
                UrlAddress = advertisement.UrlAddress

            };
            PrepareCompanyList(model.AvailableSections);

            if (model.Id == 0)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Admin/Advertisements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdvertisementModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Advertisement advertisement = new Advertisement
                    {
                        Id = model.Id,
                        CodeFlash = model.CodeFlash,
                        CreateDate = DateTime.Now,
                        EndDate = model.EndDate,
                        FlashCode = model.FlashCode,
                        IsApproved = model.IsApproved,
                        PictureId = model.PictureId,
                        SectionId = model.SectionId,
                        StartDate = model.StartDate,
                        TargetId = model.TargetId,
                        UrlAddress = model.UrlAddress
                    };

                    _context.Update(advertisement);
                    await _context.SaveChangesAsync();
                    SuccessNotification("Kayıt Güncellendi");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertisementExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/Advertisements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Advertisement advertisement = await _context.Advertisements
                .FirstOrDefaultAsync(m => m.Id == id);
            AdvertisementModel model = new AdvertisementModel
            {
                Id = advertisement.Id,
                StartDate = advertisement.StartDate,
                EndDate = advertisement.EndDate,
                PictureId = advertisement.PictureId,
                CodeFlash = advertisement.CodeFlash,
                CreateDate = advertisement.CreateDate,
                SectionId = advertisement.SectionId,
                FlashCode = advertisement.FlashCode,
                IsApproved = advertisement.IsApproved,
                TargetId = advertisement.TargetId,
                UrlAddress = advertisement.UrlAddress

            };
            PrepareCompanyList(model.AvailableSections);
            if (model.Id == 0)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Admin/Advertisements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Advertisement advertisement = await _context.Advertisements.FindAsync(id);
            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();
            SuccessNotification("Kayıt Silindi");
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertisementExists(int id)
        {
            return _context.Advertisements.Any(e => e.Id == id);
        }
    }
}
