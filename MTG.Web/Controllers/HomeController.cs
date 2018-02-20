using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using MTG.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using MTG.Entities.Models;

namespace MTG.Controllers
{
    public interface IMessageProvider
    {
        string GetMessage();
    }
    public class MessageProvider : IMessageProvider
    {
        public string GetMessage()
        {
            return "Message from " + this.GetType();
        }
    }
    public class HomeController : Controller
    {
        private readonly IMessageProvider _messageProvider;
        public HomeController(IMessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
        }

        public ActionResult Index()
        {
            dbLogin(User.Identity.Name);
            return View();
        }

        public void dbLogin(string email)
        {
            try
            {
                IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                string SqlString = $@"SELECT * FROM Users WHERE email = '{email}'";
                var User = (List<User>)db.Query<User>(SqlString);

                if (User.Count() == 0)
                {
                    SqlString = $@"Insert INTO Users (Email) VALUES ('{email}')";
                    db.Query(SqlString);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = _messageProvider.GetMessage();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}