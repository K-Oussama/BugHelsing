using System.ComponentModel.DataAnnotations;

namespace BugTracker.ViewModels
{
    public class NewBoard
    {
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage ="Please use letters only without special characters and capitalize the first letter.")]
        [Required]
        public string Title { get; set; }

        public int ProjID { get; set; }
    }
}
