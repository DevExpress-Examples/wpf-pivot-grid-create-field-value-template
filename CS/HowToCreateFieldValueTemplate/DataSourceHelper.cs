using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace HowToCreateFieldValueTemplate {
    public class DataSourceHelperBase: IDisposable {
        string ConnectionString { get; set; }
        protected DataSourceHelperBase(string connectionStringName) {
            ConnectionString = ConfigurationManager.ConnectionStrings["nwindConnectionString"].ConnectionString;
        }

        SqliteConnection connection;
        protected SqliteConnection Connection {
            get {
                if (connection == null) {
                    connection = new SqliteConnection(ConnectionString);
                }
                return connection;
            }
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    Connection.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected void FillDataTable(DataTable dt, string selectCommand) {            
            Connection.Open();
            SqliteCommand command = new SqliteCommand(selectCommand, Connection);
            using (SqliteDataReader dr = command.ExecuteReader()) {
                do {
                    dt.BeginLoadData();
                    dt.Load(dr);
                    dt.EndLoadData();
                } while (!dr.IsClosed && dr.NextResult());
            }
            Connection.Close();
        }

        protected object ExecuteCommand(string sqlCommand) {
            Connection.Open();
            SqliteCommand command = new SqliteCommand(sqlCommand, Connection);
            var result =  command.ExecuteScalar();
            Connection.Close();
            return result;
        }
    }


    public class DataSourceHelper: DataSourceHelperBase {
        public DataSourceHelper(): base("nwindConnectionString") {
        }
        public void FillCategories(DataTable dt) {
            FillDataTable(dt, "SELECT CategoryID, CategoryName, Description, Picture, Icon_17, Icon_25 FROM Categories");
        }

        public void FillSalesPerson(DataTable dt) {
            FillDataTable(dt, "SELECT OrderID, Country, FirstName, LastName, ProductName, CategoryName, OrderDate, UnitPrice, Quantity, Discount, ExtendedPrice, FullName FROM SalesPerson");
        }

        public byte[] GetIconImageByName(string categoryName) {
            return ExecuteCommand($"SELECT Icon17 FROM Categories WHERE  (CategoryName = \"{categoryName}\")") as byte[];
        }


    }
}