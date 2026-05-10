using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using NitroxClient.GameLogic.Spawning.Metadata.Processor.Abstract;
using UnityEngine;

namespace NitroxClient.GameLogic.Spawning.Metadata.Processor;

public class OxygenMetadataProcessor : EntityMetadataProcessor<OxygenMetadata>
{
    public override void ProcessMetadata(GameObject gameObject, OxygenMetadata metadata)
    {
        Log.Debug($"[OxygenMetadataProcessor] Processing metadata for {gameObject.name}, oxygen: {metadata.OxygenAvailable}");
        
        Oxygen oxygen = gameObject.GetComponent<Oxygen>();

        if (oxygen)
        {
            Log.Debug($"[OxygenMetadataProcessor] Found Oxygen component, setting oxygenAvailable to {metadata.OxygenAvailable}");
            oxygen.oxygenAvailable = metadata.OxygenAvailable;
        }
        else
        {
            Log.Warn($"[OxygenMetadataProcessor] Could not find Oxygen component on {gameObject.name}");
        }
    }
}
