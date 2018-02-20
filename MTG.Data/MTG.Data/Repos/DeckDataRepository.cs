using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTG.Entities.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace MTG.Data.Repos
{
    public interface IDeckDataRepository
    {
        List<MyDecks> GetMyDecks(string user);
    }
    public class DeckDataRepository : IDeckDataRepository
    {
        public List<MyDecks> GetMyDecks(string userEmail)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            string sqlString = $@"SELECT ID FROM Users WHERE Email = '{userEmail}'";
            var Id = db.Query<string>(sqlString).First();

            sqlString = $@"SELECT * FROM DeckNames WHERE UserId = {Id} ";

            var decks = (List<MyDecks>)db.Query<MyDecks>(sqlString);
            return decks;
        }
    }
}
