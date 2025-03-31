using ClassesManagerReborn.Util;
using FC.Extensions;
using FlairsCards.Monobehaviours;
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
    class CursedDraw : CustomEffectCard<DrawCurse>
    {
        internal static CardInfo Card = null;

        public override CardDetails Details => new CardDetails
        {
            Title = "Cursed Draw",
            Description = "Draw two common non-class cards, also draw a <color=#ff000fff>curse</color>",
            ModName = FlairsCards.ModInitials,
            Rarity = RarityUtils.GetRarity("UncommonClass"),
            Theme = CardThemeColor.CardThemeColorType.DestructiveRed,
            Art = FlairsCards.CardArtCursedDraw,
        };
    }

    public class DrawCurse : CardEffect
    {
        bool isDrawn = false;
        public override IEnumerator OnPickPhaseEnd(IGameModeHandler gameModeHandler)
        {
            if (!isDrawn)
            {
                var common = ModdingUtils.Utils.Cards.instance.GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, CommonCondition);
                var common2 = ModdingUtils.Utils.Cards.instance.GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, CommonCondition);
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, common, false, "", 2f, 2f, true);
                ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, common, 3f);
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, common2, false, "", 2f, 2f, true);
                ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, common2, 3f);
                CurseManager.instance.CursePlayer(player, (curse) => {
                    ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, curse, 3f);
                });
                isDrawn = true;
            }
            yield break;
        }
        private bool CommonCondition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            return card.rarity == CardInfo.Rarity.Common && card.cardName != "Cursed Draw";
        }
    }
}

