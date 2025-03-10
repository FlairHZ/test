using FC.Extensions;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;
using static FlairsCards.Cards.RandomBullet;

namespace FlairsCards.MonoBehaviours
{
    class RevengeMono : MonoBehaviour
    {
        private Player player;
        private Gun gun;
        bool takenDamage = false;
        float originalDamage;
        private void Start()
        {
            player = GetComponent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            player.data.stats.WasDealtDamageAction += OnDamage;
            gun.ShootPojectileAction += OnShoot;
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, PointEnd);
        }

        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookPointEnd, PointEnd);
            player.data.stats.WasDealtDamageAction -= OnDamage;
            gun.ShootPojectileAction -= OnShoot;

        }
        public void OnShoot(GameObject projectile)
        {
            if (takenDamage)
            {
                gun.damage = originalDamage;
                takenDamage = false;
            }
        }
        public void OnDamage(Vector2 damage, bool selfDamage)
        {
            if (!takenDamage)
            {
                originalDamage = gun.damage;
                gun.damage *= 2;
                takenDamage = true;
            }
        }
        IEnumerator PointEnd(IGameModeHandler gm)
        {
            gun.damage = originalDamage;
            takenDamage = false;
            yield break;
        }
    }
}