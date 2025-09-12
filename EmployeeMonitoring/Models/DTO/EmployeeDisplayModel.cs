using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Models.DTO
{

    public class EmployeeDisplayModel 
    {
        public int Id { get; set; }
        public string FullName { get; set; } // в виде Фамилия И. О.
        public string StatusName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public DateTime? DateEmploy { get; set; }
        public DateTime? DateUneploy { get; set; }
    }
}
