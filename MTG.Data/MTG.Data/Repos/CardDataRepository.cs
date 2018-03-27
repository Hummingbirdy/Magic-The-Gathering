
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MTG.Entities.Models;
using System.Configuration;
using Dapper;

namespace MTG.Data.Repos
{
    public interface ICardDataRepository
    {
        List<Card> GetAllCards();
        List<Card> GetCard(int id);
        List<Card> GetMyCards(string id);
        List<Card> GetDeckCards(int deckId);
            // List<string> GetAllNames();
        List<string> GetAllTypes();
        List<string> GetAllSubTypes();
        List<string> GetAllRaritys();
        List<Card> GetSetSearch(string setCode);
        List<Card> GetSearchResults(AdvancedSearch parameters);
        CardAmounts GetCardAmountForUser(int cardId, int accountId);
        void UpdateDeckCardAmounts(List<CardAmountsPerDeck> decks, int cardId, int userId);
        void UpdateLibraryCardAmount(int cardId, int userId, int amount, int recordId);

    }

    public class CardDataRepository : ICardDataRepository
    {
        private readonly ISetDataRepository _setData;

        public CardDataRepository(ISetDataRepository setData)
        {
            _setData = setData;
        }
        public List<Card> GetAllCards()
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = $@"SELECT * FROM Cards WHERE [Set] = '{_setData.MostRecentExpansion().First().Code}' AND Number NOT LIKE '%b' ORDER BY ID";
            var cards = (List<Card>)db.Query<Card>(sqlString);
            return cards.ToList();
        }

