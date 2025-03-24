using Microsoft.EntityFrameworkCore;

namespace WebAPIPRoject.Models
{
    public class ITIContext:DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employee { get; set; }

        public ITIContext(DbContextOptions<ITIContext> options):base(options)
        {
                
        }
    }
}
