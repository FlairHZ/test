using FC.Extensions;
using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.MonoBehaviours
{
    class NaturalLuckMono : MonoBehaviour
    {
        private Player player;
        int tempLuck = 0;
        int luck;
        private void Start()
        {
            player = GetComponentInParent<Player>();
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
                player.data.stats.GetAdditionalData().luck -= tempLuck;
                luck = UnityEngine.Random.Range(0, 2);
                player.data.stats.GetAdditionalData().luck += luck;
            }
            else
            {
                player.data.stats.GetAdditionalData().luck -= tempLuck;
                luck = UnityEngine.Random.Range(-2, 3);
                player.data.stats.GetAdditionalData().luck += luck;
            }

            tempLuck = luck;

            yield break;
        }
    }
}