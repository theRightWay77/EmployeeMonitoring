using EmployeeMonitoring.Models.Persons;
using System.Collections.Generic;
using System.Linq;

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
        
        public List<Person> GetByDep(string dep)
        {
           return GetPersons().Where(p => p.Department.Name == dep).ToList();
        }

        public List<Person> GetByPost(string post)
        {
           return GetPersons().Where(p => p.Post.Name == post).ToList();
        }

        public List<Person> GetBySecondName(string secondName)
        {
            return GetPersons().Where(p => p.SecondName.ToLower().Contains(secondName.ToLower())).ToList();
        }
    }
}
