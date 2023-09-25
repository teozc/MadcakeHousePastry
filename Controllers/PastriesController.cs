using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MadcakeHousePastry.Data;
using MadcakeHousePastry.Models;

namespace MadcakeHousePastry.Views.Pastry
{
    public class PastriesController : Controller
    {
        private readonly MadcakeHousePastryContext _context;

        public PastriesController(MadcakeHousePastryContext context)
        {
            _context = context;
        }

        // GET: Pastries
        public async Task<IActionResult> Index(string SearchPastryName, string PastryType)
        {
            IQueryable<string> query = from m in _context.Pastry
                                       orderby m.PastryType
                                       select m.PastryType;
            IEnumerable<SelectListItem> items = new SelectList(await query.Distinct().ToListAsync());
            ViewBag.PastryType = items;
            var pastry = from m in _context.Pastry
                         select m;
            if (!string.IsNullOrEmpty(SearchPastryName))
            {
                pastry = pastry.Where(s => s.PastryName.Contains(SearchPastryName));
            }
            if (!string.IsNullOrEmpty(PastryType))
            {
                pastry = pastry.Where(s => s.PastryType.Equals(PastryType));
            }
            return View(await pastry.ToListAsync());
        }

        // GET: Pastries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastry = await _context.Pastry
                .FirstOrDefaultAsync(m => m.PastryID == id);
            if (pastry == null)
            {
                return NotFound();
            }

            return View(pastry);
        }

        // GET: Pastries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pastries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PastryID,PastryName,PastryProducedDate,PastryType,PastryPrice")] Models.Pastry pastry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pastry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pastry);
        }

        // GET: Pastries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastry = await _context.Pastry.FindAsync(id);
            if (pastry == null)
            {
                return NotFound();
            }
            return View(pastry);
        }

        // POST: Pastries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PastryID,PastryName,PastryProducedDate,PastryType,PastryPrice")] Models.Pastry pastry)
        {
            if (id != pastry.PastryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pastry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PastryExists(pastry.PastryID))
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
            return View(pastry);
        }

        // GET: Pastries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastry = await _context.Pastry
                .FirstOrDefaultAsync(m => m.PastryID == id);
            if (pastry == null)
            {
                return NotFound();
            }

            return View(pastry);
        }

        // POST: Pastries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pastry = await _context.Pastry.FindAsync(id);
            _context.Pastry.Remove(pastry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PastryExists(int id)
        {
            return _context.Pastry.Any(e => e.PastryID == id);
        }
    }
}
