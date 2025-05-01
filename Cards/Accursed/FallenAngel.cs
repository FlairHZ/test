using ClassesManagerReborn.Util;
using FC.Extensions;
using FlairsCards.MonoBehaviours;
using FlairsCards.Utilities;
using ModsPlus;
using RarityLib.Utils;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnityEngine;
using System.Collections;
using WillsWackyManagers.Utils;
using System.Dynamic;

namespace FlairsCards.Cards
{
    class FallenAngel : CustomEffectCard<CursedRevive>
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = AccursedClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Fallen Angel",
            Description = "Gain a revive for every 4 curses you have",
            ModName = FlairsCards.ModInitials,
            Rarity = RarityUtils.GetRarity("CommonClass"),
            Theme = CardThemeColor.CardThemeColorType.DestructiveRed,
            Art = FlairsCards.CardArtFallenAngel,
            Stats = new[]
                {
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Health",
                    amount = "-15%",
                    simepleAmount = CardInfoStat.SimpleAmount.lower
                },
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            statModifiers.health = 0.85f;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
    }

    public class CursedRevive : CardEffect
    {
        public override IEnumerator OnPlayerPickEnd(IGameModeHandler gameModeHandler)
        {
            player.data.stats.respawns = (int)(player.data.stats.GetAdditionalData().curses / 4);

            yield break;
        }
    }
}