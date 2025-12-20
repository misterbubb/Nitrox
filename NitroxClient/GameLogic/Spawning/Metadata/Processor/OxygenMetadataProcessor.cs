using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using NitroxClient.GameLogic.Spawning.Metadata.Processor.Abstract;
using UnityEngine;

namespace NitroxClient.GameLogic.Spawning.Metadata.Processor;

public class OxygenMetadataProcessor : EntityMetadataProcessor<OxygenMetadata>
{
    public override void ProcessMetadata(GameObject gameObject, OxygenMetadata metadata)
    {
        Oxygen oxygen = gameObject.GetComponent<Oxygen>();

        if (oxygen)
        {
            oxygen.oxygenAvailable = metadata.OxygenAvailable;
        }
        else
        {
            Log.Error($"Could not find Oxygen on {gameObject.name}");
        }
    }
}
