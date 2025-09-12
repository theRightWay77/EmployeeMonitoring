using EmployeeMonitoring.Models.Persons;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Models.Statuses
{
    internal class StatusConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>()
            .HasKey(s => s.Id)
            .Property(s => s.Id)
            .HasColumnName("id");

            modelBuilder.Entity<Status>().ToTable("status");
        }
    }
}
