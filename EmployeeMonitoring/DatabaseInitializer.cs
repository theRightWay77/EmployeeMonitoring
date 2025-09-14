using Npgsql;
using System;
using System.Linq;
using System.Windows.Forms;

namespace EmployeeMonitoring
{
    public static class DatabaseInitializer
    {
        public static void InitializeDatabase(string connectionString)
        {
            try
            {
                CreateDatabaseIfNotExists(connectionString, "EmpMon");

                CreateTablesIfNotExists(connectionString, "EmpMon");

                SeedDataIfNotExists(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}");
            }
        }

        private static void CreateDatabaseIfNotExists(string connectionString, string databaseName)
        {
            try
            {
                var builder = new NpgsqlConnectionStringBuilder(connectionString);
                builder.Database = "postgres"; 
                string serverConnectionString = builder.ToString();

                using (var connection = new NpgsqlConnection(serverConnectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(
                        "SELECT 1 FROM pg_database WHERE datname = @databaseName", connection))
                    {
                        command.Parameters.AddWithValue("databaseName", databaseName);

                        var result = command.ExecuteScalar();

                        if (result == null)
                        {
                            using (var createCommand = new NpgsqlCommand(
                                $"CREATE DATABASE \"{databaseName}\"", connection))
                            {
                                createCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания базы данных: {ex.Message}");
                throw;
            }
        }

        private static void CreateTablesIfNotExists(string connectionString, string databaseName)
        {
            try
            {
                var builder = new NpgsqlConnectionStringBuilder(connectionString);
                builder.Database = databaseName;
                string dbConnectionString = builder.ToString();

                using (var context = new AppDbContext(dbConnectionString))
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания таблиц: {ex.Message}");
                throw;
            }
        }

        private static void SeedDataIfNotExists(string connectionString)
        {
            try
            {
                using (var context = new AppDbContext(connectionString))
                {
                    var personsCount = context.Database.SqlQuery<int>("SELECT COUNT(*) FROM public.persons").FirstOrDefault();

                    if (personsCount == 0)
                    {
                        var insertStatusSql = @"
                INSERT INTO public.status (name) VALUES 
                ('Работает'),
                ('Уволен'),
                ('В отпуске'),
                ('Уволен по собственному желанию');";
                        context.Database.ExecuteSqlCommand(insertStatusSql);

                        var insertDepsSql = @"
                INSERT INTO public.deps (name) VALUES 
                ('IT отдел'),
                ('Бухгалтерия'),
                ('Отдел кадров'),
                ('Отдел продаж'),
                ('Маркетинговый отдел'),
                ('Отдел логистики'),
                ('Производственный отдел'),
                ('Отдел закупок'),
                ('Юридический отдел'),
                ('Отдел контроля качества');";
                        context.Database.ExecuteSqlCommand(insertDepsSql);

                        var insertPostsSql = @"
                INSERT INTO public.posts (name) VALUES 
                ('Программист'),
                ('Бухгалтер'),
                ('HR-менеджер'),
                ('Менеджер по продажам'),
                ('Маркетолог'),
                ('Логист'),
                ('Инженер'),
                ('Менеджер по закупкам'),
                ('Юрист'),
                ('Технолог');";
                        context.Database.ExecuteSqlCommand(insertPostsSql);

                        var insertPersonsSql = @"
                INSERT INTO public.persons (second_name, first_name, last_name, date_employ, date_uneploy, status_id, dep_id, post_id) VALUES
                ('Иванов', 'Иван', 'Иванович', '2023-01-15', NULL, 1, 1, 1),
                ('Петров', 'Петр', 'Петрович', '2022-03-20', NULL, 1, 2, 2),
                ('Сидорова', 'Мария', 'Сергеевна', '2023-05-10', NULL, 1, 3, 3),
                ('Козлова', 'Анна', 'Александровна', '2022-08-12', '2023-12-01', 2, 4, 4),
                ('Морозов', 'Сергей', 'Викторович', '2023-02-28', NULL, 1, 5, 5),
                ('Новикова', 'Елена', 'Викторовна', '2021-11-05', NULL, 1, 6, 6),
                ('Волков', 'Дмитрий', 'Алексеевич', '2023-07-14', NULL, 3, 7, 7),
                ('Кузнецова', 'Ольга', 'Николаевна', '2022-09-18', NULL, 1, 8, 8),
                ('Зайцев', 'Андрей', 'Борисович', '2023-04-22', NULL, 1, 9, 9),
                ('Смирнова', 'Татьяна', 'Михайловна', '2021-12-30', '2023-11-15', 4, 10, 10);";
                        context.Database.ExecuteSqlCommand(insertPersonsSql);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания таблиц: {ex.Message}");
                throw;
            }
        }
    }
}
