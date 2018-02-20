using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MTG.Data.Repos
{
    public interface IRegexRepository
    {
        string AddIcons(string startString);
    }
    public class RegexRepository: IRegexRepository
    {
        public string AddIcons(string startString)
        {
            var endString = new StringBuilder(startString);
            endString.Replace("{W}", " <img src='/Content/Img/Icons/White_Mana.png'/> ");
            endString.Replace("{U}", " <img src='/Content/Img/Icons/Blue_Mana.png'/> ");
            endString.Replace("{B}", " <img src='/Content/Img/Icons/Black_Mana.png'/> ");
            endString.Replace("{R}", " <img src='/Content/Img/Icons/Red_Mana.png'/> ");
            endString.Replace("{G}", " <img src='/Content/Img/Icons/Green_Mana.png'/> ");
            endString.Replace("{T}", " <img src='/Content/Img/Icons/tap.png'/> ");

            endString.Replace("{1}", " <img src='/Content/Img/Icons/1.png'/> ");
            endString.Replace("{2}", " <img src='/Content/Img/Icons/2.png'/> ");
            endString.Replace("{3}", " <img src='/Content/Img/Icons/3.png'/> ");
            endString.Replace("{4}", " <img src='/Content/Img/Icons/4.png'/> ");
            endString.Replace("{5}", " <img src='/Content/Img/Icons/5.png'/> ");
            endString.Replace("{6}", " <img src='/Content/Img/Icons/6.png'/> ");
            endString.Replace("{7}", " <img src='/Content/Img/Icons/7.png'/> ");
            endString.Replace("{8}", " <img src='/Content/Img/Icons/8.png'/> ");
            endString.Replace("{9}", " <img src='/Content/Img/Icons/9.png'/> ");
            endString.Replace("{10}", " <img src='/Content/Img/Icons/10.png'/> ");
            endString.Replace("{11}", " <img src='/Content/Img/Icons/11.png'/> ");
            endString.Replace("{12}", " <img src='/Content/Img/Icons/12.png'/> ");
            endString.Replace("{13}", " <img src='/Content/Img/Icons/13.png'/> ");
            endString.Replace("{14}", " <img src='/Content/Img/Icons/14.png'/> ");
            endString.Replace("{15}", " <img src='/Content/Img/Icons/15.png'/> ");

            return endString.ToString();
        }
    }
}
