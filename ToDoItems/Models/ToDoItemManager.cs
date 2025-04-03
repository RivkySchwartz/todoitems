using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace ToDoItems.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
    public class ToDoItem
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }

    public class ToDoItemManager
    {
        private string _connectionString;

        public ToDoItemManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ToDoItem> GetUncompletedItems()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT i.*, c.CategoryName FROM Items i
                                JOIN Categories c
                                ON i.CategoryId = c.Id
                                WHERE i.CompletedDate IS NULL";
            connection.Open();
            var reader = cmd.ExecuteReader();

            List<ToDoItem> items = new List<ToDoItem>();
            while (reader.Read())
            {
                items.Add(new ToDoItem
                {
                    Title = (string)reader["Title"],
                    DueDate = (DateTime)reader["DueDate"],
                    CategoryId = (int)reader["CategoryId"],
                    CategoryName = (string)reader["CategoryName"]
                });
            }
            return items;
        }

        public void MarkAsCompleted(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Items SET CompletedDate = @date WHERE CategoryId = @id";
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();

        }

        public List<ToDoItem> GetCompletedItems()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT i.*, c.CategoryName FROM Items i
                                JOIN Categories c
                                ON i.CategoryId = c.Id
                                WHERE i.CompletedDate IS NOT NULL";
            connection.Open();
            var reader = cmd.ExecuteReader();

            List<ToDoItem> items = new List<ToDoItem>();
            while (reader.Read())
            {
                items.Add(new ToDoItem
                {
                    Title = (string)reader["Title"],
                    CategoryId = (int)reader["CategoryId"],
                    CompletedDate = (DateTime)reader["CompletedDate"],
                    CategoryName = (string)reader["CategoryName"]
                });
            }
            return items;
        }

        public void AddItem(ToDoItem item)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Items VALUES (@title, @dueDate, @dateCompleted, @categoryId)";
            cmd.Parameters.AddWithValue("@title", item.Title);
            cmd.Parameters.AddWithValue("@dueDate", item.DueDate);
            cmd.Parameters.AddWithValue("@dateCompleted", DBNull.Value);
            cmd.Parameters.AddWithValue("@categoryId", item.CategoryId);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public List<Category> GetCategories()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Categories";

            connection.Open();
            var reader = cmd.ExecuteReader();

            List<Category> categories = new List<Category>();
            while (reader.Read())
            {
                categories.Add(new Category
                {
                    Id = (int)reader["Id"],
                    CategoryName = (string)reader["CategoryName"]
                });
            }
            return categories;
        }

        public void AddCategory(Category category)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO categories (CategoryName) " +
                "VALUES (@name)";

            cmd.Parameters.AddWithValue("@name", category.CategoryName);
            connection.Open();
            cmd.ExecuteNonQuery();

        }

        public Category GetCategoryById(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Categories WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = cmd.ExecuteReader();
            reader.Read();
            return (new Category
            {
                Id = (int)reader["Id"],
                CategoryName = (string)reader["CategoryName"],
            });
        }

        public List<ToDoItem> GetItemsForCategory(int catId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT i.*, c.CategoryName FROM Items i
                                JOIN Categories c
                                ON i.CategoryId = c.Id
                                WHERE c.Id = @id";
            cmd.Parameters.AddWithValue("@id", catId);
            connection.Open();
            var reader = cmd.ExecuteReader();

            List<ToDoItem> items = new List<ToDoItem>();
            while (reader.Read())
            {
                items.Add(new ToDoItem
                {
                    Title = (string)reader["Title"],
                    CategoryId = (int)reader["CategoryId"],
                    DueDate = (DateTime)reader["DueDate"],
                    CompletedDate = reader.GetOrNull<DateTime?>("CompletedDate"),
                    CategoryName = (string)reader["CategoryName"]
                });
            }
            return items;

        }

        public void UpdateCategory(Category c)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Categories SET CategoryName = @name WHERE Id = @id";
            cmd.Parameters.AddWithValue("@Name", c.CategoryName);
            cmd.Parameters.AddWithValue("@Id", c.Id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}

