using ClassesManagerReborn.Util;
using FlairsCards.Utilities;
using ModsPlus;
using Photon.Pun;
using Photon.Realtime;
using RarityLib.Utils;
using System.Collections;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.Cards
{
    public class Arrogance : CustomEffectCard<ArroganceEffect>
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = RoyaltyClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Arrogance",
            Description = "Losing hurts",
            ModName = FlairsCards.ModInitials,
            Rarity = RarityUtils.GetRarity("CommonClass"),
            Theme = CardThemeColor.CardThemeColorType.FirepowerYellow,
            Art = FlairsCards.CardArtArrogance,
            Stats = new[]
            {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Damage/Speed per round won",
                    amount = "+15%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Damage/Speed per round lost",
                    amount = "-20%",
                    simepleAmount = CardInfoStat.SimpleAmount.slightlyLower
                },
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
    }
}
public class ArroganceEffect : CardEffect
{
    public override IEnumerator OnRoundEnd(IGameModeHandler gm)
    {
        int[] roundWinners = gm.GetRoundWinners();
        bool isWinner = roundWinners.Contains(player.teamID);

        if (isWinner)
        {
            gun.damage += 0.15f;
            player.data.stats.movementSpeed += 0.15f;
        }
        else
        {
            gun.damage -= 0.20f;
            if (gun.damage < 0.25f) { gun.damage = 0.25f; }
            player.data.stats.movementSpeed -= 0.20f;
            if (player.data.stats.movementSpeed == 0.25f) { player.data.stats.movementSpeed = 0.25f; }
        }

        yield break;
    }
}