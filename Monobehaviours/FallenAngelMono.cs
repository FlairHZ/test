using FC.Extensions;
using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.MonoBehaviours
{
    class FallenAngelMono : MonoBehaviour
    {
        private Player player;
        private void Start()
        {
            player = GetComponentInParent<Player>();
            GameModeManager.AddHook(GameModeHooks.HookRoundStart, RoundStart);
        }

        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookRoundStart, RoundStart);
        }

        IEnumerator RoundStart(IGameModeHandler gm)
        {
            player.data.stats.respawns = (int)(player.data.stats.GetAdditionalData().curses / 4);

            yield break;
        }
    }
}