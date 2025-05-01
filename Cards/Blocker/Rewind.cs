using ClassesManagerReborn.Util;
using FlairsCards.MonoBehaviours;
using FlairsCards.Utilities;
using ModsPlus;
using RarityLib.Utils;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace FlairsCards.Cards
{
    public class Rewind : SimpleCard
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = BlockerClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Rewind",
            Description = "Leap backwards after a block",
            ModName = FlairsCards.ModInitials,
            Rarity = RarityUtils.GetRarity("CommonClass"),
            Theme = CardThemeColor.CardThemeColorType.ColdBlue,
            Art = FlairsCards.CardArtRewind,
            Stats = new[]
                {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Health",
                    amount = "+20%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Block cooldown",
                    amount = "+0.25s",
                    simepleAmount = CardInfoStat.SimpleAmount.slightlyLower
                },
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            statModifiers.health = 1.2f;
            block.cdAdd = 0.25f;
            block.forceToAdd = -15f;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
    }
}