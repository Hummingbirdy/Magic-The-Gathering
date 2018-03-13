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
        string GetDeckName(int deckId);
        int GetDeckLength(int deckId);
        string GetDeckColors(int deckId);
    }
    public class DeckDataRepository : IDeckDataRepository
    {
        public List<MyDecks> GetMyDecks(string userEmail)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            string sqlString = $@"SELECT ID FROM Users WHERE Email = '{userEmail}'";
            var id = db.Query<string>(sqlString).First();

            sqlString = $@"SELECT * FROM DeckNames WHERE UserId = {id} ";

            var decks = (List<MyDecks>)db.Query<MyDecks>(sqlString);
            return decks;
        }

        public string GetDeckName(int deckId)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            string sqlString = $@"SELECT DeckName FROM DeckNames WHERE ID = {deckId}";

            var name = db.Query<string>(sqlString).First();
            return name;
        }

        public int GetDeckLength(int deckId)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            string sqlString = $@"SELECT SUM(Amount) FROM Decks WHERE DeckId = {deckId}";

            var amount = db.Query<string>(sqlString).First();
            return amount ==  null ? 0 : Int32.Parse(amount);
        }

        public string GetDeckColors(int deckId)
        {
            var colors = "";
            var colorIcons = "";
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            string sqlString = $@"SELECT DISTINCT ColorIdentity FROM Decks d JOIN Cards c on c.Id = d.CardId  WHERE DeckId = {deckId}";

            List<string> identites = (List<string>)db.Query<string>(sqlString);

            identites.ForEach(i =>
            {
                colors = colors + i + " ";
            });

            if (colors.Contains("W"))
            {
                colorIcons = colorIcons + "<img src = '/Content/Img/Icons/White_Mana.png' /> ";
            }
            if (colors.Contains("U"))
            {
                colorIcons = colorIcons + "<img src = '/Content/Img/Icons/Blue_Mana.png' /> ";
            }
            if (colors.Contains("B"))
            {
                colorIcons = colorIcons + "<img src = '/Content/Img/Icons/Black_Mana.png' /> ";
            }
            if (colors.Contains("R"))
            {
                colorIcons = colorIcons + "<img src = '/Content/Img/Icons/Red_Mana.png' /> ";
            }
            if (colors.Contains("G"))
            {
                colorIcons = colorIcons + "<img src = '/Content/Img/Icons/Green_Mana.png' /> ";
            }

            return colorIcons;
        }
    }
}
