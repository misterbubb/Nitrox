using System.Collections.Generic;
using NitroxClient.Communication.Abstract;
using NitroxClient.MonoBehaviours;
using Nitrox.Model.DataStructures;
using Nitrox.Model.Packets;
using Nitrox.Model.Subnautica.DataStructures.GameLogic;
using Nitrox.Model.Subnautica.Packets;
using UnityEngine;

namespace NitroxClient.GameLogic.PlayerLogic;

public class PlayerCinematics
{
    private readonly IPacketSender packetSender;
    private readonly LocalPlayer localPlayer;

    private IntroCinematicMode lastModeToSend = IntroCinematicMode.NONE;

    public ushort? IntroCinematicPartnerId = null;

    /// <summary>
    /// Some cinematics should not be played. Example the intro as it's completely handled by a dedicated system.
    /// </summary>
    private readonly HashSet<string> blacklistedKeys = ["escapepod_intro", "reaper_attack"];

    public PlayerCinematics(IPacketSender packetSender, LocalPlayer localPlayer)
    {
        this.packetSender = packetSender;
        this.localPlayer = localPlayer;
    }

    public void StartCinematicMode(ushort playerId, NitroxId controllerID, int controllerNameHash, string key)
    {
        if (!blacklistedKeys.Contains(key))
        {
            NitroxId heldItemId = GetHeldItemId();
            packetSender.Send(new PlayerCinematicControllerCall(playerId, controllerID, controllerNameHash, key, true, heldItemId));
        }
    }

    public void EndCinematicMode(ushort playerId, NitroxId controllerID, int controllerNameHash, string key)
    {
        if (!blacklistedKeys.Contains(key))
        {
            packetSender.Send(new PlayerCinematicControllerCall(playerId, controllerID, controllerNameHash, key, false));
        }
    }

    /// <summary>
    /// Gets the NitroxId of an item currently held in the player's tool socket (used during cinematics like tablet insertion).
    /// </summary>
    private static NitroxId GetHeldItemId()
    {
        if (!Inventory.main || !Inventory.main.toolSocket)
        {
            return null;
        }

        // Check if there's an item parented to the tool socket (this happens during terminal cinematics)
        foreach (Transform child in Inventory.main.toolSocket)
        {
            if (child.gameObject.activeInHierarchy && child.TryGetComponent(out NitroxEntity entity))
            {
                return entity.Id;
            }
        }

        return null;
    }

    public void SetLocalIntroCinematicMode(IntroCinematicMode introCinematicMode)
    {
        if (!localPlayer.PlayerId.HasValue)
        {
            Log.Error($"PlayerId was null while setting IntroCinematicMode to {introCinematicMode}");
            return;
        }

        if (localPlayer.IntroCinematicMode == introCinematicMode)
        {
            return;
        }

        localPlayer.IntroCinematicMode = introCinematicMode;

        // This method can be called before client is joined. To prevent sending as an unauthenticated packet we delay it.
        if (Multiplayer.Joined)
        {
            packetSender.Send(new SetIntroCinematicMode(localPlayer.PlayerId.Value, introCinematicMode));
            return;
        }

        if (lastModeToSend == IntroCinematicMode.NONE)
        {
            Multiplayer.OnLoadingComplete += () => SetLocalIntroCinematicMode(lastModeToSend);
        }

        lastModeToSend = introCinematicMode;
    }
}
