using System;
using Nitrox.Model.DataStructures;
using Nitrox.Model.Packets;

namespace Nitrox.Model.Subnautica.Packets;

[Serializable]
public class PlayerCinematicControllerCall : Packet
{
    public ushort PlayerId { get; }
    public NitroxId ControllerID { get; }
    public int ControllerNameHash { get; }
    public string Key { get; }
    public bool StartPlaying { get; }
    /// <summary>
    /// Optional ID of an item held in the player's hand during the cinematic (e.g., tablets, ion cubes).
    /// </summary>
    public NitroxId HeldItemId { get; }

    public PlayerCinematicControllerCall(ushort playerId, NitroxId controllerID, int controllerNameHash, string key, bool startPlaying, NitroxId heldItemId = null)
    {
        PlayerId = playerId;
        ControllerID = controllerID;
        ControllerNameHash = controllerNameHash;
        Key = key;
        StartPlaying = startPlaying;
        HeldItemId = heldItemId;
    }
}
