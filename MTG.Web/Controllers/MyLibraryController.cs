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
    public class MyLibraryController : Controller
    {
        // GET: MyLibrary
        [Authorize]
        public ActionResult Index()
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            string SqlString = $@"SELECT ID FROM Users WHERE Email = '{User.Identity.Name}'";
            var Id = db.Query<string>(SqlString).First();

            List<Card> MyCards = GetMyCards(Id);
            List<Set> Sets = GetMySets(Id);

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

        public List<Set> GetMySets(string Id)
        {
            try
            {
                IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                string SqlString = $@"  
                                        SELECT
                                            DISTINCT
	                                        s.[Code],
                                            s.Name,
                                            s.ReleaseDate
                                        FROM
	                                        Library l
	                                        left join Cards c on l.CardId = c.Id
                                            left join [Sets] s on s.Code = c.[Set]
	                                    Where 
                                            l.UserId = '{Id}'
                                        Order BY 
                                            ReleaseDate DESC";

                var set = (List<Set>)db.Query<Set>(SqlString);
                return set;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Card> GetMyCards(string Id)
        {
            try
            {
                IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                string SqlString = $@"  
                                        SELECT
	                                        c.[Name],
                                            c.[Set],
                                            c.Id,
                                            Amount = sum(cast(l.amount as int))

                                        FROM
	                                        Library l
	                                        left join Cards c on l.CardId = c.Id
	                                    Where 
                                            l.UserId = '{Id}'
                                        Group By
                                            c.[Name],
                                            c.[Set],
                                            c.Id";
                var cards = (List<Card>)db.Query<Card>(SqlString);
                return cards.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        

    }
}