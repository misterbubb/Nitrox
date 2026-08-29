using System.Reflection;
using NitroxClient.Communication.Abstract;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using Nitrox.Model.DataStructures;
using Nitrox.Model.Subnautica.Packets;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class LiveMixin_Kill_Patch : NitroxPatch, IDynamicPatch
{
    internal static readonly MethodInfo TARGET_METHOD = Reflect.Method((LiveMixin t) => t.Kill(default));

    public static void Postfix(LiveMixin __instance)
    {
        if (!Multiplayer.Main || !Multiplayer.Main.InitialSyncCompleted)
        {
            return;
        }

        // We don't broadcast if we don't have objectId or if the object is whitelisted,
        // in which case kill broadcast is managed differently
        if (!__instance.TryGetNitroxId(out NitroxId objectId))
        {
            return;
        }

        if (Resolve<LiveMixinManager>().IsWhitelistedUpdateType(__instance))
        {
            return;
        }
        
        // Broadcast destruction if any of these conditions are true:
        // 1. destroyOnDeath: LiveMixin will destroy the object directly
        // 2. broadcastKillOnDeath: The object broadcasts OnKill message (default true) which other scripts use for destruction
        // 3. passDamageDataOnDeath: Similar to broadcastKillOnDeath but passes damage data
        // 4. ShouldBroadcastDeath: Whitelist for specific entities (e.g., Crash fish)
        if (__instance.destroyOnDeath || 
            __instance.broadcastKillOnDeath || 
            __instance.passDamageDataOnDeath || 
            Resolve<LiveMixinManager>().ShouldBroadcastDeath(__instance))
        {
            Resolve<IPacketSender>().Send(new EntityDestroyed(objectId));
        }
    }
}
