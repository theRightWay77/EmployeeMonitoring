using EmployeeMonitoring.Models.Departments;
using EmployeeMonitoring.Models.Persons;
using EmployeeMonitoring.Models.Posts;
using EmployeeMonitoring.Models.Statuses;
using System;
using System.Linq;
using System.Windows.Forms;

namespace EmployeeMonitoring
{
    public static class DatabaseInitializer
    {
        public static void InitializeDatabase()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    if (!context.Database.Exists())
                    {
                        context.Database.Create();
                    }

                    CreateTablesIfNotExists(context);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}");
            }
        }

        private static void CreateTablesIfNotExists(AppDbContext context)
        {
            try
            {
                var tableExists = context.Database.SqlQuery<int>(@"
                SELECT COUNT(*) FROM information_schema.tables 
                WHERE table_schema = 'public' 
                AND table_name IN ('status', 'deps', 'posts', 'persons')"
                ).FirstOrDefault();

                if (tableExists < 4)
                {
                    var createTablesSql = @"
                    CREATE TABLE IF NOT EXISTS public.status (
                        id SERIAL PRIMARY KEY,
                        name VARCHAR(100) NOT NULL
                    );
                    
                    CREATE TABLE IF NOT EXISTS public.deps (
                        id SERIAL PRIMARY KEY,
                        name VARCHAR(100) NOT NULL
                    );
                    
                    CREATE TABLE IF NOT EXISTS public.posts (
                        id SERIAL PRIMARY KEY,
                        name VARCHAR(100) NOT NULL
                    );
                    
                    CREATE TABLE IF NOT EXISTS public.persons (
                        id SERIAL PRIMARY KEY,
                        first_name VARCHAR(100) NOT NULL,
                        second_name VARCHAR(100) NOT NULL,
                        last_name VARCHAR(100) NOT NULL,
                        date_employ DATE,
                        date_uneploy DATE,
                        status_id INTEGER REFERENCES public.status(id),
                        dep_id INTEGER REFERENCES public.deps(id),
                        post_id INTEGER REFERENCES public.posts(id)
                    );";

                    context.Database.ExecuteSqlCommand(createTablesSql);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания таблиц: {ex.Message}");
            }
        }

        //private static void SeedData(AppDbContext context)
        //{
        //    try
        //    {
        //        if (context.Statuses.Any()) return;

        //        var statuses = new[]
        //        {
        //        new Status { Name = "Работает" },
        //        new Status { Name = "Уволен" },
        //        new Status { Name = "В отпуске" }
        //    };
        //        context.Statuses.AddRange(statuses);

        //        var deps = new[]
        //        {
        //        new Department { Name = "IT отдел" },
        //        new Department { Name = "Бухгалтерия" },
        //        new Department { Name = "Отдел кадров" }
        //    };
        //        context.Departments.AddRange(deps);

        //        var posts = new[]
        //        {
        //        new Post { Name = "Программист" },
        //        new Post { Name = "Бухгалтер" },
        //        new Post { Name = "HR-менеджер" }
        //    };
        //        context.Posts.AddRange(posts);

        //        context.SaveChanges();

        //        var persons = new[]
        //        {
        //        new Person
        //        {
        //            FirstName = "Иван",
        //            SecondName = "Иванович",
        //            LastName = "Иванов",
        //            DateEmploy = new DateTime(2023, 1, 15),
        //            StatusId = 1,
        //            DepId = 1,
        //            PostId = 1
        //        }
        //    };
        //        context.Persons.AddRange(persons);

        //        context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка заполнения данными: {ex.Message}");
        //    }
        //}
    }
}
