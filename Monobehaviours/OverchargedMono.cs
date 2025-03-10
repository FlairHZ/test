using FC.Extensions;
using System;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.MonoBehaviours
{
    class OverchargedMono : MonoBehaviour
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
            block.cdMultiplier = (float)((0.9375 * player.data.HealthPercentage) + 0.0625);
            player.data.stats.GetAdditionalData().overCharged = true; // Blanket solution, make it actually better later
        }
    }
}