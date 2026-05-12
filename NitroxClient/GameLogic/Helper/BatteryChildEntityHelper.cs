using System;
using System.Collections.Generic;
using NitroxClient.MonoBehaviours;
using Nitrox.Model.Core;
using Nitrox.Model.DataStructures;
using Nitrox.Model.Subnautica.DataStructures.GameLogic;
using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities;
using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using NitroxClient.GameLogic.Spawning.Metadata;
using UnityEngine;
using UWE;

namespace NitroxClient.GameLogic.Helper;

/// <summary>
/// Vehicles and items are created without a battery loaded into them. Subnautica usually spawns these in async; however, this
/// is disabled in nitrox so we can properly tag the id. Here we create the installed battery (with a new NitroxId) and have the 
/// entity spawner take care of loading it in.
/// </summary>
public static class BatteryChildEntityHelper
{
    private static readonly Lazy<Entities> entities = new (() => NitroxServiceLocator.LocateService<Entities>());
    private static readonly Lazy<EntityMetadataManager> entityMetadataManager = new(() => NitroxServiceLocator.LocateService<EntityMetadataManager>());

    public static void TryPopulateInstalledBattery(GameObject gameObject, List<Entity> toPopulate, NitroxId parentId)
    {
        if (gameObject.TryGetComponent(out EnergyMixin energyMixin))
        {
            PopulateInstalledBattery(energyMixin, toPopulate, parentId);
        }
    }

    public static void PopulateInstalledBattery(EnergyMixin energyMixin, List<Entity> toPopulate, NitroxId parentId)
    {
        GameObject parentObject = NitroxEntity.RequireObjectFrom(parentId);
        
        // Initialize the EnergyMixin if it hasn't been initialized yet
        // This is important for newly crafted items that need their batteries spawned
        if (energyMixin.capacity == 0f && energyMixin.defaultBattery != TechType.None)
        {
            energyMixin.Initialize();
            energyMixin.RestoreBattery(); // This should spawn the default battery
            Log.Debug($"[Battery Debug] Initialized and restored battery for {parentObject.name}");
        }
        
        // Log the state of the EnergyMixin
        Log.Debug($"[Battery Debug] PopulateInstalledBattery for {parentObject.name}:");
        Log.Debug($"[Battery Debug]   - energyMixin.battery: {(energyMixin.battery != null ? energyMixin.battery.GetType().Name : "null")}");
        Log.Debug($"[Battery Debug]   - energyMixin.defaultBattery: {energyMixin.defaultBattery}");
        Log.Debug($"[Battery Debug]   - energyMixin.charge: {energyMixin.charge}");
        Log.Debug($"[Battery Debug]   - energyMixin.capacity: {energyMixin.capacity}");
        
        // Only create InstalledBatteryEntity if there's actually a battery to sync
        // This prevents the duplication bug where empty tools get new batteries when dropped/picked up
        bool shouldCreateBatteryEntity = energyMixin.battery != null || energyMixin.charge > 0f;
        
        Log.Debug($"[Battery Debug] Should create InstalledBatteryEntity: {shouldCreateBatteryEntity}");
        
        if (!shouldCreateBatteryEntity)
        {
            Log.Debug($"[Battery Debug] No battery to sync");
            return;
        }

        EnergyMixin[] components = parentObject.GetAllComponentsInChildren<EnergyMixin>();
        int componentIndex = 0;
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == energyMixin)
            {
                componentIndex = i;
                break;
            }
        }

        // Extract battery metadata (including charge) if a battery is currently installed
        EntityMetadata batteryMetadata = null;
        if (energyMixin.battery is Battery battery)
        {
            Log.Debug($"[Battery Debug] Found battery: {battery.gameObject.name}");
            Log.Debug($"[Battery Debug]   - Battery charge: {battery._charge}");
            Log.Debug($"[Battery Debug]   - Battery capacity: {battery._capacity}");
            
            Optional<EntityMetadata> metadata = entityMetadataManager.Value.Extract(battery.gameObject);
            if (metadata.HasValue)
            {
                batteryMetadata = metadata.Value;
                Log.Debug($"[Battery Debug]   - Extracted metadata: {batteryMetadata}");
            }
            else
            {
                Log.Warn($"[Battery Debug] Failed to extract metadata from battery");
            }
        }
        else
        {
            Log.Debug($"[Battery Debug] No battery currently installed, will spawn default battery");
        }

        InstalledBatteryEntity installedBattery = new(componentIndex, new NitroxId(), energyMixin.defaultBattery.ToDto(), batteryMetadata, parentId, []);
        toPopulate.Add(installedBattery);
        
        Log.Debug($"[Battery Debug] Created InstalledBatteryEntity (ComponentIndex: {componentIndex}, BatteryType: {energyMixin.defaultBattery}, HasMetadata: {batteryMetadata != null})");

        CoroutineHost.StartCoroutine(entities.Value.SpawnEntityAsync(installedBattery));
    }
}
