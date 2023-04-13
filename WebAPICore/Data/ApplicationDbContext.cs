using Microsoft.EntityFrameworkCore;
using WebAPICore.Model;

namespace WebAPICore.Data
{
  public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Patient> Patients { get; set; }
    }
}