
using EmployeeMonitoring.Models.Statuses;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeMonitoring
{
    class StatusRepository
    {
        private readonly AppDbContext appDbContext;

        public StatusRepository()
        {
            appDbContext = new AppDbContext();
        }

        public List<Status> GetAll()
        {
            return appDbContext.Statuses.ToList();
        }

        public List<string> GetAllNames()
        {
            return appDbContext.Statuses.Select(s => s.Name).ToList();
        }
    }
}
