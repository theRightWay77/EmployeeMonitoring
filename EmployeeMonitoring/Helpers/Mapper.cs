using EmployeeMonitoring.Models.DTO;
using EmployeeMonitoring.Models.Persons;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeMonitoring.Helpers
{
    static class Mapper
    {
        public static List<EmployeeDisplayModel> FromPersonToEmployeeDisplayModel(List<Person> persons)
        {
            var employees = persons
                    .Select(p => new EmployeeDisplayModel()
                    {
                        Id = p.Id,
                        FullName = p.SecondName + " " + p.FirstName.Substring(0, 1) + ". " + p.LastName.Substring(0, 1) + ".",
                        StatusName = p.Status.Name,
                        DepartmentName = p.Department.Name,
                        PositionName = p.Post.Name,
                        DateEmploy = p.DateEmploy,
                        DateUneploy = p.DateUneploy
                    })
                    .ToList();
            return employees;
        }
        
        public static EmployeeDisplayModel FromPersonToEmployeeDisplayModel(Person persons)
        {
            var employees = new EmployeeDisplayModel()
                    {
                        Id = persons.Id,
                        FullName = persons.SecondName + " " + persons.FirstName.Substring(0, 1) + ". " + persons.LastName.Substring(0, 1) + ".",
                        StatusName = persons.Status.Name,
                        DepartmentName = persons.Department.Name,
                        PositionName = persons.Post.Name,
                        DateEmploy = persons.DateEmploy,
                        DateUneploy = persons.DateUneploy
                    };
            return employees;
        }
    }
}
