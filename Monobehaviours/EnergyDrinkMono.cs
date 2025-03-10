using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;
using static ModdingUtils.Utils.SortingController;

namespace FlairsCards.Monobehaviours
{
    internal class EnergyDrinkMono : MonoBehaviour
    {
        private Player player;
        private Gun gun;
        private GunAmmo gunAmmo;
        private float oldSpeed;
        private float oldDamage;
        private float oldReload;
        private Coroutine effectCoroutine;
        private void Start()
        {
            player = GetComponent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            gunAmmo = gun.GetComponentInChildren<GunAmmo>();
            GameModeManager.AddHook(GameModeHooks.HookPointStart, PointStart);
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, PointEnd);
        }

        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookPointStart, PointStart);
            GameModeManager.RemoveHook(GameModeHooks.HookPointEnd, PointEnd);
        }

        IEnumerator PointStart(IGameModeHandler gm)
        {
            oldSpeed = player.data.stats.movementSpeed;
            oldDamage = gun.damage;
            oldReload = gunAmmo.reloadTimeAdd;

            effectCoroutine = StartCoroutine(RoundStartEffect());

            yield break;
        }

        IEnumerator PointEnd(IGameModeHandler gm)
        {
            if (effectCoroutine != null)
            {
                StopCoroutine(effectCoroutine);
                effectCoroutine = null;
            }

            player.data.stats.movementSpeed = oldSpeed;
            gun.damage = oldDamage;
            gunAmmo.reloadTimeAdd = oldReload;

            yield break;
        }

        private IEnumerator RoundStartEffect()
        {
            player.data.stats.movementSpeed += 0.5f;
            gun.damage += 0.5f;
            gunAmmo.reloadTimeAdd -= 0.5f;

            yield return new WaitForSeconds(12);

            player.data.stats.movementSpeed -= 1f;
            gun.damage -= 1f;
            gunAmmo.reloadTimeAdd += 1f;
        }
    }
}
