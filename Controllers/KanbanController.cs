using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using BugTracker.Services;
using BugTracker.ViewModels;
using BugTracker.Models;
using BugTracker.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

namespace BugTracker.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class KanbanController : Controller
    {
        private readonly BoardService _boardService;
        private readonly ApplicationDbContext _context;

        public KanbanController(ApplicationDbContext context, BoardService boardService)
        {
            _boardService = boardService;
            _context = context;
        }

        // 
        public IActionResult Calendar() {

            return View();
        }

        public async Task<IActionResult> AllT()
        {

            var v = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var project = _context.Handles.Where(c => c.Usr.Id == v).Select(c => c.Proj);
            Task<List<Board>> board = _context.Boards.ToListAsync();
            var brd = from b in await board
                      join p in project on b.Project equals p into table1
                      from p in table1.ToList()
                      select new Board
                      {
                          Id = b.Id,
                          Title = b.Title
                      };

            // await _context.Boards.ToListAsync()
            return View(brd.ToList());
        }

        public IActionResult All()
        {
            //var model = _boardService.ListBoard();
            var v = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var project = _context.Handles.Where(c => c.Usr.Id == v).Select(c => c.Proj);
            List<Board> board = _context.Boards.ToList();
            var model = from b in board
                      join p in project on b.Project equals p into table1
                      from p in table1.ToList()
                      select new Board
                      {
                          Id = b.Id,
                          Title = b.Title
                      };
            return View(model.ToList());
        }

        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.boardid = id;
            var model2 = await _context.Boards.Where(b => b.Project.Id == id).ToListAsync();

            var project = await _context.Projects.FindAsync(id);
            if(project == null)
            {
                ViewBag.boardname = "ce projet";
            }
            else { ViewBag.boardname = project.Name; }   
            //var model = _boardService.ListBoard();

            return View(model2);
        }

        [HttpGet]
        public IActionResult Create(int? id)
        {
            ViewBag.boardid2 = id;
            return View();
        }

        [HttpPost]
        public IActionResult Create(NewBoard viewModel,int? id)
        {
           // viewModel.Project = _context.Projects.FirstOrDefault(p => p.Id == id);
           // _context.Add(viewModel);
           // await _context.SaveChangesAsync();

            _boardService.AddBoard(viewModel, id);
            return RedirectToAction(nameof(Index), new { id });
        }

        //[Authorize]
        [HttpPost]
        public IActionResult Delete(BoardView boardView, int? id)
        {
            try
            {
                _boardService.DeleteBoard(boardView.Id);


                //var ticke = _context.Boards.Where(t => t.Id == id).Select(t => t.Project.Id);
                //int idd = ticke.First();
                return RedirectToAction(nameof(All)); //, new { id = idd.ToString() }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new { id });
            }
        }

    }
}