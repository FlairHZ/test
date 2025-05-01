using ClassesManagerReborn.Util;
using FC.Extensions;
using FlairsCards.Utilities;
using ModsPlus;
using RarityLib.Utils;
using UnboundLib;

namespace FlairsCards.Cards
{
    public class Gambler : SimpleCard
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>();
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Gambler",
            Description = "Are you feeling lucky today? Briefly shows approximate luck at start of each round.",
            ModName = FlairsCards.ModInitials,
            Rarity = RarityUtils.GetRarity("CommonClass"),
            Theme = CardThemeColor.CardThemeColorType.MagicPink,
            Art = FlairsCards.CardArtGambler,
            Stats = new[]
                {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Damage",
                    amount = "+40%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Bullet speed",
                    amount = "+25%",
                    simepleAmount = CardInfoStat.SimpleAmount.Some
                },
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            gun.damage = 1.4f;
            gun.projectileSpeed = 1.25f;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
    }
}