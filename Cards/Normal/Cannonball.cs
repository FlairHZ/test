using FlairsCards.Utilities;
using ModsPlus;

// I would remove this card so the card pack is only classes
// But my friends really really love cannonball so it can stay

namespace FlairsCards.Cards
{
    public class Cannonball : SimpleCard
    {
        internal static CardInfo Card = null;
        public override CardDetails Details => new CardDetails
        {
            Title = "Cannonball",
            Description = "",
            ModName = FlairsCards.ModInitials,
            Rarity = CardInfo.Rarity.Common,
            Theme = CardThemeColor.CardThemeColorType.DefensiveBlue,
            Art = FlairsCards.CardArtCannonball,
            Stats = new[]
                {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Projectile size",
                    amount = "+45%",
                    simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Damage",
                    amount = "+25%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Bullet speed",
                    amount = "-40%",
                    simepleAmount = CardInfoStat.SimpleAmount.smaller
                },
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            gun.projectileSize = 1.45f;
            gun.damage = 1.25f;
            gun.projectileSpeed = 0.6f;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
    }
}