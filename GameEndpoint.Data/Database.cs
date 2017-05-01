using System.Data.SQLite;
using System.IO;
using System.Reflection;
using Dapper;

namespace GameEndpoint.Data
{
    public class Database
    {
        private readonly string connectionString;

        public Database(string _connectionString)
        {
            this.connectionString = _connectionString;
        }

        public void Initialize()
        {
            if (!this.databaseExist())
            {
                this.createDatabase();
                this.createTables();
            }
        }

        private bool databaseExist()
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder(this.connectionString);
            return File.Exists(Path.Combine(Assembly.GetEntryAssembly().CodeBase, builder.DataSource));
        }

        private void createDatabase()
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder(this.connectionString);
            SQLiteConnection.CreateFile(Path.Combine(Assembly.GetExecutingAssembly().Location, builder.DataSource));
        }

        private void createTables()
        {
            SQLiteConnection conn = new SQLiteConnection(this.connectionString);

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "GameEndpoint.Data.Create_Tables.sql";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string sql = reader.ReadToEnd();

                conn.Open();
                conn.Execute(sql);
            }
        }
    }
}
