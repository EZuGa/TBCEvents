using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C_.Data
{
    public class DataContext: DbContext
    {

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }

        public DbSet<Event> Events => Set<Event>();
        
    }
}