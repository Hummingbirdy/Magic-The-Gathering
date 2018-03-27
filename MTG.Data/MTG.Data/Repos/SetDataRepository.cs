using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MTG.Entities.Models;
using System.Configuration;
using System.Linq;
using Dapper;

namespace MTG.Data.Repos
{
    public interface ISetDataRepository
    {
        List<Set> MostRecentExpansion();
        List<Set> GetAllSets(bool expansionsOnly = true);
        List<Set> GetMySets(int id);
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

        public List<Set> GetMySets(int id)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = @"	
                                SELECT
                                    DISTINCT

                                    s.[Code],
                                    s.Name,
                                    s.ReleaseDate
                                FROM
                                    Library l
                                    left join Cards c ON l.CardId = c.Id
                                    left join[Sets] s ON s.Code = c.[Set]

                                WHERE

                                    l.UserId in (@users)
                                    AND l.IsActive = 1

                                ORDER BY
                                    ReleaseDate DESC";

            var set = db.Query<Set>(sqlString, new {@user = "3, 4"}).ToList();

            return set;
        }
    }
}
