using System.Configuration;

namespace MTG.Utilities
{
    public class Constants
    {
        public static string ConnectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}