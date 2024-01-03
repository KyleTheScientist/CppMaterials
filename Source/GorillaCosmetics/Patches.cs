using CppMaterials.Source.Tools;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using Photon.Realtime;

namespace CppMaterials.Source.GorillaCosmetics
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    internal class ChangeMaterialPatch
    {
        internal static void Postfix(VRRig __instance)
        {
            __instance.gameObject.GetOrAddComponent<MaterialController>();
        }
    }

    [HarmonyPatch]
    public class VRRigAddedPatch
    {
        public static Action<Player, VRRig> OnRigAdded;

        static IEnumerable<MethodBase> TargetMethods()
        {
            return new MethodBase[] {
                AccessTools.Method("VRRigCache:AddRigToGorillaParent")
            };
        }

        private static void Postfix(Player player, VRRig vrrig)
        {
            OnRigAdded?.Invoke(player, vrrig);
        }
    }

    [HarmonyPatch]
    public class VRRigRemovedPatch
    {
        public static Action<Player, VRRig> OnRigRemoved;

        static IEnumerable<MethodBase> TargetMethods()
        {
            return new MethodBase[] {
                AccessTools.Method("VRRigCache:RemoveRigFromGorillaParent")
            };
        }

        private static void Prefix(Player player, VRRig vrrig)
        {
            OnRigRemoved?.Invoke(player, vrrig);
        }

        [HarmonyPatch(typeof(VRRig))]
        [HarmonyPatch("InitializeNoobMaterialLocal", MethodType.Normal)]
        internal class ColorPatch
        {
            internal static void Postfix(VRRig __instance, float red, float green, float blue)
            {
                __instance.materialsToChangeTo[0].color = new Color(red, green, blue);
            }
        }
    }
}
