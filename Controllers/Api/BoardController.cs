using Microsoft.AspNetCore.Mvc;
using BugTracker.Services;
using BugTracker.ViewModels;

namespace BugTracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly BoardService _boardService;

        public BoardController(BoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpPost("movecard")]
        public IActionResult MoveCard([FromBody] MoveCardCommand command)
        {
            _boardService.Move(command);

            return Ok(new 
            { 
                Moved = true
            });
        }
    }
}