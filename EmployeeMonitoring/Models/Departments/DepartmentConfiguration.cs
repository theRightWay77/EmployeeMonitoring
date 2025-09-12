using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Models.Departments
{
    internal class DepartmentConfiguration 
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
            .HasKey(s => s.Id)
            .Property(s => s.Id)
            .HasColumnName("id"); 

            modelBuilder.Entity<Department>().ToTable("deps");
        }
    }
}
