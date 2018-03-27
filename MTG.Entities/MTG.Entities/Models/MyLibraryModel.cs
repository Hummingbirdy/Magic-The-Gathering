using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Entities.Models
{
    public class MyLibraryModel
    {
        public List<Card> Cards { get; set; }
    }

    public class LibSet
    {
        public List<Card> Cards { get; set; }
        public string Name { get; set; }
        public bool Colapsed { get; set; }
    }
}
