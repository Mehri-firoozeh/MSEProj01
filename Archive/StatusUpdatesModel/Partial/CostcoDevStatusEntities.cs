using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            entityBuilder.Metadata = @"res://*/StatusUpdatesModel.csdl|res://*/StatusUpdatesModel.ssdl|res://*/StatusUpdatesModel.msl";

            return new CostcoDevStatusEntities(entityBuilder.ConnectionString);
        }
    }
}
