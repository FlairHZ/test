using ClassesManagerReborn;
using System.Collections;

namespace FlairsCards.Cards
{
    class AccursedClass : ClassHandler
    {
        internal static string name = "Accursed";

        public override IEnumerator Init()
        {
            while (!(Accursed.Card && UnluckySouls.Card && CursedDraw.Card && UnholyCurse.Card)) yield return null;
            ClassesRegistry.Register(Accursed.Card, CardType.Entry);
            ClassesRegistry.Register(UnluckySouls.Card, CardType.Card, Accursed.Card);
            ClassesRegistry.Register(Prayer.Card, CardType.Card, Accursed.Card);
            ClassesRegistry.Register(UnholyCurse.Card, CardType.Gate, Accursed.Card);
            ClassesRegistry.Register(CursedDraw.Card, CardType.Gate, Accursed.Card);
            ClassesRegistry.Register(Condemnation.Card, CardType.SubClass, new CardInfo[] { UnholyCurse.Card });
            ClassesRegistry.Register(FallenAngel.Card, CardType.SubClass, new CardInfo[] { UnholyCurse.Card });
        }
        public override IEnumerator PostInit()
        {
            yield break;
        }
    }
}