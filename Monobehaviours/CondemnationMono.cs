using System.Collections;
using System.Linq;
using FC.Extensions;
using ModsPlus;
using UnboundLib.GameModes;
using UnityEngine;
using WillsWackyManagers.Utils;

namespace FlairsCards.MonoBehaviours
{
    class CondemnationMono : MonoBehaviour
    {
        private Player player;
        private CustomHealthBar shieldBar;
        private HealthHandler healthHandler;
        private void Start()
        {
            player = GetComponentInParent<Player>();
            player.data.stats.WasDealtDamageAction += OnDamage;
            healthHandler = player.GetComponent<HealthHandler>();
            GameModeManager.AddHook(GameModeHooks.HookPointStart, PointStart);

            var parent = player.GetComponentInChildren<PlayerWobblePosition>().transform;
            var obj = new GameObject("Shield Bar");
            obj.transform.SetParent(parent);
            shieldBar = obj.AddComponent<CustomHealthBar>();
            shieldBar.transform.localPosition = Vector3.up * 0.25f;
            shieldBar.transform.localScale = Vector3.one;
            shieldBar.SetColor(Color.cyan);
            shieldBar.CurrentHealth = 0f;
            shieldBar.MaxHealth = 300f;
        }

        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookPointStart, PointStart);
            player.data.stats.WasDealtDamageAction -= OnDamage;
            Destroy(shieldBar.gameObject);
        }

        IEnumerator PointStart(IGameModeHandler gm)
        {
            shieldBar.CurrentHealth = 0f;
            for (int i = 0; i < player.data.stats.GetAdditionalData().curses; i++)
            {
                shieldBar.CurrentHealth += 20f;
            }

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
        }
    }
}