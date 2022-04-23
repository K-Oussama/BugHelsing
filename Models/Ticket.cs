using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public String CreatedBy { get; set; }

        public Project Project { get; set; }

        public List<TicketDetail> TicketDetails { get; set; }

        public Ticket()
        {

        }
    }
}
