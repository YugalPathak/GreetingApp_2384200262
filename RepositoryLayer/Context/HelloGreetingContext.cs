using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class HelloGreetingContext : DbContext
    {
        public HelloGreetingContext(DbContextOptions<HelloGreetingContext> options) : base(options)  { }
        public DbSet<HelloGreetingEntity> Greetings { get; set; }

    }
}