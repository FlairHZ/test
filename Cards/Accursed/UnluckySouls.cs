using ClassesManagerReborn.Util;
using FC.Extensions;
using FlairsCards.MonoBehaviours;
using FlairsCards.Utilities;
using Photon.Pun;
using RarityLib.Utils;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using WillsWackyManagers.Utils;

namespace FlairsCards.Cards
{
    class UnluckySouls : CustomCard
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = AccursedClass.name;
        }
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // Unsure if I need a RPC for this, but rather safe than sorry
            CurseRPCHandler handler = player.gameObject.GetOrAddComponent<CurseRPCHandler>();
            PhotonView view = player.GetComponent<PhotonView>();
            view.RPC("RPCA_DrawRandomCurses", RpcTarget.All);

            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been removed to player {player.playerID}.");
        }

        protected override string GetTitle()
        {
            return "Unlucky Souls";
        }
        protected override string GetDescription()
        {
            return "Two random players get a curse, including you";
        }
        protected override GameObject GetCardArt()
        {
            return FlairsCards.CardArtUnluckySouls;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return RarityUtils.GetRarity("RareClass");
        }
        protected override CardInfoStat[] GetStats()
        {
            return null;
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
        public override string GetModName()
        {
            return FlairsCards.ModInitials;
        }
    }
    public class CurseRPCHandler : MonoBehaviourPun
    {
        [PunRPC]
        public void RPCA_DrawRandomCurses()
        {
            for (int i = 0; i <= 1; i++)
            {
                var randomPlayer = UnityEngine.Random.Range(0, PlayerManager.instance.players.Count);
                var chosenPlayer = PlayerManager.instance.players[randomPlayer];
                chosenPlayer.data.stats.GetAdditionalData().curses += 1;
                CurseManager.instance.CursePlayer(chosenPlayer, (curse) => { ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(chosenPlayer, curse); });
            }
        }
    }
}

