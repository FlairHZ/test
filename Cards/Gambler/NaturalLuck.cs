using ClassesManagerReborn.Util;
using FC.Extensions;
using FlairsCards.Utilities;
using ModdingUtils.AIMinion.Extensions;
using ModsPlus;
using Photon.Pun;
using RarityLib.Utils;
using System.Collections;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.Cards
{
    public class NaturalLuck : CustomEffectCard<NaturalLuckEffect>
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = GamblerClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Natural Luck",
            Description = "Gain or lose a random amount of luck each round, gain 1 luck now",
            ModName = FlairsCards.ModInitials,
            Rarity = RarityUtils.GetRarity("CommonClass"),
            Theme = CardThemeColor.CardThemeColorType.MagicPink,
            Art = FlairsCards.CardArtNaturalLuck,
            Stats = new[]
            {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Damage",
                    amount = "+25%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Luck",
                    amount = "+1",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            gun.damage = 1.25f;
            statModifiers.GetAdditionalData().luck += 1;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
    }
}
public class NaturalLuckEffect : CardEffect
{
    public override IEnumerator OnRoundEnd(IGameModeHandler gameModeHandler)
    {
        int randNum = 0;

        if (player.data.stats.GetAdditionalData().curseAverse)
        {
            randNum = UnityEngine.Random.Range(0, 3);
        }
        else
        {
            randNum = UnityEngine.Random.Range(-2, 3);
        }
        int photonViewId = player.GetComponent<PhotonView>().ViewID;

        player.gameObject.GetOrAddComponent<NaturalLuckHandler>();
        PhotonView.Get(player).RPC("RPCA_ApplyNaturalLuck", RpcTarget.All, photonViewId, randNum);

        yield break;
    }
}
public class NaturalLuckHandler : MonoBehaviourPun
{
    [PunRPC]
    public void RPCA_ApplyNaturalLuck(int viewId, int randNum)
    {
        PhotonView view = PhotonView.Find(viewId);
        if (view == null) return;

        Player player = view.GetComponent<Player>();
        if (player == null) return;

        player.data.stats.GetAdditionalData().luck += randNum;
    }
}