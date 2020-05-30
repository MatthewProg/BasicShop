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
            entityBuilder.ProviderConnectionString = "data source=SQL5053.site4now.net;initial catalog=DB_A61718_basic;user id=DB_A61718_basic_admin;password=mowarer121;MultipleActiveResultSets=True;App=EntityFramework;";
            entityBuilder.Metadata = @"res://*/FullDatabase.csdl|res://*/FullDatabase.ssdl|res://*/FullDatabase.msl";

            return entityBuilder.ToString();
        }
    }
}
