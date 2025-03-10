using System;
using System.Runtime.CompilerServices;
using FlairsCards.Cards;
using HarmonyLib;
using Photon.Pun.Simple.Pooling;

namespace FC.Extensions
{
    [Serializable]
    public class CharacterStatModifiersAdditionalData
    {
        public int curses;
        public int luck;
        public bool curseAverse = false;
        public bool overCharged = false;
        public CharacterStatModifiersAdditionalData()
        {
            curses = 0;
            luck = 0;
            curseAverse = false;
            overCharged = false;
        }
    }
    public static class CharacterStatModifiersExtension
    {
        public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData> data =
            new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData>();

        public static CharacterStatModifiersAdditionalData GetAdditionalData(this CharacterStatModifiers statModifiers)
        {
            return data.GetOrCreateValue(statModifiers);
        }

        public static void AddData(this CharacterStatModifiers statModifiers, CharacterStatModifiersAdditionalData value)
        {
            try
            {
                data.Add(statModifiers, value);
            }
            catch (Exception) { }
        }
    }

    [HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
    class CharacterStatModifiersPatchResetStats
    {
        private static void Prefix(CharacterStatModifiers __instance)
        {
            __instance.GetAdditionalData().curses = 0;
            __instance.GetAdditionalData().luck = 0;
            __instance.GetAdditionalData().curseAverse = false;
            __instance.GetAdditionalData().overCharged = false;
        }
    }
}