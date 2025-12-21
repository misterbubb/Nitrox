using System.Reflection;
using NitroxClient.GameLogic;
using Nitrox.Model.DataStructures;

namespace NitroxPatcher.Patches.Dynamic;

/// <summary>
/// Syncs bioreactor state when items are consumed during power production.
/// </summary>
public sealed partial class BaseBioReactor_ProducePower_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = typeof(BaseBioReactor).GetMethod("ProducePower", BindingFlags.NonPublic | BindingFlags.Instance);

    public static void Postfix(BaseBioReactor __instance, float __result)
    {
        // Only sync when power was actually produced and we have an id
        if (__result > 0f && __instance.TryGetIdOrWarn(out NitroxId id))
        {
            Resolve<Entities>().EntityMetadataChangedThrottled(__instance, id, 5f);
        }
    }
}
