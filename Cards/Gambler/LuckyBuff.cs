using ClassesManagerReborn.Util;
using FlairsCards.MonoBehaviours;
using FlairsCards.Utilities;
using FC.Extensions;
using ModsPlus;
using Photon.Pun;
using RarityLib.Utils;
using System;
using System.Collections;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnityEngine;
using static Photon.Pun.Simple.SyncState.Frame;

namespace FlairsCards.Cards
{
    public class LuckyBuff : CustomEffectCard<LuckyBuffEffect>
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = GamblerClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Lucky Buff",
            Description = "Roll a random permanent buff or debuff with its strength depending on your luck each draw",
            ModName = FlairsCards.ModInitials,
            Rarity = RarityUtils.GetRarity("CommonClass"),
            Theme = CardThemeColor.CardThemeColorType.MagicPink,
            Art = FlairsCards.CardArtLuckyBuff,
            Stats = new[]
            {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "???",
                    amount = "b?ff",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "???",
                    amount = "i??re?se",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
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
public class LuckyBuffEffect : CardEffect
{
    public override IEnumerator OnPickPhaseEnd(IGameModeHandler gameModeHandler)
    {
        int num = 0;
        int chance = 0;
        int photonViewId = player.GetComponent<PhotonView>().ViewID;
        int luck = player.data.stats.GetAdditionalData().luck;

        if (player.data.stats.GetAdditionalData().curseAverse == true)
        {
            num = UnityEngine.Random.Range(1, 5);
            chance = UnityEngine.Random.Range(Mathf.Max(0, luck), luck + 2);
        }
        else
        {
            num = UnityEngine.Random.Range(1, 5); 
            chance = UnityEngine.Random.Range(luck - 2, luck + 2); 
        }

        player.gameObject.GetOrAddComponent<RPCBuffHandler>();
        PhotonView.Get(player).RPC("RPCA_ApplyLuckyBuff", RpcTarget.All, photonViewId, num, chance);

        yield break;
    }
}
public class RPCBuffHandler : MonoBehaviourPun
{
    [PunRPC]
    public void RPCA_ApplyLuckyBuff(int viewId, int num, int chance)
    {
        PhotonView view = PhotonView.Find(viewId);
        if (view == null) return;

        Player player = view.GetComponent<Player>();
        if (player == null) return;

        Gun gun = player.data.weaponHandler.gun;

        if (num == 1)
        {
            gun.ammo += chance;
            if (gun.ammo < 0) { gun.ammo = 1; }
        }
        else if (num == 2)
        {
            gun.damage += (float)(0.25 + chance * 0.1);
            if (gun.damage < 0.25f) { gun.damage = 0.25f; }
        }
        else if (num == 3)
        {
            player.data.stats.movementSpeed += (float)(chance * 0.1);
            if (player.data.stats.movementSpeed < 0.25f) { player.data.stats.movementSpeed = 0.25f; }
        }
        else
        {
            player.data.stats.gravity += (float)(chance * 0.1);
            if (player.data.stats.gravity < 0.25f) { player.data.stats.gravity = 0.25f; }
            else if (player.data.stats.gravity > 2f) { player.data.stats.gravity = 1.75f; }
        }
    }
}