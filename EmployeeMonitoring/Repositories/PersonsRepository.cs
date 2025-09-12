using EmployeeMonitoring.Models.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring
{
    public class PersonsRepository
    {
        private readonly AppDbContext context;

        public PersonsRepository()
        {
           context = new AppDbContext();
        }

        public List<Person> GetPersons()
        {
            var e = context.Persons
                        .Include("Status")
                        .Include("Department")
                        .Include("Post")
                        .ToList();

            return e;
        }

        public List<Person> GetByStatus(string status)
        {
           return GetPersons().Where(p => p.Status.Name == status).ToList();
        }
    }
}
