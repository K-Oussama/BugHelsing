using System.Collections.Generic;

namespace BugTracker.Models
{
  public class Column
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public int BoardId { get; set; }
    }
}
