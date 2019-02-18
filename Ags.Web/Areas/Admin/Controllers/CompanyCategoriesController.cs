using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.Areas.Admin.Factories;
using Ags.Data.Domain;
using Ags.Data.Domain.Catalog;
using Ags.Services.Events;
using Ags.Services.Seo;
using Ags.Web.Areas.Admin.Models.Companies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyCategoriesController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IEventPublisher _eventPublisher;

        public CompanyCategoriesController(ApplicationDbContext context, IBaseAdminModelFactory baseAdminModelFactory, IUrlRecordService urlRecordService, IEventPublisher eventPublisher)
        {
            _context = context;
            _urlRecordService = urlRecordService;
            _eventPublisher = eventPublisher;
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

            DbSet<CompanyCategory> availableEtitors = _context.CompanyCategories;
            foreach (CompanyCategory item in availableEtitors)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        // GET: Admin/CompanyCategories
        public async Task<IActionResult> Index()
        {
            List<CompanyCategory> category = await _context.CompanyCategories.ToListAsync();
            List<CompanyCategoryModel> model = new List<CompanyCategoryModel>();
            foreach (CompanyCategory item in category)
            {
                CompanyCategoryModel copmcatmodel = new CompanyCategoryModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreateDate = item.CreateDate,
                    EndDate = item.EndDate,
                    StartDate = item.StartDate,
                    ParentId = item.ParentId,
                    Published = item.Published
                };
                PrepareCompanyList(copmcatmodel.AvailableCategories,true,"Kategori Seç");
                model.Add(copmcatmodel);
            }
            return View(model);
        }

        // GET: Admin/CompanyCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CompanyCategory item = await _context.CompanyCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            CompanyCategoryModel companyCategory = new CompanyCategoryModel
            {
                Id = item.Id,
                Name = item.Name,
                CreateDate = item.CreateDate,
                EndDate = item.EndDate,
                StartDate = item.StartDate,
                ParentId = item.ParentId,
                Published = item.Published
            };
            PrepareCompanyList(companyCategory.AvailableCategories, true, "Kategori Seç");

            if (companyCategory.Id == 0)
            {
                ErrorNotification($"Kayıt Bulunamadı,{id}");
            }


            return View(companyCategory);
        }

        // GET: Admin/CompanyCategories/Create
        public IActionResult Create()
        {
            CompanyCategoryModel model = new CompanyCategoryModel();
            PrepareCompanyList(model.AvailableCategories, true, "Kategori Seç");
            return View(model);
        }

        // POST: Admin/CompanyCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                CompanyCategory companyCategory = new CompanyCategory
                {
                    Name = model.Name,
                    ParentId = model.ParentId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Published = model.Published
                };
                string seName =_urlRecordService.ValidateSeName(companyCategory, model.SeName, model.Name, true);
                _urlRecordService.SaveSlug(companyCategory,seName);
                _context.Add(companyCategory);
                _eventPublisher.EntityInserted(companyCategory);
                try
                {
                    await _context.SaveChangesAsync();
                    SuccessNotification("Kategori Oluşturuldu");
                }
                catch (Exception e)
                {
                    ErrorNotification($"Hata oluştu {e.Message}");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/CompanyCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CompanyCategory item = await _context.CompanyCategories.FindAsync(id);
            CompanyCategoryModel model = new CompanyCategoryModel
            {
                Id = item.Id,
                Name = item.Name,
                CreateDate = item.CreateDate,
                EndDate = item.EndDate,
                StartDate = item.StartDate,
                ParentId = item.ParentId,
                Published = item.Published
            };
            PrepareCompanyList(model.AvailableCategories, true, "Kategori Seç");

            if (model.Id==0)
            {
                WarningNotification($"{id} , Kayıt Bulunamadı");
            }
            return View(model);
        }

        // POST: Admin/CompanyCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyCategoryModel model,int id)
        {
            CompanyCategory updateItem = _context.CompanyCategories.Find(model.Id);
            if (updateItem.Id==0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    updateItem.Name = model.Name;
                    updateItem.ParentId = model.ParentId;
                    updateItem.UpdateDate = DateTime.Now;
                    updateItem.StartDate = model.StartDate;
                    updateItem.EndDate = model.EndDate;
                    updateItem.Published = model.Published;
                    _context.Update(updateItem);
                    await _context.SaveChangesAsync();

                    string seName = _urlRecordService.ValidateSeName(updateItem, model.SeName, model.Name, true);
                    _urlRecordService.SaveSlug(updateItem, seName);
                    _eventPublisher.EntityUpdated(updateItem);
                    SuccessNotification("Kayıt Güncelendi");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyCategoryExists(updateItem.Id))
                    {
                       WarningNotification($"{id} , Kayıt Bulunamadı");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Admin/CompanyCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                WarningNotification($"Kayıt Bulunamadı, {id.ToString()} ");
            }

            CompanyCategory companyCategory = await _context.CompanyCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyCategory == null)
            {
               WarningNotification($"{id} , Kayıt Bulunamadı");
            }

            return View(companyCategory);
        }

        // POST: Admin/CompanyCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CompanyCategory companyCategory = await _context.CompanyCategories.FindAsync(id);
            _context.CompanyCategories.Remove(companyCategory);
            await _context.SaveChangesAsync();
            SuccessNotification("Kayıt Silindi");
            _eventPublisher.EntityDeleted(companyCategory);
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyCategoryExists(int id)
        {
            return _context.CompanyCategories.Any(e => e.Id == id);
        }
    }
}
