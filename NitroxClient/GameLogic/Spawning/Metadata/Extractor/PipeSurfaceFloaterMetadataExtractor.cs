using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using NitroxClient.GameLogic.Spawning.Metadata.Extractor.Abstract;

namespace NitroxClient.GameLogic.Spawning.Metadata.Extractor;

public class PipeSurfaceFloaterMetadataExtractor : EntityMetadataExtractor<PipeSurfaceFloater, PipeSurfaceFloaterMetadata>
{
    public override PipeSurfaceFloaterMetadata Extract(PipeSurfaceFloater floater)
    {
        // deployed is a public field that determines if the pump is active
        Log.Debug($"[PipeSurfaceFloaterMetadataExtractor] Extracting metadata: deployed={floater.deployed}, isKinematic={floater.rigidBody != null && floater.rigidBody.isKinematic}");
        return new PipeSurfaceFloaterMetadata(floater.deployed);
    }
}