        public List<Card> GetCard(int id)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = "SELECT c.*, SetName = s.name FROM Cards c join [Sets] s on s.code = c.[set] WHERE ID = " + id;
            var cards = (List<Card>)db.Query<Card>(sqlString);
            if (cards.First().Number.Contains('a'))
            {
                sqlString = "SELECT * FROM Cards WHERE ID = " + (id + 1);
                cards.AddRange((List<Card>)db.Query<Card>(sqlString));
            }
            return cards.ToList();
        }
        public List<Card> GetMyCards(string id)
        {
            id = (id == "3" || id == "4") ? "3, 4" : id;
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = $@"  
                                        SELECT
	                                        c.[Name],
                                            c.[Set],
                                            c.Id,
                                            l.amount
                                        FROM
	                                        Library l
	                                        left join Cards c on l.CardId = c.Id
	                                    Where 
                                            l.UserId IN ({id})
                                            AND l.IsActive = 1
                                        AND Amount > 0";
            var cards = (List<Card>)db.Query<Card>(sqlString);
            return cards.ToList();
        }

        public List<Card> GetDeckCards(int deckId)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            string sqlString = $@"SELECT Id, [Name], [Type], Amount FROM Decks d JOIN Cards c on c.Id = d.CardId WHERE DeckId = {deckId} and isactive = 1 Order by Id";

            var cards = (List<Card>)db.Query<Card>(sqlString);
            return cards;
        }

        //public List<string> GetAllNames()
        //{

        //    IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //    var SqlString = "SELECT DISTINCT [Name] from Cards ORDER BY Name";
        //    var types = (List<string>)db.Query<string>(SqlString);
        //    return types;
        //}

        public List<string> GetAllTypes()
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = "SELECT * FROM CardTypes";
            var types = (List<string>)db.Query<string>(sqlString);
            return types;
        }

        public List<string> GetAllSubTypes()
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = "SELECT * FROM SubTypes ORDER BY SubType";
            var types = (List<string>)db.Query<string>(sqlString);
            return types;
        }

        public List<string> GetAllRaritys()
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = "SELECT * FROM Raritys";
            var raritys = (List<string>)db.Query<string>(sqlString);
            return raritys;
        }

        public List<Card> GetSetSearch(string setCode)
        {

            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = $@"SELECT * FROM Cards WHERE [Set] = '{setCode}' AND Number NOT LIKE '%b' ORDER BY ID";
            var cards = (List<Card>)db.Query<Card>(sqlString);
            return cards.ToList();
        }

        public List<Card> GetSearchResults(AdvancedSearch parameters)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var clause = "WHERE Number NOT LIKE '%b' AND ID IS NOT NULL";
            if (parameters.SetCodes != null)
            {
                var count = 0;
                if (parameters.SetCodes.First().Operator == "AND")
                {
                    parameters.SetCodes.ForEach(c =>
                    {
                        clause = clause + $@" {c.Operator}{(count == 0 ? "(" : "")} [Set] = '{c.Code}'";
                        count++;
                    });
                }
                else
                {
                    parameters.SetCodes.ForEach(c =>
                    {
                        clause = clause + $@" AND{(count == 0 ? "(" : "")} [Set] != '{c.Code}'";
                        count++;
                    });
                }
                if (count > 0)
                {
                    clause = clause + ")";
                }
            }
            if (parameters.Names != null)
            {
                var count = 0;
                if (parameters.Names.First().Operator == "AND")
                {
                    parameters.Names.ForEach(n =>
                    {
                        clause = clause + $@" {n.Operator}{(count == 0 ? "(" : "")} [Name] = '{n.Value}'";
                        count++;
                    });
                }
                else
                {
                    parameters.Names.ForEach(n =>
                    {
                        clause = clause + $@" AND{(count == 0 ? "(" : "")} [Name] != '{n.Value}'";
                        count++;
                    });
                }
                if (count > 0)
                {
                    clause = clause + ")";
                }
            }
            if (parameters.Types != null)
            {
                var count = 0;
                if (parameters.Types.First().Operator == "AND")
                {
                    parameters.Types.ForEach(n =>
                    {
                        clause = clause + $@" {n.Operator}{(count == 0 ? "(" : "")} [Type] LIKE '%{n.Value}%'";
                        count++;
                    });
                }
                else
                {
                    parameters.Types.ForEach(n =>
                    {
                        clause = clause + $@" AND{(count == 0 ? "(" : "")} [Type] NOT LIKE '%{n.Value}%'";
                        count++;
                    });
                }
                if (count > 0)
                {
                    clause = clause + ")";
                }
            }
            if (parameters.SubTypes != null)
            {
                var count = 0;
                if (parameters.SubTypes.First().Operator == "AND")
                {
                    parameters.SubTypes.ForEach(n =>
                    {
                        clause = clause + $@" {n.Operator}{(count == 0 ? "(" : "")} SubTypes LIKE '%{n.Value}%'";
                        count++;
                    });
                }
                else
                {
                    parameters.SubTypes.ForEach(n =>
                    {
                        clause = clause + $@" AND{(count == 0 ? "(" : "")} SubTypes NOT LIKE '%{n.Value}%'";
                        count++;
                    });
                }
                if (count > 0)
                {
                    clause = clause + ")";
                }
            }
            if (parameters.CMCs != null)
            {
                var count = 0;
                if (parameters.CMCs.First().Operator == "AND")
                {
                    parameters.CMCs.ForEach(n =>
                    {
                        clause = clause + $@" {n.Operator}{(count == 0 ? "(" : "")} ConvertedManaCost {n.Comparison} '{n.Value}'";
                        count++;
                    });
                }
                else
                {
                    parameters.CMCs.ForEach(n =>
                    {
                        clause = clause + $@" AND{(count == 0 ? "(" : "")} ConvertedManaCost {n.Comparison} '{n.Value}'";
                        count++;
                    });
                }
                if (count > 0)
                {
                    clause = clause + ")";
                }
            }
            if (parameters.Powers != null)
            {
                var count = 0;
                if (parameters.Powers.First().Operator == "AND")
                {
                    parameters.Powers.ForEach(n =>
                    {
                        clause = clause + $@" {n.Operator}{(count == 0 ? "(" : "")} Power {n.Comparison} '{n.Value}'";
                        count++;
                    });
                }
                else
                {
                    parameters.Powers.ForEach(n =>
                    {
                        clause = clause + $@" AND{(count == 0 ? "(" : "")} Power {n.Comparison} '{n.Value}'";
                        count++;
                    });
                }
                if (count > 0)
                {
                    clause = clause + ")";
                }
            }
            if (parameters.Toughnesses != null)
            {
                var count = 0;
                if (parameters.Toughnesses.First().Operator == "AND")
                {
                    parameters.Toughnesses.ForEach(n =>
                    {
                        clause = clause + $@" {n.Operator}{(count == 0 ? "(" : "")} Toughness {n.Comparison} '{n.Value}'";
                        count++;
                    });
                }
                else
                {
                    parameters.Toughnesses.ForEach(n =>
                    {
                        clause = clause + $@" AND{(count == 0 ? "(" : "")} Toughness {n.Comparison} '{n.Value}'";
                        count++;
                    });
                }
                if (count > 0)
                {
                    clause = clause + ")";
                }
            }
            if (parameters.Raritys != null)
            {
                var count = 0;
                if (parameters.Raritys.First().Operator == "AND")
                {
                    parameters.Raritys.ForEach(n =>
                    {
                        clause = clause + $@" {n.Operator}{(count == 0 ? "(" : "")} Rarity = '{n.Value}'";
                        count++;
                    });
                }
                else
                {
                    parameters.Raritys.ForEach(n =>
                    {
                        clause = clause + $@" AND{(count == 0 ? "(" : "")} Rarity != '{n.Value}'";
                        count++;
                    });
                }
                if (count > 0)
                {
                    clause = clause + ")";
                }
            }
            if (parameters.Texts != null)
            {
                var count = 0;
                if (parameters.Texts.First().Operator == "AND")
                {
                    parameters.Texts.ForEach(n =>
                    {
                        clause = clause + $@" {n.Operator}{(count == 0 ? "(" : "")} ([Text] LIKE '%{n.Value}%' or Flavor Like '%{n.Value}%')";
                        count++;
                    });
                }
                else
                {
                    parameters.Texts.ForEach(n =>
                    {
                        clause = clause + $@" AND{(count == 0 ? "(" : "")} ([Text] NOT LIKE '%{n.Value}%' or Flavor NOT LIKE '%{n.Value}%')";
                        count++;
                    });
                }
                if (count > 0)
                {
                    clause = clause + ")";
                }
            }
            if (parameters.Artists != null)
            {
                var count = 0;
                if (parameters.Artists.First().Operator == "AND")
                {
                    parameters.Artists.ForEach(n =>
                    {
                        clause = clause + $@" {n.Operator}{(count == 0 ? "(" : "")} Artist LIKE '%{n.Value}%'";
                        count++;
                    });
                }
                else
                {
                    parameters.Artists.ForEach(n =>
                    {
                        clause = clause + $@" AND{(count == 0 ? "(" : "")} Artist NOT LIKE '%{n.Value}%'";
                        count++;
                    });
                }
                if (count > 0)
                {
                    clause = clause + ")";
                }
            }
            if (parameters.Colors != null)
            {
                var count = 0;
                if (parameters.Colors.White)
                {
                    clause = clause + $@" {(count == 0 ? "AND(" : parameters.Colors.Opp)} ColorIdentity LIKE '%W%'";
                    count++;
                }
                if (parameters.Colors.Blue)
                {
                    clause = clause + $@" {(count == 0 ? "AND(" : parameters.Colors.Opp)} ColorIdentity LIKE '%U%'";
                    count++;
                }
                if (parameters.Colors.Black)
                {
                    clause = clause + $@" {(count == 0 ? "AND(" : parameters.Colors.Opp)} ColorIdentity LIKE '%B%'";
                    count++;
                }
                if (parameters.Colors.Red)
                {
                    clause = clause + $@" {(count == 0 ? "AND(" : parameters.Colors.Opp)} ColorIdentity LIKE '%R%'";
                    count++;
                }
                if (parameters.Colors.Green)
                {
                    clause = clause + $@" {(count == 0 ? "AND(" : parameters.Colors.Opp)} ColorIdentity LIKE '%W%'";
                    count++;
                }
                if (parameters.Colors.Colorless)
                {
                    clause = clause + $@" {(count == 0 ? "AND(" : parameters.Colors.Opp)} ColorIdentity = 'null'" + " OR Text LIKE '%{C}%'";
                    count++;
                }
                if (count > 0)
                {
                    clause = clause + ")";
                }

            }
            string sqlString;
            if (parameters.InLibrary)
            {
                sqlString =
                   $@"With lib as (
                           Select 
                                CardId,
                                Amount = SUM(CAST(Amount as int))
                            From
                                Library
                            Group By

                                CardId
                            )
                            SELECT  distinct c.Id, c.[Name], l.Amount FROM Cards c LEFT JOIN Library l on l.CardId = c.Id " + clause + " and l.amount > 0  order by id";
            }
            else
            {
                sqlString = $@"SELECT * FROM Cards " + clause + " ORDER BY Id";
            }


            var cards = (List<Card>)db.Query<Card>(sqlString);
            return cards.ToList();
        }

        public CardAmounts GetCardAmountForUser(int cardId, int userId)
        {
            string users  = (userId == 3 || userId == 4) ? "3, 4" : userId.ToString();
            var amounts = new CardAmounts();
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = $@"SELECT Name FROM Cards WHERE ID = {cardId}";
            amounts.CardId = cardId;
            amounts.CardName = ((List<string>)db.Query<string>(sqlString)).FirstOrDefault();
            sqlString = $@"SELECT Amount FROM Library WHERE UserId IN ({users}) AND CardId = {cardId} AND IsActive = 1";
            amounts.LibraryAmount = db.Query<int>(sqlString).FirstOrDefault();
            sqlString = $@"SELECT LibraryId FROM Library WHERE UserId IN ({users}) AND CardId = {cardId} AND IsActive = 1";
            amounts.CurrentRecordId = db.Query<int>(sqlString).FirstOrDefault();
            amounts.OrigionalLibraryAmount = amounts.LibraryAmount;
            sqlString = $@"SELECT n.Id AS DeckId, DeckName, ISNULL(Amount, 0) AS DeckAmount, ISNULL(Amount,0) AS OrigionalDeckAmount, DecksId AS CurrentRecordId FROM DeckNames n LEFT JOIN Decks d ON d.DeckId = n.ID AND d.CardId = {cardId} AND d.IsActive = 1 WHERE n.UserId = {userId}";
            amounts.DeckInfo = ((List<CardAmountsPerDeck>)db.Query<CardAmountsPerDeck>(sqlString)).ToList();
            amounts.DeckInfo.Add(new CardAmountsPerDeck { CurrentRecordId = 0, DeckId = 0, DeckName = "", DeckAmount = 0, OrigionalDeckAmount = 0 });
            return amounts;
        }

        public void UpdateLibraryCardAmount(int cardId, int userId, int amount, int recordId)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = $@"";
            if (recordId != 0)
            {
                sqlString = $@"UPDATE Library SET IsActive = 0, ModifiedDate = GETDATE() WHERE LibraryId = {recordId} ";
            }

            sqlString = sqlString + $@"INSERT INTO Library VALUES ({userId}, {cardId}, {amount}, 1, GETDATE(), GETDATE()) ";
            db.Query(sqlString);
        }

        public void UpdateDeckCardAmounts(List<CardAmountsPerDeck> decks, int cardId, int userId)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = $@"";
            decks.ForEach(d =>
            {
                if (d.DeckAmount != d.OrigionalDeckAmount)
                {
                    if (d.CurrentRecordId != 0)
                    {
                        sqlString = sqlString + $@"Update Decks Set IsActive = 0, ModifiedDate = GETDATE() WHERE DecksId = {d.CurrentRecordId} ";
                    }
                    if (d.DeckId != 0)
                    {
                        sqlString = sqlString + $@"INSERT INTO Decks VALUES({d.DeckId}, {cardId}, {d.DeckAmount}, 1, GETDATE(), GETDATE()) ";
                    }
                    else
                    {
                        var newDeckId = InsertNewDeck(d.DeckName, userId);
                        sqlString = sqlString + $@"INSERT INTO Decks VALUES({newDeckId}, {cardId}, {d.DeckAmount}, 1, GETDATE(), GETDATE()) ";
                    }
                }
            });
            db.Query(sqlString);

        }

        int InsertNewDeck(string deckName, int userId)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var sqlString = $@"INSERT INTO DeckNames VALUES( {userId}, '{deckName}') " +
                            $@"SELECT Id FROM DeckNames WHERE DeckName = '{deckName}' AND UserId = {userId}";
            return db.Query<int>(sqlString).FirstOrDefault();
        }
    }
}



