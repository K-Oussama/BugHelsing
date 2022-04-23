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
    public class TicketDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketDetails
        [Authorize]
        public IActionResult Index()
        {
            var v = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var project = _context.Handles.Where(c => c.Usr.Id == v).Select(c => c.Proj);
            List<TicketDetail> toc = _context.TicketDetail.ToList();
            List<Ticket> tick = _context.Ticket.ToList();
            var toctocd = from td in toc
                         join t in tick on td.Ticket equals t into table1
                         from t in table1.ToList()
                         join p in project on t.Project equals p into table2
                         from p in table2.ToList()
                          select new TicketDetail
                         {
                             Id = td.Id,
                             Comment = td.Comment,
                             Status = td.Status,
                             Priority = td.Priority,
                             CreatedBy = td.CreatedBy,
                             CreatedOn = td.CreatedOn,
                             Ticket = td.Ticket
                         };

            return View(toctocd.ToList());
        }

        [Authorize]
        public async Task<IActionResult> TickD(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.IDd = id;
            var ticketd = await _context.TicketDetail
            .Where(b => b.Ticket.Id == id)
            .ToListAsync();


            var ticket = _context.Ticket.Find(id);
            ViewBag.ticketname = ticket.Name;


            return View(ticketd);
        }

        // GET: TicketDetails/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketDetail = await _context.TicketDetail
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketDetail == null)
            {
                return NotFound();
            }

            return View(ticketDetail);
        }

        // GET: TicketDetails/Create
        [Authorize]
        public PartialViewResult Create(int? Number)
        {
            ViewBag.ticke = Number;
            return PartialView("Create");
        }

        // POST: TicketDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ticket, [Bind("Id,Comment,Priority,Status,CreatedOn,CreatedBy")] TicketDetail ticketDetail)
        {
            if (ModelState.IsValid)
            {
                ticketDetail.Ticket = _context.Ticket.FirstOrDefault(p => p.Id == ticket);

                _context.Add(ticketDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketDetail);
        }

        // GET: TicketDetails/Edit/5
        [Authorize]
        public async Task<PartialViewResult> Edit(int? Number)
        {

            var ticketDetail = await _context.TicketDetail.FindAsync(Number);
            return PartialView("Edit", ticketDetail);
        }

        // POST: TicketDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comment,Priority,Status,CreatedOn,CreatedBy")] TicketDetail ticketDetail)
        {
            if (id != ticketDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketDetailExists(ticketDetail.Id))
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
            return View(ticketDetail);
        }

        // GET: TicketDetails/Delete/5
        [Authorize]
        public async Task<PartialViewResult> Delete(int? Number)
        {
            var ticketDetail = await _context.TicketDetail
                .FirstOrDefaultAsync(m => m.Id == Number);

            return PartialView("Delete", ticketDetail);
        }

        // POST: TicketDetails/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketDetail = await _context.TicketDetail.FindAsync(id);
            _context.TicketDetail.Remove(ticketDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool TicketDetailExists(int id)
        {
            return _context.TicketDetail.Any(e => e.Id == id);
        }
    }
}
