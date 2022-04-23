using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketDetail
    {
        public int Id { get; set; }
        public String Comment { get; set; }
        public String Priority { get; set; }
        public String Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public String CreatedBy { get; set; }

        public Ticket Ticket { get; set; }

        public TicketDetail()
        {

        }
    }
}
