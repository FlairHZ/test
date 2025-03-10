using System;
using System.Collections;
using System.Linq;
using ModsPlus;
using Photon.Pun;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.MonoBehaviours
{
    class SelfSacrificeMono : MonoBehaviour
    {
        private Player player;
        private Block block;
        private GeneralInput input;
        private HealthHandler healthHandler;
        private void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>(); 
            input = player.GetComponent<GeneralInput>();
            healthHandler = player.GetComponent<HealthHandler>();
        }

        void Update()
        {
            if (!block.IsOnCD() && input.shieldWasPressed)
            {
                block.RPCA_DoBlock(true);
            }
            else if (block.IsOnCD() && player.data.HealthPercentage > 0.2 && input.shieldWasPressed)
            {
                Vector2 damage = Vector2.up * (player.data.maxHealth / 5);
                healthHandler.TakeDamage(damage, transform.position, null, null, false);
                block.RPCA_DoBlock(true);
            }
        }
    }
}
