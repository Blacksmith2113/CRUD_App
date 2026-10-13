using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CRUD_App;
using System.Collections.ObjectModel;
using Microsoft.Data.Sqlite;

namespace CRUD_App {
    class UnitRepository : IUnitRepository{

        private static string _dbFilePath = Path.Combine(Environment.CurrentDirectory, "dbUnits.db");
        private static string _connectionString = $"Data Source = {_dbFilePath}";

        private bool IsDatabaseExist() {
            bool isDatabaseExist = false;
            if (File.Exists(_dbFilePath)) {
                SqliteConnection dbConnection = new SqliteConnection(_connectionString);
                dbConnection.Open();
                if (dbConnection.State == ConnectionState.Open) {
                    string dbQuery = "SELECT name FROM sqlite_master WHERE type = 'table' AND name='Units'";
                    SqliteCommand dbCommand = new SqliteCommand(dbQuery, dbConnection);
                    object? resultObj = dbCommand.ExecuteScalar();
                    string? result = resultObj as string;
                    if (result == "Units") {
                        isDatabaseExist = true;
                    }
                }
                dbConnection.Close();
            }
            return isDatabaseExist;
        }

        private void CreateDatabase() {
            SqliteConnection dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();

            if (dbConnection.State == ConnectionState.Open) {
                string dbQuery = "CREATE TABLE Units(Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Points INTEGER, Image BLOB, Category TEXT)";
                SqliteCommand dbCommand = new SqliteCommand(dbQuery, dbConnection);
                dbCommand.ExecuteNonQuery();
            }
            dbConnection.Close();
        }

        public UnitRepository() {
            if (!IsDatabaseExist()) {
                CreateDatabase();
            }
        }
        public bool Create(Unit e) {
            bool isCreated = false;
            SqliteConnection dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            if (dbConnection.State == ConnectionState.Open) {
                string Query = $"INSERT INTO Units(Name, Points, Image, Category) VALUES('{e.Name}','{e.Points}', @Image, '{e.Category}')";
                SqliteCommand dbCommand = new SqliteCommand(Query, dbConnection);
                dbCommand.Parameters.Add("@Image", SqliteType.Blob, e.Image.Length).Value = e.Image;
                if (dbCommand.ExecuteNonQuery() == 1) {
                    isCreated = true;
                }
            }
            dbConnection.Close();
            return isCreated;
        }
        public ObservableCollection<Unit> ReadAll() {
            ObservableCollection<Unit> units = new ObservableCollection<Unit>();
            SqliteConnection dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            if (dbConnection.State == ConnectionState.Open) {
                string Query = "SELECT * FROM Units";
                SqliteCommand dbCommand = new SqliteCommand(Query, dbConnection);
                SqliteDataReader dataReader = dbCommand.ExecuteReader();
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
            }
            dbConnection.Close();
            return units;
        } 
        public Unit Read(int id) {
            Unit unit = new Unit();
            SqliteConnection dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            if (dbConnection.State == ConnectionState.Open) {
                string Query = $"SELECT * FROM Units WHERE Id = {id}";
                SqliteCommand dbCommand = new SqliteCommand(Query, dbConnection);
                SqliteDataReader dataReader = dbCommand.ExecuteReader();

                while (dataReader.Read()) {

                    unit = new Unit() {
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Name = dataReader["Name"].ToString(),
                        Points = Convert.ToInt32(dataReader["Points"]),
                        Image = (byte[])dataReader["Image"],
                        Category = dataReader["Category"].ToString()
                    };
                }
            }
            dbConnection.Close();
            return unit;
        }
        public bool Edit(Unit e) {
            bool isUpdated = false;
            SqliteConnection dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            if (dbConnection.State == ConnectionState.Open) {
                string Query = $"UPDATE Units SET Name = '{e.Name}', Points = '{e.Points}', Category = '{e.Category}', Image = @Image Where Id = {e.Id}";
                SqliteCommand dbCommand = new SqliteCommand(Query, dbConnection);
                dbCommand.Parameters.Add("@Image", SqliteType.Blob, e.Image.Length).Value = e.Image;
                if (dbCommand.ExecuteNonQuery() == 1) {
                    isUpdated = true;
                }
            }
            dbConnection.Close();
            return isUpdated;
        }
        public bool Delete(Unit e) {
            bool isDeleted = false;
            SqliteConnection dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            if (dbConnection.State == ConnectionState.Open) {
                string Query = $"DELETE FROM Units WHERE Id={e.Id}";
                SqliteCommand dbCommand = new SqliteCommand(Query, dbConnection);
                if (dbCommand.ExecuteNonQuery() == 1) {
                    isDeleted = true;
                }

            }
            dbConnection.Close();
            return isDeleted;
        }
    }
}
