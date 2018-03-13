using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Entities.Models
{
    public class CardDetailViewModel
    {
        public List<Card> Cards { get; set; }
    }

    public class CardAmounts
    {
        public string CardName { get; set; }
        public int LibraryAmount { get; set; }
        public int OrigionalLibraryAmount { get; set; }
        public int CurrentRecordId { get; set; }
        public List<CardAmountsPerDeck> DeckInfo { get; set;}
    }

    public class CardAmountsPerDeck
    {
        public int DeckId { get; set; }
        public string DeckName { get; set; }
        public int DeckAmount { get; set; }      
        public int OrigionalDeckAmount { get; set; }
        public int CurrentRecordId { get; set; }
    }
}
