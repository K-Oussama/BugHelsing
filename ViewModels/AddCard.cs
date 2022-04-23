using System.ComponentModel.DataAnnotations;

namespace BugTracker.ViewModels
{
    public class AddCard
    {
        public int Id { get; set; }

        [Required]
        public string Contents { get; set; }
    }
}
