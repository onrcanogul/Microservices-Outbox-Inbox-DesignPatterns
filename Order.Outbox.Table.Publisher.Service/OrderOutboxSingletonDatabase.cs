using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Order.Outbox.Table.Publisher.Service
{
    public static class OrderOutboxSingletonDatabase
    {
        static IDbConnection dbConnection;
        static IConfiguration configuration;
        static OrderOutboxSingletonDatabase()
            => dbConnection = new SqlConnection("Data Source=ONURCAN;Integrated Security=True;Initial Catalog = Outbox-Order;Trust Server Certificate=True;");

        public static IDbConnection Connection 
        {
            get
            {
                if (dbConnection.State == ConnectionState.Closed)
                    dbConnection.Open();
                return dbConnection;
            } 
        }

        static bool dataReaderState = true;
        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql) => await dbConnection.QueryAsync<T>(sql);
        public static async Task<int> ExecuteAsync(string sql) => await dbConnection.ExecuteAsync(sql);
        public static void DataReaderReady() => dataReaderState = true;
        public static void DataReaderBusy() => dataReaderState = false;
        public static bool DataReaderState => dataReaderState;
     }
}
