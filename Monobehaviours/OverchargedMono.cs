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
        private float timer = 0.0f;
        private float waitTime = 0.05f;
        private void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();
        }
        void Update()
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                block.cdMultiplier = (float)((0.9375 * player.data.HealthPercentage) + 0.0625);
                player.data.stats.GetAdditionalData().overCharged = true; // Blanket solution, make it actually better later
            }
        }
    }
}