using Dapper;
using MTG.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MTG.Entities.Models;

namespace MTG.Controllers
{
    public class DecksController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            List<MyDecks> Decks = GetMyDecks();
            Decks.ForEach(d =>
            {
                d.URL = Url.Action("DeckDetail", "Decks", new { DeckId = d.Id });
                d.Colors = GetDeckColors(d.Id);
            });

            var vm = new DeckModel()
            {
                Decks = Decks,
            };

            return View(vm);
        }

        [Authorize]
        public ActionResult DeckDetail(int DeckId)
        {
            List<Card> Cards = GetDeckCards(DeckId);
            Cards.ForEach(c =>
            {
                c.src = "http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=" + c.ID + "&type=card";
                c.url = Url.Action("CardDetail", "Cards", new { ID = c.ID });
                c.Hovering = false;
            });
            var Planeswalkers = new List<Card>();
            var Creatures = new List<Card>();
            var Sorceries = new List<Card>();
            var Instants = new List<Card>();
            var Enchantments = new List<Card>();
            var Artifacts = new List<Card>();
            var Lands = new List<Card>();
            var Other = new List<Card>();

            Cards.ForEach(c =>
            {
                if (c.Type.Contains("Planeswalker"))
                {
                    Planeswalkers.Add(c);
                }
                else if (c.Type.Contains("Creature"))
                {
                    Creatures.Add(c);
                }
                else if (c.Type.Contains("Sorcery"))
                {
                    Sorceries.Add(c);
                }
                else if (c.Type.Contains("Instant"))
                {
                    Instants.Add(c);
                }
                else if (c.Type.Contains("Enchantment"))
                {
                    Enchantments.Add(c);
                }
                else if (c.Type.Contains("Artifact"))
                {
                    Artifacts.Add(c);
                }
                else if (c.Type.Contains("Land"))
                {
                    Lands.Add(c);
                }
                else
                {
                    Other.Add(c);
                }
            });
            var vm = new DeckDetailViewModel()
            {
                Cards = Cards,
                Planeswalkers = Planeswalkers,
                Creatures = Creatures,
                Sorceries = Sorceries,
                Instants = Instants,
                Enchantments = Enchantments,
                Artifacts = Artifacts,
                Lands = Lands,
                Other = Other,
                Amount = GetDeckLength(DeckId),
                Name = GetDeckName(DeckId)
            };

            return View(vm);
        }

        public List<MyDecks> GetMyDecks()
        {
            try
            {
                IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                string SqlString = $@"SELECT ID FROM Users WHERE Email = '{User.Identity.Name}'";
                var Id = db.Query<string>(SqlString).First();

                SqlString = $@"SELECT * FROM DeckNames WHERE UserId = {Id} ";

                var decks = (List<MyDecks>)db.Query<MyDecks>(SqlString);
                return decks;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Card> GetDeckCards(int DeckId)
        {
            try
            {
                IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                string SqlString = $@"SELECT Id, [Name], [Type], Amount = Sum(amount) FROM Decks d JOIN Cards c on c.Id = d.CardId WHERE DeckId = {DeckId} GROUP BY Id, [Name], [Type] Order by Id";

                var cards = (List<Card>)db.Query<Card>(SqlString);
                return cards;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetDeckName(int DeckId)
        {
            try
            {
                IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                string SqlString = $@"SELECT DeckName FROM DeckNames WHERE ID = {DeckId}";

                var Name = db.Query<string>(SqlString).First();
                return Name;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetDeckLength(int DeckId)
        {
            try
            {
                IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                string SqlString = $@"SELECT SUM(Amount) FROM Decks WHERE DeckId = {DeckId}";

                var Amount = db.Query<string>(SqlString).First();
                return Int32.Parse(Amount);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetDeckColors(int DeckId)
        {
            try
            {
                var Colors = "";
                var ColorIcons = "";
                IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                string SqlString = $@"SELECT DISTINCT ColorIdentity FROM Decks d JOIN Cards c on c.Id = d.CardId  WHERE DeckId = {DeckId}";

                List<string> Identites = (List<string>)db.Query<string>(SqlString);

                Identites.ForEach(i =>
                {
                   Colors = Colors + i + " ";
                });

                if (Colors.Contains("W"))
                {
                    ColorIcons = ColorIcons + "<img src = '/Content/Img/Icons/White_Mana.png' /> ";
                }
                if (Colors.Contains("U"))
                {
                    ColorIcons = ColorIcons + "<img src = '/Content/Img/Icons/Blue_Mana.png' /> ";
                }
                if (Colors.Contains("B"))
                {
                    ColorIcons = ColorIcons + "<img src = '/Content/Img/Icons/Black_Mana.png' /> ";
                }
                if (Colors.Contains("R"))
                {
                    ColorIcons = ColorIcons + "<img src = '/Content/Img/Icons/Red_Mana.png' /> ";
                }
                if (Colors.Contains("G"))
                {
                    ColorIcons = ColorIcons + "<img src = '/Content/Img/Icons/Green_Mana.png' /> ";
                }

                return ColorIcons;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}