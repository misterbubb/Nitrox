using System.Reflection;
using Nitrox.Model.DataStructures;
using NitroxClient.GameLogic;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class Equipment_RemoveItem_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((Equipment t) => t.RemoveItem(default, default, default));

    public static void Postfix(Equipment __instance, InventoryItem __result)
    {
        if (__result != null)
        {
            Resolve<EquipmentSlots>().BroadcastUnequip(__result.item, __instance.owner);

            // Sync oxygen tank metadata when unequipped
            if (__result.item.TryGetComponent(out Oxygen oxygen) && 
                __result.item.TryGetIdOrWarn(out NitroxId id))
            {
                Resolve<Entities>().EntityMetadataChanged(oxygen, id);
            }
        }
    }
}
