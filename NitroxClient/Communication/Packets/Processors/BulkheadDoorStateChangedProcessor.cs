using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.MonoBehaviours;
using Nitrox.Model.Subnautica.Extensions;
using Nitrox.Model.Subnautica.Packets;
using UnityEngine;

namespace NitroxClient.Communication.Packets.Processors;

public class BulkheadDoorStateChangedProcessor : ClientPacketProcessor<BulkheadDoorStateChanged>
{
    public override void Process(BulkheadDoorStateChanged packet)
    {
        if (!NitroxEntity.TryGetComponentFrom(packet.BaseId, out Base @base))
        {
            Log.Warn($"[{nameof(BulkheadDoorStateChangedProcessor)}] Couldn't find Base with id {packet.BaseId}");
            return;
        }

        Base.Face face = packet.Face.ToUnity();

        // Find the face object that contains the BulkheadDoor
        Transform faceObject = @base.FindFaceObject(face);
        if (!faceObject)
        {
            Log.Warn($"[{nameof(BulkheadDoorStateChangedProcessor)}] Couldn't find face object for face {face}");
            return;
        }

        BulkheadDoor bulkheadDoor = faceObject.GetComponentInChildren<BulkheadDoor>();
        if (!bulkheadDoor)
        {
            Log.Warn($"[{nameof(BulkheadDoorStateChangedProcessor)}] Couldn't find BulkheadDoor component in face object");
            return;
        }

        // Only update if the state is different to avoid triggering the patch again
        if (bulkheadDoor.opened != packet.IsOpen)
        {
            using (PacketSuppressor<BulkheadDoorStateChanged>.Suppress())
            {
                bulkheadDoor.SetState(packet.IsOpen);
            }
        }
    }
}
