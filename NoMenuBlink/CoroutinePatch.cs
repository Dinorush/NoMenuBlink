using CellMenu;
using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;
using CollectionExtensions = BepInEx.Unity.IL2CPP.Utils.Collections.CollectionExtensions;

namespace ColorCrosshair
{
    [HarmonyPatch]
    internal static class CoroutinePatch
    {
        private static bool _inLoadout = false;
        [HarmonyPatch(typeof(CM_InventorySlotItem), nameof(CM_InventorySlotItem.OnBtnPress))]
        [HarmonyPrefix]
        private static void Pre_Select() => _inLoadout = true;

        [HarmonyPatch(typeof(CM_InventorySlotItem), nameof(CM_InventorySlotItem.OnBtnPress))]
        [HarmonyPostfix]
        private static void Post_Select() => _inLoadout = false;

        [HarmonyPatch(typeof(CoroutineManager), nameof(CoroutineManager.BlinkIn), new Type[] {typeof(GameObject), typeof(float)})]
        [HarmonyPrefix]
        private static bool CancelBlink(GameObject go, ref Coroutine __result)
        {
            if (!_inLoadout) return true;

            __result = CoroutineManager.StartCoroutine(CollectionExtensions.WrapToIl2Cpp(EmptyRoutine()));
            if (go != null)
                go.SetActive(true);
            return false;
        }

        private static IEnumerator EmptyRoutine()
        {
            yield return null;
        }
    }
}