using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.Monobehaviours
{
    internal class PersonalBodyguardMono : MonoBehaviour
    {
        private Player player;
        private bool firstTimeHit = false;
        private void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            player.data.stats.WasDealtDamageAction += OnDamage;
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, PointEnd);
        }
        private void OnDestroy()
        {
            player.data.stats.WasDealtDamageAction -= OnDamage;
            GameModeManager.RemoveHook(GameModeHooks.HookPointEnd, PointEnd);
        }
        
        IEnumerator PointEnd(IGameModeHandler gm)
        {
            firstTimeHit = false;
            yield break;
        }

        private void OnDamage(Vector2 damage, bool selfDamage)
        {
            if (!firstTimeHit)
            {
                player.data.health += damage.magnitude;
                firstTimeHit = true;
            }
        }
    }
}
