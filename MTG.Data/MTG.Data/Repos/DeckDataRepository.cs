﻿using System;
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
    public interface IDeckDataRepository : IGenericRepository<MyDecks>
    {
        List<MyDecks> GetMyDecks(string user);
        string GetDeckName(int deckId);
        int GetDeckLength(int deckId);
        string GetDeckColors(int deckId);
    }
    public class DeckDataRepository : GenericRepository<MyDecks>, IDeckDataRepository
    {
        public DeckDataRepository(IDbTransaction transaction) : base(transaction)
        {

        }
        public List<MyDecks> GetMyDecks(string userEmail)
        {
            string sqlString = $@"SELECT ID FROM Users WHERE Email = '{userEmail}'";
            var id = Connection.Query<string>(sqlString, transaction:Transaction).First();

            sqlString = $@"SELECT * FROM DeckNames WHERE UserId = {id} ";

            var decks = Connection.Query<MyDecks>(sqlString, transaction:Transaction);
            return decks.ToList();
        }

        public string GetDeckName(int deckId)
        {
            string sqlString = $@"SELECT DeckName FROM DeckNames WHERE ID = {deckId}";

            var name = Connection.Query<string>(sqlString, transaction:Transaction).First();
            return name;
        }

        public int GetDeckLength(int deckId)
        {
            string sqlString = $@"SELECT SUM(Amount) FROM Decks WHERE DeckId = {deckId}";

            var amount = Connection.Query<string>(sqlString, transaction:Transaction).First();
            return amount ==  null ? 0 : int.Parse(amount);
        }

        public string GetDeckColors(int deckId)
        {
            var colors = "";
            var colorIcons = "";

            string sqlString = $@"SELECT DISTINCT ColorIdentity FROM Decks d JOIN Cards c on c.Id = d.CardId  WHERE DeckId = {deckId}";

            var identites = Connection.Query<string>(sqlString, transaction:Transaction).ToList();

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
