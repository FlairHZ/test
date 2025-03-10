using FC.Extensions;
using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;


namespace FlairsCards.MonoBehaviours
{
    class CoinflipMono : MonoBehaviour
    {
        private Player player;
        private CharacterStatModifiers characterStats;
        private CharacterData data; 
        private Gun gun;
        private GunAmmo gunAmmo;
        int luck;
        private void Start()
        {
            player = GetComponentInParent<Player>(); 
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            gunAmmo = gun.GetComponentInChildren<GunAmmo>();
            characterStats = player.GetComponent<CharacterStatModifiers>();
            data = player.GetComponent<CharacterData>();
            GameModeManager.AddHook(GameModeHooks.HookRoundEnd, RoundEnd);
        }

        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookRoundEnd, RoundEnd);
        }

        IEnumerator RoundEnd(IGameModeHandler gm)
        {
            if (player.data.stats.GetAdditionalData().curseAverse == true)
            {
                luck = 0;
            }
            else
            {
                luck = UnityEngine.Random.Range(0, 2);
            }

            if (luck == 0)
            {
                player.data.stats.GetAdditionalData().luck += 1;
                gun.projectileSpeed += 0.25f;
            }
            else
            {
                player.data.stats.GetAdditionalData().luck -= 1;
                gun.projectileSpeed -= 0.25f;
            }

            yield break;
        }
    }
}