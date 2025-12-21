using System;
using System.Runtime.Serialization;
using BinaryPack.Attributes;

namespace Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;

[Serializable]
[DataContract]
public class BaseBioReactorMetadata : EntityMetadata
{
    [DataMember(Order = 1)]
    public float ToConsume { get; }

    [DataMember(Order = 2)]
    public float Power { get; }

    [IgnoreConstructor]
    protected BaseBioReactorMetadata()
    {
        // Constructor for serialization. Has to be "protected" for json serialization.
    }

    public BaseBioReactorMetadata(float toConsume, float power)
    {
        ToConsume = toConsume;
        Power = power;
    }

    public override string ToString()
    {
        return $"[BaseBioReactorMetadata ToConsume: {ToConsume}, Power: {Power}]";
    }
}
