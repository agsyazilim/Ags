using System.Linq;
using System.Threading.Tasks;
using Ags.Data.Domain;
using Ags.Data.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModulesController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public ModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Modules
        public async Task<IActionResult> Index()
        {
            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name");
            return View(await _context.Moduleses.Include(x=>x.Section).ToListAsync());
        }



        // GET: Admin/Modules/Create
        public IActionResult Create()
        {
             ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name");
            return View();
        }

        // POST: Admin/Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Published,SectionId,Id")] Modules modules)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modules);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modules);
        }

        // GET: Admin/Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Modules modules = await _context.Moduleses.FindAsync(id);
            if (modules == null)
            {
                return NotFound();
            }
             ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name");
            return View(modules);
        }

        // POST: Admin/Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Published,SectionId,Id")] Modules modules)
        {
            if (id != modules.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modules);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModulesExists(modules.Id))
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
            return View(modules);
        }

        // GET: Admin/Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Modules modules = await _context.Moduleses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modules == null)
            {
                return NotFound();
            }

            return View(modules);
        }

        // POST: Admin/Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Modules modules = await _context.Moduleses.FindAsync(id);
            _context.Moduleses.Remove(modules);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModulesExists(int id)
        {
            return _context.Moduleses.Any(e => e.Id == id);
        }
    }
}
