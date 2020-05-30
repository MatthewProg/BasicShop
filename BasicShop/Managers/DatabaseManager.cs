using System.Data.SqlClient;

namespace BasicShop.Managers
{
    public class DatabaseManager
    {
        public static bool CheckConnection()
        {
            try
            {
                var ctx = new shopEntities(DatabaseHelper.GetConnectionString());
                ctx.Database.Connection.Open();
                ctx.Database.Connection.Close();
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }
    }
}
