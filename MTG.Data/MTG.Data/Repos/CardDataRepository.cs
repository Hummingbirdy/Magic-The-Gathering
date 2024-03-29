﻿
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
    public interface ICardDataRepository : IGenericRepository<Card>
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
        List<Card> GetSearchResults(AdvancedSearch parameters, int id);
        CardAmounts GetCardAmountForUser(int cardId, int accountId);
        void UpdateDeckCardAmounts(List<CardAmountsPerDeck> decks, int cardId, int userId);
        void UpdateLibraryCardAmount(int cardId, int userId, int amount, int recordId);

    }

    public class CardDataRepository : GenericRepository<Card>, ICardDataRepository
    {
        private readonly ISetDataRepository _setData;

        public CardDataRepository(IDbTransaction transaction, ISetDataRepository setData)  : base(transaction)
        {
            _setData = setData;
        }
        public List<Card> GetAllCards()
        {
            var cards = Connection.Query<Card>($@"SELECT * FROM Cards WHERE [Set] = '{_setData.MostRecentExpansion().First().Code}' AND Number NOT LIKE '%b' ORDER BY ID", transaction: Transaction);
            return cards.ToList();
        }

        public List<Card> GetCard(int id)
        {
            var cards = Connection.Query<Card>("SELECT c.*, SetName = s.name FROM Cards c join [Sets] s on s.code = c.[set] WHERE ID = " + id, transaction: Transaction).ToList();
            if (cards.First().Number.Contains('a'))
            {
                var newCard = Connection.Query<Card>("SELECT c.*, SetName = s.name FROM Cards c join [Sets] s on s.code = c.[set] WHERE ID = " + (id + 1), transaction: Transaction).ToList();
                cards.AddRange(newCard);
            }
            return cards.ToList();
        }
        public List<Card> GetMyCards(string id)
        {
            id = (id == "3" || id == "4") ? "3, 4" : id;
            var cards = Connection.Query<Card>($@"  
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
                                        AND Amount > 0", transaction: Transaction);
            return cards.ToList();
        }

        public List<Card> GetDeckCards(int deckId)
        {
            var cards = Connection.Query<Card>($@"SELECT Id, [Name], [Type], Amount FROM Decks d JOIN Cards c on c.Id = d.CardId WHERE DeckId = {deckId} and isactive = 1 Order by Id", transaction : Transaction);
            return cards.ToList();
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
            var types = Connection.Query<string>("SELECT * FROM CardTypes", transaction:Transaction);
            return types.ToList();
        }

        public List<string> GetAllSubTypes()
        {
            var types = Connection.Query<string>("SELECT * FROM SubTypes ORDER BY SubType", transaction:Transaction);
            return types.ToList();
        }

        public List<string> GetAllRaritys()
        {
            var raritys = Connection.Query<string>("SELECT * FROM Raritys", transaction: Transaction);
            return raritys.ToList();
        }

        public List<Card> GetSetSearch(string setCode)
        {
            var cards = Connection.Query<Card>($@"SELECT * FROM Cards WHERE [Set] = '{setCode}' AND Number NOT LIKE '%b' ORDER BY cast(dbo.fnRemoveNonNumericCharacters(Number)  as int)", transaction:Transaction);
            return cards.ToList();
        }

        public List<Card> GetSearchResults(AdvancedSearch parameters, int id)
        {
            string users = (id == 3 || id == 4) ? "3, 4" : id.ToString();

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
                                Amount 
                            From
                                Library
                            )
                            SELECT  distinct c.Id, c.[Name], l.Amount FROM Cards c LEFT JOIN Library l on l.CardId = c.Id {clause} and l.amount > 0 and l.IsActive = 1 and UserId in ({users}) order by id";
            }
            else
            {
                sqlString = $@"SELECT * FROM Cards " + clause + " ORDER BY Id";
            }

            var cards = Connection.Query<Card>(sqlString, transaction:Transaction);
            return cards.ToList();
        }

        public CardAmounts GetCardAmountForUser(int cardId, int userId)
        {
            string users  = (userId == 3 || userId == 4) ? "3, 4" : userId.ToString();
            var amounts = new CardAmounts();
            var sqlString = $@"SELECT Name FROM Cards WHERE ID = {cardId}";
            amounts.CardId = cardId;
            amounts.CardName = Connection.Query<string>(sqlString, transaction:Transaction).FirstOrDefault();
            sqlString = $@"SELECT Amount FROM Library WHERE UserId IN ({users}) AND CardId = {cardId} AND IsActive = 1";
            amounts.LibraryAmount = Connection.Query<int>(sqlString, transaction:Transaction).FirstOrDefault();
            sqlString = $@"SELECT LibraryId FROM Library WHERE UserId IN ({users}) AND CardId = {cardId} AND IsActive = 1";
            amounts.CurrentRecordId = Connection.Query<int>(sqlString, transaction:Transaction).FirstOrDefault();
            amounts.OrigionalLibraryAmount = amounts.LibraryAmount;
            sqlString = $@"SELECT n.Id AS DeckId, DeckName, ISNULL(Amount, 0) AS DeckAmount, ISNULL(Amount,0) AS OrigionalDeckAmount, DecksId AS CurrentRecordId FROM DeckNames n LEFT JOIN Decks d ON d.DeckId = n.ID AND d.CardId = {cardId} AND d.IsActive = 1 WHERE n.UserId = {userId}";
            amounts.DeckInfo = Connection.Query<CardAmountsPerDeck>(sqlString,transaction:Transaction).ToList();
            amounts.DeckInfo.Add(new CardAmountsPerDeck { CurrentRecordId = 0, DeckId = 0, DeckName = "", DeckAmount = 0, OrigionalDeckAmount = 0 });
            return amounts;
        }

        public void UpdateLibraryCardAmount(int cardId, int userId, int amount, int recordId)
        {
            var sqlString = $@"";
            if (recordId != 0)
            {
                sqlString = $@"UPDATE Library SET IsActive = 0, ModifiedDate = GETDATE() WHERE LibraryId = {recordId} ";
            }

            sqlString = sqlString + $@"INSERT INTO Library VALUES ({userId}, {cardId}, {amount}, 1, GETDATE(), GETDATE()) ";
            Connection.Query(sqlString, transaction:Transaction);
        }

        public void UpdateDeckCardAmounts(List<CardAmountsPerDeck> decks, int cardId, int userId)
        {
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
            Connection.Query(sqlString, transaction:Transaction);

        }

        int InsertNewDeck(string deckName, int userId)
        {
            var sqlString = $@"INSERT INTO DeckNames VALUES( {userId}, '{deckName}') " +
                            $@"SELECT Id FROM DeckNames WHERE DeckName = '{deckName}' AND UserId = {userId}";
            return Connection.Query<int>(sqlString, transaction:Transaction).FirstOrDefault();
        }
    }
}



