using System.Collections.Generic;
using NitroxClient.GameLogic;
using NitroxClient.GameLogic.Spawning.Metadata;
using NitroxClient.MonoBehaviours;
using Nitrox.Model.DataStructures;
using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using UnityEngine;

namespace NitroxClient.MonoBehaviours;

/// <summary>
/// Periodically syncs oxygen tank values to the server so they persist across reconnects
/// </summary>
public class OxygenTankBroadcaster : MonoBehaviour
{
    private float time;
    private const float BROADCAST_INTERVAL = 5f;
    private Entities entities;
    private EntityMetadataManager entityMetadataManager;

    public void Awake()
    {
        entities = this.Resolve<Entities>();
        entityMetadataManager = this.Resolve<EntityMetadataManager>();
    }

    public void Update()
    {
        time += Time.deltaTime;

        if (time >= BROADCAST_INTERVAL)
        {
            time = 0;
            SyncOxygenTanks();
        }
    }

    private void SyncOxygenTanks()
    {
        if (!Player.main)
        {
            return;
        }

        Inventory inventory = Inventory.main;
        if (!inventory)
        {
            return;
        }

        // Sync oxygen tanks in player inventory
        foreach (InventoryItem item in inventory.container)
        {
            if (item?.item && item.item.TryGetComponent(out Oxygen oxygen))
            {
                UpdateOxygenMetadata(item.item.gameObject, oxygen);
            }
        }

        // Sync equipped oxygen tanks
        Equipment equipment = inventory.equipment;
        if (equipment != null)
        {
            foreach (KeyValuePair<string, InventoryItem> kvp in equipment.equipment)
            {
                InventoryItem item = kvp.Value;
                if (item?.item && item.item.TryGetComponent(out Oxygen oxygen))
                {
                    UpdateOxygenMetadata(item.item.gameObject, oxygen);
                }
            }
        }
    }

    private void UpdateOxygenMetadata(GameObject gameObject, Oxygen oxygen)
    {
        if (!gameObject.TryGetNitroxId(out NitroxId id))
        {
            return;
        }

        // Only update if oxygen value is meaningful (> 0)
        if (oxygen.oxygenAvailable > 0)
        {
            OxygenMetadata metadata = new(oxygen.oxygenAvailable);
            entities.BroadcastMetadataUpdate(id, metadata);
        }
    }
}
