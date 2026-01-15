using Nitrox.Model.DataStructures;
using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxClient.MonoBehaviours.CinematicController;
using Nitrox.Model.Helper;
using Nitrox.Model.Packets;
using Nitrox.Model.Subnautica.Packets;
using UnityEngine;

namespace NitroxClient.Communication.Packets.Processors;

public class PlayerCinematicControllerCallProcessor : ClientPacketProcessor<PlayerCinematicControllerCall>
{
    private readonly PlayerManager playerManager;

    public PlayerCinematicControllerCallProcessor(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public override void Process(PlayerCinematicControllerCall packet)
    {
        if (!NitroxEntity.TryGetObjectFrom(packet.ControllerID, out GameObject entity))
        {
            return; // Entity can be not spawned yet bc async.
        }

        if (!entity.TryGetComponent(out MultiplayerCinematicReference reference))
        {
            Log.Warn($"Couldn't find {nameof(MultiplayerCinematicReference)} on {entity.name}:{packet.ControllerID}");
            return;
        }

        Optional<RemotePlayer> opPlayer = playerManager.Find(packet.PlayerId);
        Validate.IsPresent(opPlayer);

        if (packet.StartPlaying)
        {
            // Handle held item visibility during cinematic
            if (packet.HeldItemId != null)
            {
                AttachHeldItemToPlayer(packet.HeldItemId, opPlayer.Value);
            }

            reference.CallStartCinematicMode(packet.Key, packet.ControllerNameHash, opPlayer.Value);
        }
        else
        {
            reference.CallCinematicModeEnd(packet.Key, packet.ControllerNameHash, opPlayer.Value);

            // Hide any held item when cinematic ends (item is typically consumed/destroyed)
            DetachHeldItemFromPlayer(opPlayer.Value);
        }
    }

    private static void AttachHeldItemToPlayer(NitroxId itemId, RemotePlayer player)
    {
        if (!NitroxEntity.TryGetObjectFrom(itemId, out GameObject itemObject))
        {
            Log.Warn($"[{nameof(PlayerCinematicControllerCallProcessor)}] Could not find held item with id {itemId} for cinematic");
            return;
        }

        // Store original parent so we can restore if needed
        player.SetCinematicHeldItem(itemObject);

        // Attach to player's item attach point
        itemObject.transform.SetParent(player.ItemAttachPoint, false);
        itemObject.transform.localPosition = Vector3.zero;
        itemObject.transform.localRotation = Quaternion.identity;
        itemObject.SetActive(true);

        // Set to viewmodel layer so it renders properly
        int viewModelLayer = LayerMask.NameToLayer("Viewmodel");
        Utils.SetLayerRecursively(itemObject, viewModelLayer);
    }

    private static void DetachHeldItemFromPlayer(RemotePlayer player)
    {
        GameObject heldItem = player.GetCinematicHeldItem();
        if (heldItem != null)
        {
            // Hide the item - it will be destroyed/consumed by the terminal logic
            heldItem.SetActive(false);

            // Reset layer
            int defaultLayer = LayerMask.NameToLayer("Default");
            Utils.SetLayerRecursively(heldItem, defaultLayer);

            player.ClearCinematicHeldItem();
        }
    }
}
