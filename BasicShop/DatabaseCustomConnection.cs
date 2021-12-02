using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;

namespace BasicShop
{
    public partial class shopEntities : DbContext
    {
        public shopEntities(string ConnectionString)
            : base(ConnectionString)
        {
        }
    }

    public class DatabaseHelper
    {
        public static string GetConnectionString()
        {
            var entityBuilder = new EntityConnectionStringBuilder();

            entityBuilder.Provider = "System.Data.SqlClient";
            entityBuilder.ProviderConnectionString = "Data Source=LAPTOP-HFC8SPKF\\SQLEXPRESS;Database=shop_database;User Id=shop_admin;Password=Admin12345;MultipleActiveResultSets=True;App=EntityFramework;";
            entityBuilder.Metadata = @"res://*/FullDatabase.csdl|res://*/FullDatabase.ssdl|res://*/FullDatabase.msl";

            return entityBuilder.ToString();
        }
    }
}
