using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using MTG.App_Start;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Web;

namespace MTG.DependencyResolution
{
    public class DatabaseRegistry : Registry
    {
        public DatabaseRegistry()
        {
            For<IDbConnection>()
                .Use(
                    c =>
                        new SqlConnection(
                            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)).Transient();

        }
    }
}