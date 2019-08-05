using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;

namespace StatusUpdatesModel
{
    public partial class CostcoDevStatusEntities : DbContext
    {

        public CostcoDevStatusEntities(string connection)
                : base(connection)
        { }

        public static CostcoDevStatusEntities Create(string providerConnectionString)
        {
            var entityBuilder = new EntityConnectionStringBuilder();

            // use your ADO.NET connection string
            entityBuilder.ProviderConnectionString = providerConnectionString;

            entityBuilder.Provider = "System.Data.SqlClient";

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/";

            return new CostcoDevStatusEntities(entityBuilder.ConnectionString);
        }
    }
}

