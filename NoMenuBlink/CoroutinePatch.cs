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
        [HarmonyPatch(typeof(CoroutineManager), nameof(CoroutineManager.BlinkIn), new Type[] {typeof(GameObject), typeof(float)})]
        [HarmonyPrefix]
        public static bool CancelBlink(GameObject go, ref Coroutine __result)
        {
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