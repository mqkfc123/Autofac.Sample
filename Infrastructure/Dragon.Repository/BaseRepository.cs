using MySql.Data.MySqlClient;

namespace Dragon.Repository
{
    public class BaseRepository
    {
        private static string _conn = System.Configuration.ConfigurationManager.ConnectionStrings["DataConnectionString"].ToString();
        public static MySqlConnection GetConnect()
        {
            var conn = new MySqlConnection(_conn);
            conn.Open();
            return conn;
        }

    }
}
