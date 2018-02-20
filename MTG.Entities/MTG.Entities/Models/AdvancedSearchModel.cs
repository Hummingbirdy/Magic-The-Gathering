using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Entities.Models
{
    public class AdvancedSearch
    {
        public List<SearchList> SetCodes { get; set; }
        public List<SearchList> Names { get; set; }
        public List<SearchList> Types { get; set; }
        public List<SearchList> SubTypes { get; set; }
        public List<SearchList> CMCs { get; set; }
        public List<SearchList> Powers { get; set; }
        public List<SearchList> Toughnesses { get; set; }
        public List<SearchList> Raritys { get; set; }
        public List<SearchList> Texts { get; set; }
        public List<SearchList> Artists { get; set; }
        public ColorSelector Colors { get; set; }
        public string ManaOperator { get; set; }
        public bool InLibrary { get; set; }
    }

    public class ColorSelector
    {
        public bool White { get; set; }
        public bool Blue { get; set; }
        public bool Black { get; set; }
        public bool Red { get; set; }
        public bool Green { get; set; }
        public bool Colorless { get; set; }

        public string Opp { get; set; }
    }
}
