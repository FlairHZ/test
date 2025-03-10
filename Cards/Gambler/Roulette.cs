using ClassesManagerReborn.Util;
using FlairsCards.Utilities;
using ModdingUtils.RoundsEffects;
using ModsPlus;
using Photon.Pun;
using RarityLib.Utils;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Extensions;
using UnityEngine;
using static Photon.Pun.Simple.SyncState.Frame;

// wip, add this in v0.3.0
// (its very broken ignore this)
// make this a unity project first
namespace FlairsCards.Cards
{
    class Roulette : CustomEffectCard<RandomBullet>
    {
        internal static CardInfo Card = null;

        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = GamblerClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Roulette",
            Description = "",
            ModName = FlairsCards.ModInitials,
            Rarity = RarityUtils.GetRarity("CommonClass"),
            Theme = CardThemeColor.CardThemeColorType.MagicPink,
            Art = FlairsCards.CardArtRoulette,
            Stats = new[]
                {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Health",
                    amount = "+20%",
                    simepleAmount = CardInfoStat.SimpleAmount.Some
                },
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            statModifiers.health = 1.20f;
            FCDebug.Log($"[{FlairsCards.ModInitials}][Card] {GetTitle()} has been setup.");
        }
    }
    
    // Credit to Willis for some of this
    class RandomBullet : CardEffect
    {
        private float stunDuration = 1f;

        public override void OnShoot(GameObject projectile)
        {
            //gun.projectileColor = Color.blue;

            projectile
                .AddComponent<StunBulletEffect>()
                .Initialize(stunDuration);
        }
        public override void OnBulletHit(GameObject projectile, HitInfo hit)
        {
            var stun = projectile.GetComponent<StunBulletEffect>();
            if (stun == null) return;

            var target = hit.collider.gameObject.GetComponentInChildren<Player>();
            if (target == null) return;

            stun.StunPlayer(target);
        }

        public class StunBulletEffect : MonoBehaviour
        {
            private float duration = 1f;
            private Player player;
            private CharacterData data;
            private StunHandler stunHandler;

            public void Start()
            {
                player = gameObject.GetComponentInParent<Player>();
                data = player.GetComponent<CharacterData>();
                stunHandler = player.GetComponent<StunHandler>();
            }
            public void Initialize(float duration)
            {
                this.duration = duration;
            }
            public void StunPlayer(Player targetPlayer)
            {
                PhotonView photonView = targetPlayer.GetComponent<PhotonView>();
                if (photonView != null && photonView.IsMine)
                {
                    player.data.maxHealth = 150f;
                    photonView.RPC("RPCA_StunPlayer", RpcTarget.All, targetPlayer.playerID, duration);
                }
            }

            [PunRPC]
            public void RPCA_StunPlayer(int playerID, float duration)
            {
                var targetPlayer = PlayerManager.instance.GetPlayerWithID(playerID);
                if (targetPlayer != null)
                {
                    targetPlayer.data.stunHandler.AddStun(duration);
                }
            }
        }
    }
}