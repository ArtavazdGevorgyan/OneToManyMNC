using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneToManyMVC.Models;

namespace OneToManyMVC.Controllers
{
    public class AmbionsController : Controller
    {
        private readonly OneToManyMVCContext _context;

        public AmbionsController(OneToManyMVCContext context)
        {
            _context = context;
        }

        // GET: Ambions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ambion.ToListAsync());
        }

        // GET: Ambions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ambion = await _context.Ambion
                .FirstOrDefaultAsync(m => m.AmbionId == id);
            if (ambion == null)
            {
                return NotFound();
            }

            return View(ambion);
        }

        // GET: Ambions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ambions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AmbionId,AmbionName")] Ambion ambion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ambion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ambion);
        }

        // GET: Ambions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ambion = await _context.Ambion.FindAsync(id);
            if (ambion == null)
            {
                return NotFound();
            }
            return View(ambion);
        }

        // POST: Ambions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AmbionId,AmbionName")] Ambion ambion)
        {
            if (id != ambion.AmbionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ambion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmbionExists(ambion.AmbionId))
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
            return View(ambion);
        }

        // GET: Ambions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _context.Grade
                .FirstOrDefaultAsync(m => m.GradeId == id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // POST: Ambions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ambion = await _context.Ambion.FindAsync(id);
            _context.Ambion.Remove(ambion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmbionExists(int id)
        {
            return _context.Ambion.Any(e => e.AmbionId == id);
        }
    }
}
