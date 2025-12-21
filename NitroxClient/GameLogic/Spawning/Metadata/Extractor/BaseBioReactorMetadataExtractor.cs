using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using NitroxClient.GameLogic.Spawning.Metadata.Extractor.Abstract;

namespace NitroxClient.GameLogic.Spawning.Metadata.Extractor;

public class BaseBioReactorMetadataExtractor : EntityMetadataExtractor<BaseBioReactor, BaseBioReactorMetadata>
{
    public override BaseBioReactorMetadata Extract(BaseBioReactor entity)
    {
        float power = 0f;
        if (entity.TryGetComponent(out PowerSource powerSource))
        {
            power = powerSource.power;
        }

        return new(entity._toConsume, power);
    }
}
