using ModdingUtils.MonoBehaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnboundLib.GameModes;
using UnityEngine;
using static ModdingUtils.Utils.SortingController;

namespace FlairsCards.Monobehaviours
{
    class CantTouchThisMono : MonoBehaviour
    {
        private Player player;
        private Gun gun;
        private GunAmmo gunAmmo;
        private void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            gunAmmo = gun.GetComponentInChildren<GunAmmo>();
            GameModeManager.AddHook(GameModeHooks.HookPickEnd, PickEnd);
        }
        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookPickEnd, PickEnd);
        }
        IEnumerator PickEnd(IGameModeHandler gm)
        {
            gunAmmo.maxAmmo = 1;
            gun.numberOfProjectiles = 1;
            yield break;
        }
    }
}
