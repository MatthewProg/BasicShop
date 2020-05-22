using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop
{
    public class DatabaseManager
    {
        public static bool CheckConnection()
        {
            try
            {
                var ctx = new shopEntities();
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
