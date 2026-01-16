using Nitrox.Model.Subnautica.Packets;
using Nitrox.Server.Subnautica.Models.Packets.Processors.Core;
using Nitrox.Server.Subnautica.Models.GameLogic;
using Nitrox.Server.Subnautica.Models.GameLogic.Unlockables;

namespace Nitrox.Server.Subnautica.Models.Packets.Processors;

public class PDALogEntryReadProcessor : AuthenticatedPacketProcessor<PDALogEntryRead>
{
    private readonly PlayerManager playerManager;
    private readonly PDAStateData pdaState;

    public PDALogEntryReadProcessor(PlayerManager playerManager, PDAStateData pdaState)
    {
        this.playerManager = playerManager;
        this.pdaState = pdaState;
    }

    public override void Process(PDALogEntryRead packet, Player player)
    {
        pdaState.MarkPDALogEntryRead(packet.Key);
        playerManager.SendPacketToOtherPlayers(packet, player);
    }
}
