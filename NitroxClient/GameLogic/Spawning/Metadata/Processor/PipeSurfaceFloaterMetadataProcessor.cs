using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using NitroxClient.GameLogic.Spawning.Metadata.Processor.Abstract;
using UnityEngine;

namespace NitroxClient.GameLogic.Spawning.Metadata.Processor;

public class PipeSurfaceFloaterMetadataProcessor : EntityMetadataProcessor<PipeSurfaceFloaterMetadata>
{
    public override void ProcessMetadata(GameObject gameObject, PipeSurfaceFloaterMetadata metadata)
    {
        if (!gameObject.TryGetComponent(out PipeSurfaceFloater floater))
        {
            Log.Error($"[{nameof(PipeSurfaceFloaterMetadataProcessor)}] Can't apply metadata to {gameObject.name} because it doesn't have a {nameof(PipeSurfaceFloater)} component");
            return;
        }

        // Set deployed state - this is required for GetProvidesOxygen() to return true
        floater.deployed = metadata.Deployed;

        // If deployed, the pump should be at the surface with kinematic rigidbody
        // The physics will naturally handle floating to surface, but we ensure kinematic is set
        // when deployed so GetProvidesOxygen() returns true immediately
        if (metadata.Deployed && floater.rigidBody != null)
        {
            UWE.Utils.SetIsKinematicAndUpdateInterpolation(floater.rigidBody, true, false);
        }

        // Debug: Check if UniqueIdentifier is properly set
        if (gameObject.TryGetComponent(out UniqueIdentifier uid))
        {
            bool isKinematic = floater.rigidBody != null && floater.rigidBody.isKinematic;
            Log.Debug($"[PipeSurfaceFloaterMetadataProcessor] UniqueIdentifier.Id = {uid.Id}, deployed = {metadata.Deployed}, isKinematic = {isKinematic}, GetProvidesOxygen = {floater.GetProvidesOxygen()}");
        }
        else
        {
            Log.Warn($"[PipeSurfaceFloaterMetadataProcessor] No UniqueIdentifier on {gameObject.name}");
        }
    }
}
