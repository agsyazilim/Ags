using System;
using System.Linq;
using System.Threading.Tasks;
using Ags.Data.Domain;
using Ags.Data.Domain.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoresController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public StoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Stores
        public async Task<IActionResult> Index()
        {
            AddPageHeader("Site Ayarları", "Site Listesi");
            AddBreadcrumb("Site Ayarları", "/Admin/Stores");
            return View(await _context.Stores.ToListAsync());
        }


        // GET: Admin/Stores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            AddPageHeader("Site Ayarları", "Site Detay");
            AddBreadcrumb("Site Ayarları", string.Format("/Admin/Stores/Detay/{0}", id));
            if (id == null)
            {
                return NotFound();
            }

            Store store = await _context.Stores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Admin/Stores/Create
        public IActionResult Create()
        {
            AddPageHeader("Site Ayarları", "Site Oluştur");
            AddBreadcrumb("Site Ayarları", string.Format("/Admin/Stores/Create"));

            return View();
        }

        // POST: Admin/Stores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Url,SslEnabled,Hosts,DisplayOrder,CompanyName,CompanyAddress,CompanyPhoneNumber,Id")] Store store)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(store);
                    await _context.SaveChangesAsync();
                    SuccessNotification("Başarıyla Oluşturuldu");
                    //AddPageAlerts(PageAlertType.Success, );
                }
                catch (Exception exception)
                {
                    //AddPageAlerts(PageAlertType.Error, exception.Message);
                    ErrorNotification(exception.Message);
                }
                return RedirectToAction(nameof(Index));
            }

            return View(store);
        }

        // GET: Admin/Stores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            AddPageHeader("Site Ayarları", "Site Düzenle");
            if (id == null)
            {
                //AddPageAlerts(PageAlertType.Info,"Düzenlenecek Satır Seçiniz");
                return NotFound();

            }

            Store store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                //AddPageAlerts(PageAlertType.Warning,"Düzenlenecek Bilgi Yok");
                return NotFound();
            }
            return View(store);
        }

        // POST: Admin/Stores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Url,SslEnabled,Hosts,DisplayOrder,CompanyName,CompanyAddress,CompanyPhoneNumber,Id")] Store store)
        {
            if (id != store.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                    SuccessNotification("Güncelleme Başarıyla Yapıldı.");
                    //AddPageAlerts(PageAlertType.Success,"Güncelleme Başarıyla Yapıldı.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.Id))
                    {
                        //AddPageAlerts(PageAlertType.Success,"Güncellenecekk Bir Satır Bulunamadı.");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // GET: Admin/Stores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            AddPageHeader("Site Ayarları", "Site Sil");
            if (id == null)
            {
                return NotFound();
            }

            Store store = await _context.Stores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Admin/Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Store store = await _context.Stores.FindAsync(id);
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
            //AddPageAlerts(PageAlertType.Success,"Satır Silinidi");
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
            return _context.Stores.Any(e => e.Id == id);
        }
    }
}
