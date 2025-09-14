using EmployeeMonitoring.Models.Departments;
using EmployeeMonitoring.Models.Posts;
using EmployeeMonitoring.Models.Statuses;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeMonitoring.Models.Persons
{
    /// <summary>
    /// Сотрудники.
    /// </summary>
    public class Person
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("second_name")]
        public string SecondName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("date_employ")]
        public DateTime? DateEmploy { get; set; }

        [Column("date_uneploy")]
        public DateTime? DateUneploy { get; set; }

        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("dep_id")]
        public int DepId { get; set; }
        [Column("post_id")]
        public int PostId { get; set; }

       
        public virtual Status Status { get; set; }
        public virtual Department Department { get; set; }
        public virtual Post Post { get; set; }
    }
}
