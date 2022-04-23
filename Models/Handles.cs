using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Handles
    {
        public int Id { get; set; }

        public IdentityUser Usr { get; set; }
        public Project Proj { get; set; }

        public Handles()
        {

        }
    }
}
