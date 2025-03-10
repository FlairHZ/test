using FC.Extensions;
using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;

namespace FlairsCards.MonoBehaviours
{
    class RouletteMono : MonoBehaviour
    {
        private Player player; 
        private Gun gun;
        private GunAmmo gunAmmo;
        private void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            gunAmmo = gun.GetComponentInChildren<GunAmmo>();
        }
    }
}
