using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MTG.Entities.Models;
using System.Configuration;
using System.Linq;
using Dapper;

namespace MTG.Data.Repos
{
    public interface ISetDataRepository : IGenericRepository<Set>
    {
        List<Set> MostRecentExpansion();
        List<Set> GetAllSets(bool expansionsOnly = true);
        List<Set> GetMySets(int id);
    }
    public class SetDataRepository : GenericRepository<Set>, ISetDataRepository
    {
        public SetDataRepository(IDbTransaction transaction) : base(transaction)
        {

        }
        public List<Set> MostRecentExpansion()
        {
            var set = Connection.Query<Set>("SELECT TOP 1 * FROM [Sets] WHERE [Type] = 'expansion' ORDER BY ReleaseDate DESC", transaction:Transaction);
            return set.ToList();
        }

        public List<Set> GetAllSets(bool expansionsOnly = true)
        {
            var sqlString = expansionsOnly ? "SELECT * FROM [Sets] WHERE [Type] = 'expansion' ORDER BY ReleaseDate DESC" : "SELECT * FROM [Sets] ORDER BY ReleaseDate DESC";
            var set = Connection.Query<Set>(sqlString, transaction:Transaction);
            return set.ToList();
        }

        public List<Set> GetMySets(int id)
        {
            var users = id.ToString();
            users = (users == "3" || users == "4") ? "3, 4" : users;
            var sqlString = $@"	
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
                                    l.UserId in ({users})
                                    AND l.IsActive = 1
                                ORDER BY
                                    ReleaseDate DESC";

            var set = Connection.Query<Set>(sqlString, transaction:Transaction).ToList();
            return set;
        }
    }
}
