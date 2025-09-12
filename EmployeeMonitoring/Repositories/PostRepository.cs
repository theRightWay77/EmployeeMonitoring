using EmployeeMonitoring.Models.Posts;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeMonitoring.Repositories
{
    public class PostRepository
    {
        private readonly AppDbContext appDbContext;
        public PostRepository()
        {
            appDbContext = new AppDbContext();
        }

        public List<Post> GetAll()
        {
            return appDbContext.Posts.ToList();
        }
    }
}
