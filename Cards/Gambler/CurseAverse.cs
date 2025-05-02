using ClassesManagerReborn.Util;
using FC.Extensions;
using FlairsCards.MonoBehaviours;
using FlairsCards.Utilities;
using ModsPlus;
using RarityLib.Utils;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace FlairsCards.Cards
{
    public class CurseAverse : SimpleCard
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = GamblerClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Curse Averse",
            Description = "Become unable to have negative luck",
            ModName = FlairsCards.ModInitials,
            Rarity = RarityUtils.GetRarity("CommonClass"),
            Theme = CardThemeColor.CardThemeColorType.MagicPink,
            Art = FlairsCards.CardArtCurseAverse,
            Stats = new[]
                {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Health",
                    amount = "+25%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            statModifiers.health = 1.25f;
            statModifiers.GetAdditionalData().curseAverse = true;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
    }
}