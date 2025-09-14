using EmployeeMonitoring.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring
{
    public class DepartmentRepository
    {
        private readonly AppDbContext appDbContext;

        public DepartmentRepository()
        {
           appDbContext = new AppDbContext();
        }

        public List<Department> GetAll()
        {
            return appDbContext.Departments.ToList();
        }
        
        public List<string> GetAllNames()
        {
            return appDbContext.Departments.Select(d => d.Name).ToList();
        }
    }
}
