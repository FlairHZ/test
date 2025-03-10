using FC.Extensions;
using System.Collections;
using System.Data.SqlTypes;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;
using WillsWackyManagers.Utils;

namespace FlairsCards.MonoBehaviours
{
    class WildcardMono : MonoBehaviour
    {
        private Player player;
        private Gun gun;
        private GunAmmo gunAmmo;
        private CharacterData data;
        private HealthHandler health;
        private Gravity gravity;
        private Block block;
        private CharacterStatModifiers characterStats;
        private CardInfo? previousCard = null;  // Field to track the previous card
        int chance;

        private void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            gunAmmo = gun.GetComponentInChildren<GunAmmo>();
            data = player.GetComponent<CharacterData>();
            health = player.GetComponent<HealthHandler>();
            gravity = player.GetComponent<Gravity>();
            block = player.GetComponent<Block>();
            characterStats = player.GetComponent<CharacterStatModifiers>();
            GameModeManager.AddHook(GameModeHooks.HookPickEnd, PickEnd);
            GameModeManager.AddHook(GameModeHooks.HookPickStart, PickStart);
        }

        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookPickEnd, PickEnd);
            GameModeManager.RemoveHook(GameModeHooks.HookPickStart, PickStart);
        }

        IEnumerator PickStart(IGameModeHandler gm)
        {
            if (previousCard != null)
            {
                ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(player, player.data.currentCards.Count - 1, true);
            }

            yield break;
        }

        IEnumerator PickEnd(IGameModeHandler gm)
        {
            // Draw a new card for this turn
            if (player.data.stats.GetAdditionalData().curseAverse == true)
            {
                chance = UnityEngine.Random.Range(player.data.stats.GetAdditionalData().luck, player.data.stats.GetAdditionalData().luck + 2);
            }
            else
            {
                chance = UnityEngine.Random.Range(-1 + player.data.stats.GetAdditionalData().luck, player.data.stats.GetAdditionalData().luck + 2);
            }
            CardInfo newCard = null;

            if (chance <= 0)
            {
                CardInfo[] cardsToDrawFrom = ModdingUtils.Utils.Cards.instance.HiddenCards.ToArray();
                newCard = ModdingUtils.Utils.Cards.instance.DrawRandomCardWithCondition(cardsToDrawFrom, player, gun, gunAmmo, data, health, gravity, block, characterStats, NeutralNameCondition);
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, newCard, addToCardBar: true);
                ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, newCard, 0f);
            }
            else if (chance == 1)
            {
                newCard = ModdingUtils.Utils.Cards.instance.GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, CommonCondition);
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, newCard, addToCardBar: true);
                ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, newCard, 0f);
            }
            else if (chance >= 2 && chance <= 3)
            {
                newCard = ModdingUtils.Utils.Cards.instance.GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, UncommonCondition);
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, newCard, addToCardBar: true);
                ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, newCard, 0f);
            }
            else if (chance >= 4)
            {
                newCard = ModdingUtils.Utils.Cards.instance.GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, RareCondition);
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, newCard, addToCardBar: true);
                ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, newCard, 0f);
            }

            previousCard = newCard;  // Update the previous card to the newly drawn card

            yield break;
        }
        private bool NeutralNameCondition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            return true;
        }
        private bool CommonCondition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            return card.rarity == CardInfo.Rarity.Common;
        }
        private bool UncommonCondition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            return card.rarity == CardInfo.Rarity.Uncommon;
        }
        private bool RareCondition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            return card.rarity == CardInfo.Rarity.Rare;
        }
    }
}