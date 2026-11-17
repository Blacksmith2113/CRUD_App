using MaterialDesignThemes.Wpf;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.IO;

namespace CRUD_App {
    class UnitRepository : IUnitRepository{

        private static string _dbFilePath = Path.Combine(Environment.CurrentDirectory, "dbUnits.db");
        private static string _connectionString = $"Data Source = {_dbFilePath}";

        private bool IsDatabaseExist() {
            bool isDatabaseExist = false;
            if (File.Exists(_dbFilePath)) {
                using var dbConnection = new SqliteConnection(_connectionString);
                dbConnection.Open();
                string dbQuery = "SELECT name FROM sqlite_master WHERE type = 'table' AND name='Units'";
                using var dbCommand = new SqliteCommand(dbQuery, dbConnection);
                object? resultObj = dbCommand.ExecuteScalar();
                string? result = resultObj as string;
                if (result == "Units") {
                    isDatabaseExist = true;
                }
            }
            return isDatabaseExist;
        }

        private void CreateDatabase() {
            using var dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            string dbQuery = "CREATE TABLE Units(Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Points INTEGER, Image BLOB, Category TEXT)";
            using var dbCommand = new SqliteCommand(dbQuery, dbConnection);
            dbCommand.ExecuteNonQuery();
        }

        public UnitRepository() {
            if (!IsDatabaseExist()) {
                CreateDatabase();
            }
        }
        public bool Create(Unit e) {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var command = new SqliteCommand("INSERT INTO Units(Name, Points, Image, Category) VALUES(@Name, @Points, @Image, @Category)", connection);
            command.Parameters.AddWithValue("@Name", e.Name);
            command.Parameters.AddWithValue("@Points", e.Points);
            command.Parameters.Add("@Image", SqliteType.Blob, e.Image.Length).Value = e.Image;
            command.Parameters.AddWithValue("@Category", e.Category);

            if(command.ExecuteNonQuery() != 1) {
                return false;
            }

            using var getId = new SqliteCommand("SELECT last_insert_rowid()", connection);
            e.Id = Convert.ToInt32(getId.ExecuteScalar());
            
            return true;
        }
        public ObservableCollection<Unit> ReadAll() {
            ObservableCollection<Unit> units = new ObservableCollection<Unit>();
            using var dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            string Query = "SELECT * FROM Units";
            using var dbCommand = new SqliteCommand(Query, dbConnection);
            using var dataReader = dbCommand.ExecuteReader();
            while (dataReader.Read()) {
                Unit unit= new Unit() {
                    Id = Convert.ToInt32(dataReader["Id"]),
                    Name = dataReader["Name"].ToString(),
                    Points = Convert.ToInt32(dataReader["Points"]),
                    Image = (byte[])dataReader["Image"],
                    Category = dataReader["Category"].ToString()
                };
                units.Add(unit);
            }
            return units;
        } 
        public Unit Read(int id) {
            Unit unit = new Unit();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var command = new SqliteCommand("SELECT * FROM Units WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            using var dataReader= command.ExecuteReader();
            while (dataReader.Read()) {
                unit = new Unit() {
                    Id = Convert.ToInt32(dataReader["Id"]),
                    Name = dataReader["Name"].ToString(),
                    Points = Convert.ToInt32(dataReader["Points"]),
                    Image = (byte[])dataReader["Image"],
                    Category = dataReader["Category"].ToString()
                };
            }
            return unit;
        }
        public bool Edit(Unit e) {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var command = new SqliteCommand("UPDATE Units SET Name = @Name, Points = @Points, Category = @Category, Image = @Image Where Id = @Id", connection);
            command.Parameters.AddWithValue("@Name", e.Name);
            command.Parameters.AddWithValue("@Points", e.Points);
            command.Parameters.AddWithValue("@Category", e.Category);
            command.Parameters.Add("@Image", SqliteType.Blob, e.Image.Length).Value = e.Image;
            command.Parameters.AddWithValue("@Id", e.Id);

            return command.ExecuteNonQuery() == 1;
        }
        public bool Delete(Unit e) {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var command = new SqliteCommand("DELETE FROM Units WHERE Id =  @Id", connection);
            command.Parameters.AddWithValue("@Id", e.Id);
            return command.ExecuteNonQuery() == 1;
        }
    }
}
