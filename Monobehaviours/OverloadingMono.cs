using FC.Extensions;
using System;
using UnboundLib.GameModes;
using UnityEngine;
using UnityEngine.Rendering;

namespace FlairsCards.MonoBehaviours
{
    class OverloadingMono : MonoBehaviour
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
                if (!player.data.stats.GetAdditionalData().overCharged) {
                    block.cdMultiplier = (float)((0.625 * Mathf.Max(player.data.HealthPercentage, 0.2f)) + 0.375);
                }
                timer = timer - waitTime;
            }
        }
    }
}