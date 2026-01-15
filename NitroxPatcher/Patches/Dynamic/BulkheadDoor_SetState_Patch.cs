using System.Reflection;
using NitroxClient.Communication;
using NitroxClient.Communication.Abstract;
using NitroxClient.Extensions;
using Nitrox.Model.DataStructures;
using Nitrox.Model.Subnautica.Extensions;
using Nitrox.Model.Subnautica.Packets;

namespace NitroxPatcher.Patches.Dynamic;

/// <summary>
/// Broadcasts bulkhead door state changes to other players.
/// </summary>
public sealed partial class BulkheadDoor_SetState_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((BulkheadDoor t) => t.SetState(default));

    public static void Postfix(BulkheadDoor __instance, bool open)
    {
        // Skip if suppressed (e.g., when processing incoming packets)
        if (PacketSuppressor<BulkheadDoorStateChanged>.IsSuppressed)
        {
            return;
        }

        // Skip if this is being called during initial setup (Awake)
        if (!__instance.gameObject.activeInHierarchy)
        {
            return;
        }

        // Get the BaseDeconstructable which contains the face information
        if (!__instance.TryGetComponentInParent(out BaseDeconstructable baseDeconstructable, true) ||
            !baseDeconstructable.face.HasValue)
        {
            return;
        }

        // Get the Base component
        if (!__instance.TryGetComponentInParent(out Base @base, true) ||
            !@base.TryGetNitroxId(out NitroxId baseId))
        {
            return;
        }

        BulkheadDoorStateChanged packet = new(baseId, baseDeconstructable.face.Value.ToDto(), open);
        Resolve<IPacketSender>().Send(packet);
    }
}
