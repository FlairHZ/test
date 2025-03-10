using FC.Extensions;
using ModdingUtils.MonoBehaviours;
using static ModdingUtils.Utils.SortingController;
using ModsPlus;
using Photon.Pun;
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

namespace FlairsCards.Monobehaviours
{
    class GamblerMono : MonoBehaviourPun
    {
        private Player player;
        private CustomHealthBar luckBar;
        private void Start()
        {
            player = GetComponent<Player>();
            GameModeManager.AddHook(GameModeHooks.HookRoundStart, RoundStart);
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, PointEnd);
        }
        private void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookRoundStart, RoundStart);
            GameModeManager.RemoveHook(GameModeHooks.HookPointEnd, PointEnd);
        }
        IEnumerator PointEnd(IGameModeHandler gm)
        {
            Destroy(luckBar.gameObject);
            yield break;
        }

        IEnumerator RoundStart(IGameModeHandler gm)
        {
            var parent = player.GetComponentInChildren<PlayerWobblePosition>().transform;
            var obj = new GameObject("Shield Bar");
            obj.transform.SetParent(parent);
            luckBar = obj.AddComponent<CustomHealthBar>();
            luckBar.transform.localPosition = Vector3.up * 0.25f;
            luckBar.transform.localScale = Vector3.one;
            int luck = player.data.stats.GetAdditionalData().luck;
            float barHealth = GetBarHealth(luck);
            Color barColor = GetBarColor(luck);
            photonView.RPC("RPCA_SyncLuckBar", RpcTarget.AllBuffered, barHealth, barColor.r, barColor.g, barColor.b);
            yield break;
        }
        private float GetBarHealth(int luck)
        {
            if (luck <= -2) return 0f;
            if (luck <= -1) return 15f;
            if (luck == 0) return 30f;
            if (luck == 1) return 45f;
            if (luck == 2) return 60f;
            if (luck <= 4) return 80f;
            return 100f;
        }

        private Color GetBarColor(int luck)
        {
            if (luck <= -2) return Color.black;
            if (luck <= -1) return Color.red;
            if (luck == 0) return Color.yellow;
            if (luck == 1) return Color.cyan;
            if (luck == 2) return Color.blue;
            if (luck <= 4) return Color.green;
            return Color.white;
        }

        [PunRPC]
        private void RPCA_SyncLuckBar(float health, float r, float g, float b)
        {
            if (luckBar == null) return;

            luckBar.CurrentHealth = health;
            luckBar.SetColor(new Color(r, g, b));
        }

    }
}
