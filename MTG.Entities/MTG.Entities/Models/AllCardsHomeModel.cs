using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Entities.Models
{
    public class AllCardsHomeModel
    {
        public List<Card> Cards { get; set; }
        public List<Set> Sets { get; set; }
        public List<Set> AllSets { get; set; }
        public string SetOperator { get; set; }
        public List<SearchList> SetList { get; set; }
        public List<string> AllNames { get; set; }
        public string NameOperator { get; set; }
        public List<SearchList> NameList { get; set; }
        public List<string> AllTypes { get; set; }
        public string TypeOperator { get; set; }
        public List<SearchList> TypeList { get; set; }
        public List<string> AllSubTypes { get; set; }
        public string SubTypeOperator { get; set; }
        public List<SearchList> SubTypeList { get; set; }
        public string ManaCostOperator { get; set; }
        public string ManaCostComparison { get; set; }
        public List<SearchList> ManaCostList { get; set; }
        public string PowerOperator { get; set; }
        public string PowerComparison { get; set; }
        public List<SearchList> PowerList { get; set; }
        public string ToughnessOperator { get; set; }
        public string ToughnessComparison { get; set; }
        public List<SearchList> ToughnessList { get; set; }
        public List<string> AllRaritys { get; set; }
        public string RarityOperator { get; set; }
        public List<SearchList> RarityList { get; set; }
        public string TextOperator { get; set; }
        public List<SearchList> TextList { get; set; }
        public string ArtistOperator { get; set; }
        public List<SearchList> ArtistList { get; set; }
        public ColorSelector Colors { get; set; }
        public string ManaOperator { get; set; }
        public string Title { get; set; }
        public bool AdvancedSearch { get; set; }
        public bool Search { get; set; }
        public bool Hovering { get; set; }
        public int SelectedCard { get; set; }
        public List<MyDecks> Decks { get; set; }
        public bool Loading { get; set; }
        public CardAmounts CardAmounts { get; set; }
    }

    public class SearchList
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
        public string Operator { get; set; }
        public string Comparison { get; set; }
    }

    public class MyDecks
    {
        public int Id { get; set; }
        public string DeckName { get; set; }
        public string URL { get; set; }
        public string Colors { get; set; }
    }
}
