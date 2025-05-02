using ClassesManagerReborn.Util;
using FlairsCards.Cards;
using FlairsCards.MonoBehaviours;
using FlairsCards.Utilities;
using FC.Extensions;
using ModsPlus;
using Photon.Pun;
using Photon.Realtime;
using RarityLib.Utils;
using System.Collections;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.Cards
{
    public class Coinflip : CustomEffectCard<CoinflipEffect>
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = GamblerClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Coinflip",
            Description = "50/50 odds of changing your luck and bullet speed at the end of each round",
            ModName = FlairsCards.ModInitials,
            Rarity = RarityUtils.GetRarity("CommonClass"),
            Theme = CardThemeColor.CardThemeColorType.MagicPink,
            Art = FlairsCards.CardArtCoinflip,
            Stats = new[]
            {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Reload time",
                    amount = "-0.5s",
                    simepleAmount = CardInfoStat.SimpleAmount.Some
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Bullet speed",
                    amount = "±25%",
                    simepleAmount = CardInfoStat.SimpleAmount.Some
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Luck",
                    amount = "±1",
                    simepleAmount = CardInfoStat.SimpleAmount.Some
                },
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            gun.reloadTimeAdd = -0.5f;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
    }
}
public class CoinflipEffect : CardEffect
{
    public override IEnumerator OnRoundEnd(IGameModeHandler gameModeHandler)
    {
        int randNum = UnityEngine.Random.Range(0, 2);
        int photonViewId = player.GetComponent<PhotonView>().ViewID;

        player.gameObject.GetOrAddComponent<DoFlipHandler>();
        PhotonView.Get(player).RPC("RPCA_DoFlip", RpcTarget.All, photonViewId, randNum);

        yield break;
    }
}
public class DoFlipHandler : MonoBehaviourPun
{
    [PunRPC]
    public void RPCA_DoFlip(int viewId, int randNum)
    {
        PhotonView view = PhotonView.Find(viewId);
        if (view == null) return;

        Player player = view.GetComponent<Player>();
        if (player == null) return;

        Gun gun = player.data.weaponHandler.gun;

        if (player.data.stats.GetAdditionalData().curseAverse == true)
        {
            randNum = 0;
        }

        if (randNum == 0)
        {
            player.data.stats.GetAdditionalData().luck += 1;
            gun.projectileSpeed += 0.25f;
        }
        else
        {
            player.data.stats.GetAdditionalData().luck -= 1;
            gun.projectileSpeed -= 0.25f;
        }
    }
}