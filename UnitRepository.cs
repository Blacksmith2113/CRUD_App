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
            ObservableCollection<Unit> Units = new ObservableCollection<Unit>();
            return Units;
        } 
        public Unit Read(int id) {
            Unit Unit = new Unit();
            return Unit;
        }
        public bool Edit(Unit e) {
            return true;
        }
        public bool Delete(Unit e) {
            return true;
        }
    }
}
