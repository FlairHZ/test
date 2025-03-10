using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;


namespace FlairsCards.MonoBehaviours
{
    class ControlFreakMono : MonoBehaviour
    {
        private Player player;
        private Block block;

        private void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();
            GameModeManager.AddHook(GameModeHooks.HookRoundStart, RoundStart);
        }

        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookRoundStart, RoundStart);
        }
        IEnumerator RoundStart(IGameModeHandler gm)
        {
            block.counter = 1000;
            block.cdMultiplier = 1f;
            block.cdAdd = 0f;
            block.cooldown = 1f;

            if (block.cdAdd > 0f)
            {
                block.cdAdd = 0f;
            }

            yield break;
        }
        void Update()
        {
            if (!block.IsOnCD())
            {
                block.RPCA_DoBlock(true);
            }
        }
    }
}
