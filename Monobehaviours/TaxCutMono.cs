using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.MonoBehaviours
{
    class TaxCutMono : MonoBehaviour
    {
        private Player player;
        private Gun gun;
        private GunAmmo gunAmmo;
        private CharacterData data;
        private HealthHandler health;
        private Gravity gravity;
        private Block block;
        private CharacterStatModifiers characterStats;
        private int playerTeamID;
        private CardInfo previousCard;  // Field to track the previous card

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
            playerTeamID = player.teamID;
            GameModeManager.AddHook(GameModeHooks.HookPickEnd, PickEnd);
            GameModeManager.AddHook(GameModeHooks.HookRoundEnd, RoundEnd);
        }

        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookRoundEnd, RoundEnd);
            GameModeManager.RemoveHook(GameModeHooks.HookPickEnd, PickEnd);
        }

        IEnumerator RoundEnd(IGameModeHandler gm)
        {
            ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(player, previousCard, ModdingUtils.Utils.Cards.SelectionType.Newest);
            previousCard = null;
            yield break;
        }

        IEnumerator PickEnd(IGameModeHandler gm)
        {
            int[] roundWinners = gm.GetPointWinners();
            bool isWinner = roundWinners.Contains(playerTeamID);
            if (isWinner)
            {
                CardInfo randomDraw = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, Condition);
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomDraw, addToCardBar: true);
                ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, randomDraw, 0f);
                previousCard = randomDraw;
            }
            yield break;
        }

        private bool Condition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            return card.rarity == CardInfo.Rarity.Common || card.rarity == CardInfo.Rarity.Uncommon || card.rarity == CardInfo.Rarity.Rare;
        }
    }
}