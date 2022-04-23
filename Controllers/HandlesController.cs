using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class HandlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HandlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Handles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Handles.ToListAsync());
        }

        // GET: Handles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handles = await _context.Handles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (handles == null)
            {
                return NotFound();
            }

            return View(handles);
        }

        // GET: Handles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Handles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Handles handles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(handles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(handles);
        }

        // GET: Handles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handles = await _context.Handles.FindAsync(id);
            if (handles == null)
            {
                return NotFound();
            }
            return View(handles);
        }

        // POST: Handles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Handles handles)
        {
            if (id != handles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(handles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HandlesExists(handles.Id))
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
            return View(handles);
        }

        // GET: Handles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handles = await _context.Handles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (handles == null)
            {
                return NotFound();
            }

            return View(handles);
        }

        // POST: Handles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var handles = await _context.Handles.FindAsync(id);
            _context.Handles.Remove(handles);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HandlesExists(int id)
        {
            return _context.Handles.Any(e => e.Id == id);
        }
    }
}
