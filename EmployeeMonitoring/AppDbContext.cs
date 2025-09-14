using EmployeeMonitoring.Models.Departments;
using EmployeeMonitoring.Models.Persons;
using EmployeeMonitoring.Models.Posts;
using EmployeeMonitoring.Models.Statuses;
using System.Data.Entity;

namespace EmployeeMonitoring
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DatabaseConnectionString")
        {
            Database.SetInitializer<AppDbContext>(null);
        }

        public AppDbContext(string connectionString)  : base(connectionString) 
        {
            Database.SetInitializer<AppDbContext>(null);
        }

        public DbSet<Department> Departments {  get; set; }
        public DbSet<Person> Persons {  get; set; }
        public DbSet<Post> Posts {  get; set; }
        public DbSet<Status> Statuses {  get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            StatusConfiguration.Configure(modelBuilder);
            PostConfiguration.Configure(modelBuilder);
            DepartmentConfiguration.Configure(modelBuilder);
            PersonConfiguration.Configure(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
