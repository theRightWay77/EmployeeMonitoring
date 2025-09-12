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
                        FullName = p.LastName + " " + p.FirstName.Substring(0, 1) + ". " + p.SecondName.Substring(0, 1) + ".",
                        StatusName = p.Status.Name,
                        DepartmentName = p.Department.Name,
                        PositionName = p.Post.Name,
                        DateEmploy = p.DateEmploy,
                        DateUneploy = p.DateUneploy
                    })
                    .ToList();
            return employees;
        }
    }
}
