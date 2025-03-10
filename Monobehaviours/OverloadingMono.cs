using FC.Extensions;
using System;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.MonoBehaviours
{
    class OverloadingMono : MonoBehaviour
    {
        private Player player;
        private Block block;
        private void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();
        }
        void Update()
        {
            if (player.data.stats.GetAdditionalData().overCharged)
            {
                block.cdMultiplier = 1f;
            }
            else
            {
                block.cdMultiplier = (float) ((0.625 * player.data.HealthPercentage) + 0.375);
            }
        }
    }
}