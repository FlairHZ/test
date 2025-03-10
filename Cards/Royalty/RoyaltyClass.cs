using ClassesManagerReborn;
using System.Collections;

namespace FlairsCards.Cards
{
    class RoyaltyClass : ClassHandler
    {
        internal static string name = "Royalty";

        public override IEnumerator Init()
        {
            while (!(Royalty.Card)) yield return null;
            ClassesRegistry.Register(Royalty.Card, CardType.Entry);
            ClassesRegistry.Register(Arrogance.Card, CardType.Card, Royalty.Card);
            ClassesRegistry.Register(Revenge.Card, CardType.Card, Royalty.Card);
            ClassesRegistry.Register(PersonalBodyguard.Card, CardType.Gate, Royalty.Card);
            ClassesRegistry.Register(TaxCut.Card, CardType.SubClass, new CardInfo[] { PersonalBodyguard.Card });
        }
        public override IEnumerator PostInit()
        {
            yield break;
        }
    }
}