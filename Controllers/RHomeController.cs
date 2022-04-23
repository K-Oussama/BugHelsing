using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    public class RHomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RHomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Dash()
        {
            // count
            var v = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // projets
            var project = _context.Handles.Where(c => c.Usr.Id == v).Select(c => c.Proj);
            ViewBag.projets = project.Count();


            // Tickets
            List<Ticket> ticket = _context.Ticket.ToList();
            var bug = from t in ticket
                      join p in project on t.Project equals p into table1
                      from p in table1.ToList()
                      select new Ticket
                      {
                          Id = t.Id,
                      };
            ViewBag.bugs = bug.Count();



            //Tache
            List<Card> tache = _context.Cards.ToList();
            List<Column> col = _context.Columns.ToList();
            List<Board> board = _context.Boards.ToList();
            var task = from t in tache
                       join c in col on t.ColumnId equals c.Id into table1
                       from c in table1.ToList()
                       join b in board on c.BoardId equals b.Id into table2
                       from b in table2.ToList()
                       join p in project on b.Project equals p into table3
                       from p in table3.ToList()
                       select new Card
                       {
                           Id = t.Id
                       };
            ViewBag.tasks = task.Count();

            //Board
            var brd = from b in board
                      join p in project on b.Project equals p into table1
                      from p in table1.ToList()
                      select new Board
                      {
                          Id = b.Id
                      };


            ViewBag.board = brd.Count();
            



            return View();
        }
    }
}
