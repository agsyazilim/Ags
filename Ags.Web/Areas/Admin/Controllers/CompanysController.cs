using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ags.Data.Core.Repository;
using Ags.Data.Domain;
using Ags.Data.Domain.Catalog;
using Ags.Data.Domain.Directory;
using Ags.Services.Directory;
using Ags.Services.Events;
using Ags.Services.Seo;
using Ags.Web.Areas.Admin.Models.Companies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanysController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<CompanyCategory> _companyCategoryRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IStateProvinceService _stateProvinceService;

        public CompanysController(ApplicationDbContext context, IUrlRecordService urlRecordService, IEventPublisher eventPublisher, IRepository<CompanyCategory> companyCategoryRepository, IRepository<Company> companyRepository, IStateProvinceService stateProvinceService)
        {
            _context = context;
            _urlRecordService = urlRecordService;
            _eventPublisher = eventPublisher;
            _companyCategoryRepository = companyCategoryRepository;
            _companyRepository = companyRepository;
            _stateProvinceService = stateProvinceService;
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
        protected void PrepareStateProvinceList(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

           var availableEtitors =  _context.StateProvinces;
            foreach (StateProvince item in availableEtitors)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }
        // GET: Admin/Companies
        public async Task<IActionResult> Index()
        {
            var companies = await _context.Companies.Include(c => c.CompanyCategories).Include(c => c.Picture).ToListAsync();

            var model = new  List<CompanyModel>();
            foreach (Company company in companies)
            {
                CompanyModel compayModel = new CompanyModel
                {
                    Name = company.Name,
                    Id = company.Id,
                    Address = company.Address,
                    Description = company.Description,
                    EndDate = company.EndDate,
                    StartDate = company.StartDate,
                    Fax = company.Fax,
                    PictureId = company.PictureId,
                    Phone = company.Phone,
                    Gsm = company.Gsm,
                    Published = company.Published,
                    VideoEmbedCode = company.VideoEmbedCode,
                    Www = company.Www,
                    StateProvenceId = company.StateProvincesId,
                    CompanyCategoryId = company.CompanyCategoryId

                };
                PrepareCompanyList(compayModel.AvailableCatagories,true,"Katgeori Seçiniz");
                PrepareStateProvinceList(compayModel.AvailableStateProvince,true,"Şehir Şeçiniz");
                model.Add(compayModel);

            }


            return View(model);
        }
        // GET: Admin/Companies/Create
        public IActionResult Create()
        {
            CompanyModel model = new CompanyModel();
            PrepareCompanyList(model.AvailableCatagories, true, "Katgeori Seçiniz");
            PrepareStateProvinceList(model.AvailableStateProvince, true, "Şehir Şeçiniz");
            return View(model);
        }

        // POST: Admin/Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyModel model)
        {
            if (ModelState.IsValid)
            {

                Company company = new Company
                {
                    Name = model.Name,
                    Address = model.Address,
                    Description = model.Description,
                    Fax = model.Fax,
                    Gsm = model.Gsm,
                    Phone = model.Phone,
                    EndDate = model.EndDate,
                    StartDate = model.StartDate,
                    Published = model.Published,
                    VideoEmbedCode = model.VideoEmbedCode,
                    Www = model.Www,
                    PictureId = model.PictureId,
                    CompanyCategoryId = model.CompanyCategoryId,
                    StateProvincesId = model.StateProvenceId

                };

                await _context.AddAsync(company);
                await _context.SaveChangesAsync();
                string seName = _urlRecordService.ValidateSeName(company, model.SeName, model.Name, true);
                _urlRecordService.SaveSlug(company, seName);
                _eventPublisher.EntityInserted(company);
                SuccessNotification("Firma Eklendi.");
                return RedirectToAction(nameof(Index));
            }


            return View(model);
        }

        // GET: Admin/Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Company company = await _context.Companies.FindAsync(id);
            CompanyModel model = new CompanyModel
            {
                Name = company.Name,
                Id = company.Id,
                Address = company.Address,
                Description = company.Address,
                EndDate = company.EndDate,
                StartDate = company.StartDate,
                Fax = company.Fax,
                PictureId = company.PictureId,
                Phone = company.Phone,
                Gsm = company.Gsm,
                Published = company.Published,
                VideoEmbedCode = company.VideoEmbedCode,
                Www = company.Www,
                StateProvenceId = company.StateProvincesId,
                CompanyCategoryId = company.CompanyCategoryId
            };

            PrepareCompanyList(model.AvailableCatagories, true, "Katgeori Seçiniz");
            PrepareStateProvinceList(model.AvailableStateProvince, true, "Şehir Şeçiniz");


            if (model.Id == 0)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Admin/Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,CompanyModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Company company = new Company
                {
                    Name = model.Name,
                    Address = model.Address,
                    Description = model.Description,
                    Fax = model.Fax,
                    Gsm = model.Gsm,
                    Phone = model.Phone,
                    EndDate = model.EndDate,
                    StartDate = model.StartDate,
                    Published = model.Published,
                    VideoEmbedCode = model.VideoEmbedCode,
                    Www = model.Www,
                    PictureId = model.PictureId,
                    CompanyCategoryId = model.CompanyCategoryId,
                    StateProvincesId = model.StateProvenceId

                };
                try
                {
                    await _context.Companies.AddAsync(company);
                    await _context.SaveChangesAsync();
                    string seName = _urlRecordService.ValidateSeName(company, model.SeName, model.Name, true);
                    _urlRecordService.SaveSlug(company, seName);
                    _eventPublisher.EntityUpdated(company);
                    SuccessNotification("Firma Güncellendi");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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

        // GET: Admin/Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Company company = await _context.Companies.FindAsync(id);
            CompanyModel model = new CompanyModel
            {
                Name = company.Name,
                Id = company.Id,
                Address = company.Address,
                Description = company.Address,
                EndDate = company.EndDate,
                StartDate = company.StartDate,
                Fax = company.Fax,
                PictureId = company.PictureId,
                Phone = company.Phone,
                Gsm = company.Gsm,
                Published = company.Published,
                VideoEmbedCode = company.VideoEmbedCode,
                Www = company.Www,
                StateProvenceId = company.StateProvincesId,
                CompanyCategoryId = company.CompanyCategoryId
            };
            if (model.Id == 0)
            {
                return NotFound();
            }
            PrepareCompanyList(model.AvailableCatagories, true, "Katgeori Seçiniz");
            PrepareStateProvinceList(model.AvailableStateProvince, true, "Şehir Şeçiniz");
            return View(model);
        }

        // POST: Admin/Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Company company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            SuccessNotification("Kayıt Silindi");
            _eventPublisher.EntityDeleted(company);
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
