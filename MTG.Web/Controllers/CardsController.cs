using Dapper;
using MTG.ActionResults;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MTG.Data.Repos;
using MTG.Entities.Models;

namespace MTG.Controllers
{
   
    public class CardsController : Controller
    {
        private readonly ICardDataRepository _cardData;
        private readonly ISetDataRepository _setData;
        private readonly IDeckDataRepository _deckData;
        private readonly IRegexRepository _regex;

        public CardsController(ICardDataRepository cardData, ISetDataRepository setData, IDeckDataRepository deckData, IRegexRepository regex)
        {
            _cardData = cardData;
            _setData = setData;
            _deckData = deckData;
            _regex = regex;
        }
        public ActionResult Index()
        {

            var cards = _cardData.GetAllCards();

            cards.ForEach(c =>
            {
                c.src = "http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=" + c.ID + "&type=card";
                c.url = Url.Action("CardDetail", "Cards", new { ID = c.ID });
                c.Hovering = false;
            });

            var vm = new AllCardsHomeModel()
            {
                Title = _setData.MostRecentExpansion().First().Name,
                AdvancedSearch = false,
                Search = true,
                SelectedCard = 0,
                Sets = _setData.GetAllSets(),
                AllSets = _setData.GetAllSets(false),
                SetOperator = "AND",
                SetList = new List<SearchList>(),
                AllNames = null,
                NameOperator = "AND",
                NameList = new List<SearchList>(),
                AllTypes = _cardData.GetAllTypes(),
                TypeOperator = "AND",
                TypeList = new List<SearchList>(),
                AllSubTypes = _cardData.GetAllSubTypes(),
                SubTypeOperator = "AND",
                SubTypeList = new List<SearchList>(),
                ManaCostOperator = "AND",
                ManaCostComparison = "=",
                ManaCostList = new List<SearchList>(),                
                PowerOperator = "AND",
                PowerComparison = "=",
                PowerList = new List<SearchList>(),
                ToughnessOperator = "AND",
                ToughnessComparison = "=",
                ToughnessList = new List<SearchList>(),
                AllRaritys = _cardData.GetAllRaritys(),
                RarityOperator = "AND",
                RarityList = new List<SearchList>(),
                TextOperator = "AND",
                TextList = new List<SearchList>(),
                ArtistOperator = "AND",
                ArtistList = new List<SearchList>(),

                Colors = new ColorSelector()
                {
                    White = false,
                    Blue = false,
                    Black = false,
                    Red = false,
                    Green = false,
                    Colorless = false
                },
                ManaOperator = "OR",
                Cards = cards,
                Decks = _deckData.GetMyDecks(User.Identity.Name),
                Loading = true,
                CardAmounts = new CardAmounts()

            };

            return View(vm);
        }

        public ActionResult CardDetail(int id)
        {
            var card = _cardData.GetCard(id);
            card.ForEach(c =>
            {
                c.ManaCost = _regex.AddIcons(c.ManaCost);
                c.Text = _regex.AddIcons(c.Text);
                c.src = "http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=" + c.ID + "&type=card";
            });
            var vm = new CardDetailViewModel()
            {
                Cards = card
            };
            return View(vm);
        }

        [HttpGet]
        public JsonResult Search(string setCode)
        {
            try
            {
                List<Card> results = _cardData.GetSetSearch(setCode);
                results.ForEach(c =>
                {
                    c.src = "http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=" + c.ID + "&type=card";
                    c.url = Url.Action("CardDetail", "Cards", new { ID = c.ID });
                    c.Hovering = false;
                });

                return new CustomJsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new CustomJsonResult(new List<string> { ex.Message });
            }
        }
        [HttpPost]
        public JsonResult AdvancedSearch(AdvancedSearch searchParameters)
        {
            try
            {
                List<Card> results = _cardData.GetSearchResults(searchParameters);

                results.ForEach(c =>
                {
                    c.src = "http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=" + c.ID + "&type=card";
                    c.url = Url.Action("CardDetail", "Cards", new { ID = c.ID });
                    c.Hovering = false;
                });


                return new CustomJsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new CustomJsonResult(new List<string> { ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetAddInfo(int cardId)
        {
            try
            {
                IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                string idCheck = $@"SELECT ID FROM Users WHERE Email = '{User.Identity.Name}'";
                var id = db.Query<int>(idCheck).First();
                CardAmounts results = _cardData.GetCardAmountForUser(cardId, id );

                return new CustomJsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new CustomJsonResult(new List<string> { ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddCard(string cardNumber, List<int> where, int howMany, string newDeck)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return new CustomJsonResult(new List<string> { "Please Log In " });
                }
                else
                {
                    IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                    string idCheck = $@"SELECT ID FROM Users WHERE Email = '{User.Identity.Name}'";

                    var id = db.Query<string>(idCheck).First();

                    if (newDeck != "")
                    {
                        string addDeck = $@"INSERT INTO DeckNames (UserId, DeckName) VALUES ({id}, '{newDeck}') 
                                            SELECT Id FROM DeckNames Where DeckName = '{newDeck}'";
                        var deckId = db.Query<string>(addDeck).First();

                        var insert = $@"INSERT INTO Decks (DeckId, CardId, Amount) Values ({deckId}, {cardNumber}, {howMany}) ";
                        db.Query(insert);
                    }

                    
                    string inserts = "";

                    where.ForEach(n =>
                   {
                       if (n == 0)
                       {
                           if(id == "3" || id == "4")
                           { 
                                inserts = inserts + $@"INSERT INTO Library (UserId, CardId, Amount) Values (3, {cardNumber}, {howMany}) INSERT INTO Library (UserId, CardId, Amount) Values (4, {cardNumber}, {howMany})";
                           }
                           else
                           {
                               inserts = inserts + $@"INSERT INTO Library(UserId, CardId, Amount) Values({ id}, { cardNumber}, { howMany})";
                           }
                       }
                       else
                       {
                           inserts = inserts + $@"INSERT INTO Decks (DeckId, CardId, Amount) Values ({n}, {cardNumber}, {howMany}) ";
                       }
                   });

                    
                    db.Query(inserts);
                }


                return new CustomJsonResult { Data = "test", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new CustomJsonResult(new List<string> { ex.Message });
            }
        }     
    }
}