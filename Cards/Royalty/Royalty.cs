using ClassesManagerReborn.Util;
using FlairsCards.MonoBehaviours;
using FlairsCards.Utilities;
using ModsPlus;
using RarityLib.Utils;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.Cards
{
    class Royalty : CustomCard
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>();
        }
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            statModifiers.health = 1.3f;
            cardInfo.allowMultiple = false;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<RoyaltyMono>();
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Destroy(player.gameObject.GetOrAddComponent<RoyaltyMono>());
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been removed to player {player.playerID}.");
        }
        protected override string GetTitle()
        {
            return "Royalty";
        }
        protected override string GetDescription()
        {
            return "Gain increasing damage at the end of a round upon winning";
        }
        protected override GameObject GetCardArt()
        {
            return FlairsCards.CardArtRoyalty;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return RarityUtils.GetRarity("CommonClass");
        }
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Health",
                    amount = "+30%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Damage per round",
                    amount = "+15%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.FirepowerYellow;
        }
        public override string GetModName()
        {
            return FlairsCards.ModInitials;
        }
    }
}

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