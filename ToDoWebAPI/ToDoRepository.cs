using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ToDoWebAPI
{
    public class ToDoRepository
    {
        private static string _connectionString = @"Server = RUPRECHTMACHINE\SQLEXPRESS;
                Database = ToDo_Storage;
                Trusted_Connection = True;";

        private class ToDo
        {
            public int Id { get; }
            public string Name { get; set; }
            public bool Done { get; set; }
            public DateTime creationDate { get; }

            public ToDo(int id, string name, bool done)
            {
                Id = id;
                Name = name;
                Done = done;
                creationDate = DateTime.Now;
            }
        }

        public List<ToDoDto> GetAll()
        {
            List<ToDoDto> toDoDtos = new List<ToDoDto>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [Id], [Name], [IsDone] FROM [ToDos]";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ToDoDto dto = new ToDoDto
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = Convert.ToString(reader["Name"]),
                                Done = Convert.ToBoolean(reader["IsDone"])
                            };
                            toDoDtos.Add(dto);
                        }
                    }
                }
            }

            return toDoDtos;

            //return DataBase.ConvertAll(x => new ToDoDto
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    Done = x.Done
            //});
        }

        public int Create(ToDoDto toDoDto)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO [ToDos] 
                        ([Name], 
                         [IsDone], 
                         [CreationDateTime]) 
                    VALUES 
                        (@name, 
                         @done, 
                         @datetime)
                    SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.Add("@name", System.Data.SqlDbType.NVarChar).Value = toDoDto.Name;
                    cmd.Parameters.Add("@done", System.Data.SqlDbType.NVarChar).Value = toDoDto.Done;
                    cmd.Parameters.Add("@datetime", System.Data.SqlDbType.NVarChar).Value = DateTime.Now;

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(int id, ToDoDto toDoDto)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE [ToDos]
                                        SET [IsDone] = @done
                                        WHERE [Id] = @id";

                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@done", System.Data.SqlDbType.Bit).Value = toDoDto.Done;

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
