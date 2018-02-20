using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MTG.Entities.Models;
using System.Configuration;
using Dapper;

namespace MTG.Data.Repos
{
    public interface ISetDataRepository
    {
        List<Set> MostRecentExpansion();
        List<Set> GetAllSets(bool expansionsOnly = true);
    }
    public class SetDataRepository : ISetDataRepository
    {
        public List<Set> MostRecentExpansion()
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = "SELECT TOP 1 * FROM [Sets] WHERE [Type] = 'expansion' ORDER BY ReleaseDate DESC";
            var set = (List<Set>)db.Query<Set>(sqlString);
            return set;
        }

        public List<Set> GetAllSets(bool expansionsOnly = true)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = expansionsOnly ? "SELECT * FROM [Sets] WHERE [Type] = 'expansion' ORDER BY ReleaseDate DESC" : "SELECT * FROM [Sets] ORDER BY ReleaseDate DESC";
            var set = (List<Set>)db.Query<Set>(sqlString);
            return set;
        }
    }
}
