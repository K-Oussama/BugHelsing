using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using BugTracker.Models;

namespace BugTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Models.Project> Projects { get; set; }
        public DbSet<BugTracker.Models.Ticket> Ticket { get; set; }
        public DbSet<BugTracker.Models.TicketDetail> TicketDetail { get; set; }

        public DbSet<BugTracker.Models.Handles> Handles { get; set; }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Column> Columns { get; set; }
    }
}
