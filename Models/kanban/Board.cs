using System.Collections.Generic;

namespace BugTracker.Models
{
  public class Board
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Column> Columns { get; set; } = new List<Column>();

        public Project Project { get; set; }
    }
}
