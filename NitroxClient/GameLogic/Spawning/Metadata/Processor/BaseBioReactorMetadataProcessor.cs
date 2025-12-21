using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using NitroxClient.GameLogic.Spawning.Metadata.Processor.Abstract;
using UnityEngine;

namespace NitroxClient.GameLogic.Spawning.Metadata.Processor;

public class BaseBioReactorMetadataProcessor : EntityMetadataProcessor<BaseBioReactorMetadata>
{
    public override void ProcessMetadata(GameObject gameObject, BaseBioReactorMetadata metadata)
    {
        BaseBioReactor baseBioReactor = gameObject.GetComponentInChildren<BaseBioReactor>(true);

        if (baseBioReactor)
        {
            baseBioReactor._toConsume = metadata.ToConsume;

            if (baseBioReactor.TryGetComponent(out PowerSource powerSource))
            {
                powerSource.SetPower(metadata.Power);
            }
        }
        else
        {
            Log.Error($"Could not find BaseBioReactor on {gameObject.name}");
        }
    }
}
