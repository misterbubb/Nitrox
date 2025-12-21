using System;
using System.Runtime.Serialization;
using BinaryPack.Attributes;

namespace Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;

[Serializable, DataContract]
public class PipeSurfaceFloaterMetadata : EntityMetadata
{
    /// <summary>
    /// Whether the pump has been deployed (dropped in water and activated).
    /// Combined with rigidBody.isKinematic (at surface), this determines if oxygen is provided.
    /// </summary>
    [DataMember(Order = 1)]
    public bool Deployed { get; }

    [IgnoreConstructor]
    protected PipeSurfaceFloaterMetadata()
    {
        // Constructor for serialization. Has to be "protected" for json serialization.
    }

    public PipeSurfaceFloaterMetadata(bool deployed)
    {
        Deployed = deployed;
    }

    public override string ToString()
    {
        return $"[{nameof(PipeSurfaceFloaterMetadata)} Deployed: {Deployed}]";
    }
}
