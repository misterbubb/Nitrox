using System;
using Nitrox.Model.DataStructures;
using Nitrox.Model.Packets;
using Nitrox.Model.Subnautica.DataStructures.GameLogic.Bases;

namespace Nitrox.Model.Subnautica.Packets;

[Serializable]
public class BulkheadDoorStateChanged : Packet
{
    public NitroxId BaseId { get; }
    public NitroxBaseFace Face { get; }
    public bool IsOpen { get; }

    public BulkheadDoorStateChanged(NitroxId baseId, NitroxBaseFace face, bool isOpen)
    {
        BaseId = baseId;
        Face = face;
        IsOpen = isOpen;
    }
}
