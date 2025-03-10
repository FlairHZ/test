using ModdingUtils.MonoBehaviours;
using ModsPlus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnboundLib.GameModes;
using UnityEngine;
using UnityEngine.UI;
using static ModdingUtils.Utils.SortingController;

namespace FlairsCards.Monobehaviours
{
    class AdrenalineMono : MonoBehaviour
    {
        private Player player;
        private Coroutine effectCoroutine;
        private bool isActive = false;
        private float oldSpeed = 1.35f;
        private CustomHealthBar shieldBar;
        private HealthHandler healthHandler;
        private void Start()
        {
            player = GetComponent<Player>();
            player.data.stats.WasDealtDamageAction += OnDamage;
            healthHandler = player.GetComponent<HealthHandler>();
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, PointEnd);
            GameModeManager.AddHook(GameModeHooks.HookPickEnd, PickEnd);

            var parent = player.GetComponentInChildren<PlayerWobblePosition>().transform;
            var obj = new GameObject("Shield Bar");
            obj.transform.SetParent(parent);
            shieldBar = obj.AddComponent<CustomHealthBar>();
            shieldBar.transform.localPosition = Vector3.up * 0.25f;
            shieldBar.transform.localScale = Vector3.one;
            shieldBar.SetColor(Color.cyan);
            shieldBar.CurrentHealth = 0f;
        }
        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookPointEnd, PointEnd);
            GameModeManager.RemoveHook(GameModeHooks.HookPickEnd, PickEnd);
            player.data.stats.WasDealtDamageAction -= OnDamage;
            Destroy(shieldBar.gameObject);
        }
        IEnumerator PointEnd(IGameModeHandler gm)
        {
            if (isActive)
            {
                isActive = false;
                if (player.data.stats.movementSpeed - 0.4f >= (oldSpeed - 0.1f))
                {
                    player.data.stats.movementSpeed -= 0.4f;
                }
            }
            if (effectCoroutine != null)
            {
                StopCoroutine(effectCoroutine);
                effectCoroutine = null;
            }
            shieldBar.CurrentHealth = 0f;
            yield break;
        }

        IEnumerator PickEnd(IGameModeHandler gm)
        {
            oldSpeed = player.data.stats.movementSpeed;
            yield break;
        }
        private void OnDamage(Vector2 damage, bool selfDamage)
        {
            if (shieldBar.CurrentHealth > 0f)
            {
                // Absorb as much damage as possible by the shield
                float remainingDamage = damage.magnitude;

                if (shieldBar.CurrentHealth >= remainingDamage)
                {
                    shieldBar.CurrentHealth -= remainingDamage;
                    player.data.health += damage.magnitude;
                }
                else
                {
                    // Shield is depleted, subtract the remainder from health
                    remainingDamage -= shieldBar.CurrentHealth;
                    if (remainingDamage < 0f)
                    {
                        remainingDamage = 0f;
                    }
                    shieldBar.CurrentHealth = 0f;
                    Vector2 damageToHealth = Vector2.up * remainingDamage;
                    player.data.health += damage.magnitude;
                    healthHandler.TakeDamage(damageToHealth, transform.position, null, null, true, true);
                }
            }

            if (!isActive)
            {
                effectCoroutine = StartCoroutine(RoundStartEffect());
            }
        }
        private IEnumerator RoundStartEffect()
        {
            isActive = true;
            player.data.stats.movementSpeed += 0.4f;
            if (player.data.health > 0f)
            {
                shieldBar.CurrentHealth += Mathf.Round(player.data.maxHealth * 0.15f);
            }

            try
            {
                yield return new WaitForSeconds(2);
            }
            finally
            {
                player.data.stats.movementSpeed -= 0.4f;
                shieldBar.CurrentHealth -= Mathf.Round(player.data.maxHealth * 0.15f);
                if (shieldBar.CurrentHealth < 0f)
                {
                    shieldBar.CurrentHealth = 0f;
                }
                isActive = false;
                effectCoroutine = null;
            }
        }
    }
}
