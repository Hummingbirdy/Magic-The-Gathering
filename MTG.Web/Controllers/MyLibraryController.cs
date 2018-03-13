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
    public class MyLibraryController : Controller
    {
        private readonly ISetDataRepository _setData;

        private readonly ICardDataRepository _cardData;

        public MyLibraryController(ISetDataRepository setData, ICardDataRepository cardData)
        {
            _setData = setData;
            _cardData = cardData;
        }
        // GET: MyLibrary
        [Authorize]
        public ActionResult Index()
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            string sqlString = $@"SELECT ID FROM Users WHERE Email = '{User.Identity.Name}'";
            var id = db.Query<int>(sqlString).First();

            List<Card> MyCards = _cardData.GetMyCards(id.ToString());
            List<Set> Sets = _setData.GetMySets(id);

            MyCards.ForEach(c =>
            {
                c.src = "http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=" + c.ID + "&type=card";
                c.url = Url.Action("CardDetail", "Cards", new { ID = c.ID });
                c.Hovering = false;
            });

            List<LibSet> FinalSets = new List<LibSet>();

            Sets.ForEach(s =>
            {
                List<Card> CardsInSet = new List<Card>();
                MyCards.ForEach(c =>
                {

                    if (s.Code == c.Set)
                    {
                        CardsInSet.Add(new Card() { Name = c.Name, ID = c.ID, src = c.src, url = c.url, Amount = c.Amount });
                    }
                });
               
                FinalSets.Add(new LibSet() { Cards = CardsInSet, Name = s.Name, Colapsed = false });
            });

            

            return View( new MyLibraryModel()
            {
                Sets = FinalSets
            });
        }
    }
}