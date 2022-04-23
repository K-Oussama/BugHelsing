using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var v = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var project = _context.Handles.Where(c => c.Usr.Id == v).Select(c => c.Proj);
            return View(await project.ToListAsync());
        }
        public async Task<IActionResult> Table()
        {
            return View(await _context.Projects.ToListAsync());
        }
        public async Task<IActionResult> TableC()
        {
            return View(await _context.Projects.ToListAsync());
        }
        // Modal


        // GET: Projects/Details/5
        [Authorize]
        public async Task<PartialViewResult> Details(int? Number)
        {

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == Number);

            return PartialView("Details", project);
        }

        [Authorize]
        public async Task<IActionResult> DetailsPage(int? id)
        {

            var project = await _context.Projects.FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.projetname = project.Name;
            return View("DetailsPage", project);
        }

        // GET: Projects/Create
        [Authorize]
        [HttpGet]
        public PartialViewResult Create()
        {
            return PartialView("Create");
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreatedOn,CreatedBy")] Project project)
        {
            if (ModelState.IsValid)
            {
                Handles hndl = new();
                var currentUsr = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                hndl.Usr = currentUsr;
                hndl.Proj = project;
                _context.Add(hndl);
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }



        // GET: Projects/Edit/5
        [Authorize]
        [HttpGet]
        public async Task<PartialViewResult> Edit(int? Number)
        {
            var project = await _context.Projects.FindAsync(Number);
  
            return PartialView("Edit", project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedOn,CreatedBy")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize]
        public async Task<PartialViewResult> Delete(int? Number)
        {

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == Number);

            return PartialView("Delete", project);
        }

        // POST: Projects/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var project = await _context.Projects.FindAsync(id);

            // Handle
            var currentUsr = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var handle1 = _context.Handles.Where(c => c.Usr == currentUsr).Select(c => c);
            var handle2 = handle1.Where(c => c.Proj == project).Select(c => c.Id);
            var hndl = await _context.Handles.FindAsync(handle2.First());
            _context.Handles.Remove(hndl);

            // bUG + bug detail
            var ticket0 = _context.Ticket.Where(t => t.Project.Id == project.Id).Select(t => t.Id);

            foreach (var Tick in ticket0)
            {
                var ticketd0 = _context.TicketDetail.Where(t => t.Ticket.Id == Tick).Select(t => t.Id);
                foreach(var Tickd in ticketd0)
                {
                    var ticketd1 = await _context.TicketDetail.FindAsync(Tickd);
                    _context.TicketDetail.Remove(ticketd1);
                }
                var ticket1 = await _context.Ticket.FindAsync(Tick);
                _context.Ticket.Remove(ticket1);
            }

            // board + column + card
            var board0 = _context.Boards.Where(t => t.Project.Id == project.Id).Select(t => t.Id);
            foreach (var bor in board0)
            {
                var colum = _context.Columns.Where(t => t.BoardId == bor).Select(t => t.Id);
                foreach (var colm in colum)
                {
                    var card0 = _context.Cards.Where(t => t.ColumnId == colm).Select(t => t.Id);
                    foreach (var crd in card0) 
                    {
                        var card1 = await _context.Cards.FindAsync(crd);
                        _context.Cards.Remove(card1);
                    }
                    var clm = await _context.Columns.FindAsync(colm);
                    _context.Columns.Remove(clm);
                }
                var board1 = await _context.Boards.FindAsync(bor);
                _context.Boards.Remove(board1);
            }

            // Projet
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
