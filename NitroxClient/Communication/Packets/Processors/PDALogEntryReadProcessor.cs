using NitroxClient.Communication.Packets.Processors.Abstract;
using Nitrox.Model.Subnautica.Packets;

namespace NitroxClient.Communication.Packets.Processors;

/// <summary>
/// Processes when another player has read a PDA log entry (dismissed its notification).
/// </summary>
public class PDALogEntryReadProcessor : ClientPacketProcessor<PDALogEntryRead>
{
    public override void Process(PDALogEntryRead packet)
    {
        using (PacketSuppressor<PDALogEntryRead>.Suppress())
        {
            NotificationManager.main.Remove(NotificationManager.Group.Log, packet.Key);
        }
    }
}
