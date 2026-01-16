using System.Reflection;
using NitroxClient.Communication.Abstract;
using Nitrox.Model.Subnautica.Packets;

namespace NitroxPatcher.Patches.Dynamic;

/// <summary>
/// Broadcasts when a PDA log notification is dismissed (user viewed the entry).
/// </summary>
public sealed partial class NotificationManager_Remove_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((NotificationManager t) => t.Remove(default, default(string)));

    public static void Postfix(NotificationManager.Group group, string key)
    {
        // Only track log entry notifications being read
        if (group != NotificationManager.Group.Log)
        {
            return;
        }

        Resolve<IPacketSender>().Send(new PDALogEntryRead(key));
    }
}
