using EmployeeMonitoring.Models.Persons;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Models.Posts
{
    internal class PostConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
            .HasKey(s => s.Id)
            .Property(s => s.Id)
            .HasColumnName("id");

            modelBuilder.Entity<Post>().ToTable("posts");
        }
    }
}
