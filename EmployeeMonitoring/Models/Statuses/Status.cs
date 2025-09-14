using EmployeeMonitoring.Models.Persons;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeMonitoring.Models.Statuses
{
    /// <summary>
    /// Статусы сотрудников.
    /// </summary>
    public class Status
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
