using ClassesManagerReborn.Util;
using FlairsCards.Monobehaviours;
using FlairsCards.Utilities;
using RarityLib.Utils;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace FlairsCards.Cards
{
    class PersonalBodyguard : CustomCard
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = RoyaltyClass.name;
        }
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            statModifiers.health = 1.4f;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<PersonalBodyguardMono>();
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Destroy(player.gameObject.GetOrAddComponent<PersonalBodyguardMono>());
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been removed to player {player.playerID}.");
        }

        protected override string GetTitle()
        {
            return "Personal Bodyguard";
        }
        protected override string GetDescription()
        {
            return "The first instance of damage you take deals no damage (on hit effects still occur)";
        }
        protected override GameObject GetCardArt()
        {
            return FlairsCards.CardArtPersonalBodyguard;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return RarityUtils.GetRarity("UncommonClass");
        }
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Health",
                    amount = "+40%",
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