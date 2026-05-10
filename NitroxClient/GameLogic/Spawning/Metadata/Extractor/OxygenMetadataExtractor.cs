using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using NitroxClient.GameLogic.Spawning.Metadata.Extractor.Abstract;

namespace NitroxClient.GameLogic.Spawning.Metadata.Extractor;

public class OxygenMetadataExtractor : EntityMetadataExtractor<Oxygen, OxygenMetadata>
{
    public override OxygenMetadata Extract(Oxygen entity)
    {
        float oxygen = entity.oxygenAvailable;
        Log.Debug($"[OxygenMetadataExtractor] Extracting oxygen: {oxygen} from {entity.gameObject.name}");
        return new(oxygen);
    }
}
