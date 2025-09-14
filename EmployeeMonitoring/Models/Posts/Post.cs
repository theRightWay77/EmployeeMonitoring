using EmployeeMonitoring.Models.Persons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Models.Posts
{
    /// <summary>
    /// Должности.
    /// </summary>
    public class Post
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
