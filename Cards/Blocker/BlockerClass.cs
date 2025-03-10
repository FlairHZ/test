using ClassesManagerReborn;
using System.Collections;

namespace FlairsCards.Cards
{
    class BlockerClass : ClassHandler
    {
        internal static string name = "Blocker";

        public override IEnumerator Init()
        {
            while (!(Gambler.Card)) yield return null;
            ClassesRegistry.Register(Blocker.Card, CardType.Entry); 
            ClassesRegistry.Register(Rewind.Card, CardType.Card, Blocker.Card);
            ClassesRegistry.Register(ControlFreak.Card, CardType.Card, Blocker.Card);
            ClassesRegistry.Register(Overloading.Card, CardType.Gate, Blocker.Card);
            ClassesRegistry.Register(SelfSacrifice.Card, CardType.Gate, Blocker.Card);
            ClassesRegistry.Register(Overcharged.Card, CardType.SubClass, new CardInfo[] { Overloading.Card });
            ClassesRegistry.Register(TerminalVelocity.Card, CardType.SubClass, new CardInfo[] { SelfSacrifice.Card });
        }
        public override IEnumerator PostInit()
        {
            ClassesRegistry.Get(ControlFreak.Card).Blacklist(Overloading.Card);
            ClassesRegistry.Get(Overloading.Card).Blacklist(ControlFreak.Card);
            yield break;
        }
    }
}