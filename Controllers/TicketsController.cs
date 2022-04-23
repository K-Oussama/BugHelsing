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
using System.Security.Claims;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var v = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var project = _context.Handles.Where(c => c.Usr.Id == v).Select(c => c.Proj);
            Task<List<Ticket>> toc = _context.Ticket.ToListAsync();
            var toctoc = from t in await toc
                      join p in project on t.Project equals p into table1
                      from p in table1.ToList()
                      select new Ticket
                      {
                          Id = t.Id,
                          Name = t.Name,
                          Description = t.Description,
                          CreatedOn = t.CreatedOn,
                          CreatedBy = t.CreatedBy,
                          Project = t.Project
                      };

            return View(toctoc.ToList());
        }

        // GET: Projects/Tick/5
        [Authorize]
        public async Task<IActionResult> Tick(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ID = id;



            var ticket = await _context.Ticket
            .Where(b => b.Project.Id == id)
            .ToListAsync();

            var project = _context.Projects.Find(id);
            if (project == null)
            {
                ViewBag.projetname = "ce projet";
            }
            else { ViewBag.boardname = project.Name; }

            return View(ticket);
        }


        // GET: Tickets/Details/5
        [Authorize]
        public async Task<PartialViewResult> Details(int? Number)
        {

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == Number);

            return PartialView(ticket);
        }

        // GET: Tickets/Create
        [Authorize]
        [HttpGet]
        public PartialViewResult Create(int? Number)
        {
            ViewBag.pro = Number;
            return PartialView("Create");
        }


        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int Project, [Bind("Id,Name,Description,CreatedOn,CreatedBy")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //var projectRecord = _context.Projects.FirstOrDefault(p => p.Id == Number);
                //ticket.Project.Id = ViewBag.pro; //projectRecord.Id;
                // post action in controller with int parameters
                ticket.Project = _context.Projects.FirstOrDefault(p => p.Id == Project);

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public async Task<PartialViewResult> Edit(int? Number)
        {
            var ticket = await _context.Ticket.FindAsync(Number);
            return PartialView("Edit", ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedOn,CreatedBy,Project")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var ticke = _context.Ticket.Where(t => t.Id == id).Select(t => t.Project.Id);
                int idd = ticke.First();
                return RedirectToAction(nameof(Tick), new { id = idd.ToString() });
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize]
        public async Task<PartialViewResult> Delete(int? Number)
        {

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == Number);

            return PartialView("Delete", ticket);
        }

        // POST: Tickets/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);


            var ticketd0 = _context.TicketDetail.Where(t => t.Ticket == ticket).Select(t => t.Id);
            foreach (var Tickd in ticketd0)
            {
                var ticketd1 = await _context.TicketDetail.FindAsync(Tickd);
                _context.TicketDetail.Remove(ticketd1);
            }


            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
