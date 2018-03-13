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
using MTG.Data.Repos;
using MTG.Entities.Models;

namespace MTG.Controllers
{
    public class DecksController : Controller
    {
        private readonly IDeckDataRepository _deckData;
        private readonly ICardDataRepository _cardData;

        public DecksController(IDeckDataRepository deckData, ICardDataRepository cardData)
        {
            _deckData = deckData;
            _cardData = cardData;
        }

        [Authorize]
        public ActionResult Index()
        {

            List<MyDecks> decks = _deckData.GetMyDecks(User.Identity.Name);
            decks.ForEach(d =>
            {
                d.URL = Url.Action("DeckDetail", "Decks", new { DeckId = d.Id });
                d.Colors = _deckData.GetDeckColors(d.Id);
            });

            var vm = new DeckModel()
            {
                Decks = decks,
            };

            return View(vm);
        }

        [Authorize]
        public ActionResult DeckDetail(int deckId)
        {
            List<Card> cards = _cardData.GetDeckCards(deckId);
            cards.ForEach(c =>
            {
                c.src = "http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=" + c.ID + "&type=card";
                c.url = Url.Action("CardDetail", "Cards", new { ID = c.ID });
                c.Hovering = false;
            });
            var planeswalkers = new List<Card>();
            var creatures = new List<Card>();
            var sorceries = new List<Card>();
            var instants = new List<Card>();
            var enchantments = new List<Card>();
            var artifacts = new List<Card>();
            var lands = new List<Card>();
            var other = new List<Card>();

            cards.ForEach(c =>
            {
                if (c.Type.Contains("Planeswalker"))
                {
                    planeswalkers.Add(c);
                }
                else if (c.Type.Contains("Creature"))
                {
                    creatures.Add(c);
                }
                else if (c.Type.Contains("Sorcery"))
                {
                    sorceries.Add(c);
                }
                else if (c.Type.Contains("Instant"))
                {
                    instants.Add(c);
                }
                else if (c.Type.Contains("Enchantment"))
                {
                    enchantments.Add(c);
                }
                else if (c.Type.Contains("Artifact"))
                {
                    artifacts.Add(c);
                }
                else if (c.Type.Contains("Land"))
                {
                    lands.Add(c);
                }
                else
                {
                    other.Add(c);
                }
            });
            var vm = new DeckDetailViewModel()
            {
                Cards = cards,
                Planeswalkers = planeswalkers,
                Creatures = creatures,
                Sorceries = sorceries,
                Instants = instants,
                Enchantments = enchantments,
                Artifacts = artifacts,
                Lands = lands,
                Other = other,
                Amount = _deckData.GetDeckLength(deckId),
                Name = _deckData.GetDeckName(deckId)
            };

            return View(vm);
        }
    }
}