using ClassesManagerReborn.Util;
using FC.Extensions;
using FlairsCards.MonoBehaviours;
using FlairsCards.Utilities;
using ModsPlus;
using RarityLib.Utils;
using System.Collections;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnityEngine;
using WillsWackyManagers.Utils;

namespace FlairsCards.Cards
{
    class UnholyCurse : CustomEffectCard<DrawOneCurse>
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = AccursedClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Unholy Curse",
            Description = "Luck has never been on your side",
            ModName = FlairsCards.ModInitials,
            Rarity = RarityUtils.GetRarity("UncommonClass"),
            Theme = CardThemeColor.CardThemeColorType.DestructiveRed,
            Art = FlairsCards.CardArtUnholyCurse,
            Stats = new[]
        {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Damage",
                    amount = "+15%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Health",
                    amount = "+45%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Forced curses per draw",
                    amount = "+1",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                }
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            gun.damage = 1.15f;
            statModifiers.health = 1.45f;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
    }
    public class DrawOneCurse : CardEffect
    {
        public override IEnumerator OnPickPhaseEnd(IGameModeHandler gameModeHandler)
        {
            CurseManager.instance.CursePlayer(player, (curse) => { ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, curse); });
            player.data.stats.GetAdditionalData().curses += 1;
            yield break;
        }
    }
}