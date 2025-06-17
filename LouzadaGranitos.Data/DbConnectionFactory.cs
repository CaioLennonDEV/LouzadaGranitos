using MySql.Data.MySqlClient;

namespace LouzadaGranitos.Data
{
    public static class DbConnectionFactory
    {
        public static MySqlConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}