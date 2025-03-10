using FC.Extensions;
using System.Collections;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.MonoBehaviours
{
    class LuckyBuffMono : MonoBehaviour
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
        int num;
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
            playerTeamID = player.teamID;
            GameModeManager.AddHook(GameModeHooks.HookPickEnd, PickEnd);
        }

        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookPickEnd, PickEnd);
        }

        IEnumerator PickEnd(IGameModeHandler gm)
        {
            if (player.data.stats.GetAdditionalData().curseAverse == true)
            {
                System.Random rndPos = new System.Random();
                num = rndPos.Next(1, 5);
                chance = UnityEngine.Random.Range(player.data.stats.GetAdditionalData().luck, player.data.stats.GetAdditionalData().luck);
                if (chance < 0)
                {
                    chance = 0;
                }
            }
            else
            {
                System.Random rndNeu = new System.Random();
                num = rndNeu.Next(1, 5);
                chance = UnityEngine.Random.Range(-2 + player.data.stats.GetAdditionalData().luck, player.data.stats.GetAdditionalData().luck + 1);
            }

            if (num == 1)
            {
                gunAmmo.maxAmmo += chance;
            }
            else if (num == 2)
            {
                gun.damage += (float)(0.25 + chance * 0.1);
            }
            else if (num == 3)
            {
                characterStats.movementSpeed += (float)(chance * 0.1);
            }
            else
            {
                characterStats.gravity += (float)(chance * 0.1);
            }

            yield break;
        }
    }
}