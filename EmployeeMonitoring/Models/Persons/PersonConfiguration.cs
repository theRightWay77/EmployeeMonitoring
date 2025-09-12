using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Models.Persons
{
    internal class PersonConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Person>().ToTable("persons", "public");

            modelBuilder.Entity<Person>()
                .HasRequired(p => p.Status)
                .WithMany(s => s.Persons)
                .HasForeignKey(p => p.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasRequired(p => p.Department)
                .WithMany(d => d.Persons)
                .HasForeignKey(p => p.DepId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasRequired(p => p.Post)
                .WithMany(p => p.Persons)
                .HasForeignKey(p => p.PostId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .Property(p => p.DateEmploy)
                .IsOptional();

            modelBuilder.Entity<Person>()
                .Property(p => p.DateUneploy)
                .IsOptional();
        }
    }
}
