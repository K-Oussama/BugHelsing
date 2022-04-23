using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public String CreatedBy { get; set; }

        public List<Ticket> Tickets { get; set; }


        public List<Board> Boards { get; set; }

        public Project()
        {

        }
    }
}
