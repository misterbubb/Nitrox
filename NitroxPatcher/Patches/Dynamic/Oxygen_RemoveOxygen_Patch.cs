using System;
using System.Reflection;
using Nitrox.Model.DataStructures;
using NitroxClient.GameLogic;

namespace NitroxPatcher.Patches.Dynamic;

/// <summary>
/// Syncs oxygen tank oxygen level when oxygen is removed.
/// </summary>
public sealed partial class Oxygen_RemoveOxygen_Patch : NitroxPatch, IDynamicPatch
{
    public static readonly MethodInfo TARGET_METHOD = Reflect.Method((Oxygen t) => t.RemoveOxygen(default));

    public static void Prefix(Oxygen __instance, out float __state)
    {
        __state = __instance.oxygenAvailable;
    }

    public static void Postfix(Oxygen __instance, float __state)
    {
        // Only sync oxygen tanks (Pickupable items), not the player's oxygen manager
        if (!__instance.GetComponent<Pickupable>())
        {
            return;
        }

        // Only sync if oxygen level changed by at least 1 unit (avoid spam from continuous small changes)
        if (Math.Abs(Math.Floor(__instance.oxygenAvailable) - Math.Floor(__state)) > 0.0 &&
            __instance.TryGetIdOrWarn(out NitroxId id))
        {
            Resolve<Entities>().EntityMetadataChanged(__instance, id);
        }
    }
}
