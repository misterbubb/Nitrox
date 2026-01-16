using System;
using Nitrox.Model.Packets;

namespace Nitrox.Model.Subnautica.Packets;

/// <summary>
/// Packet sent when a PDA log entry notification has been dismissed (user viewed the entry).
/// </summary>
[Serializable]
public class PDALogEntryRead : Packet
{
    public string Key { get; }

    public PDALogEntryRead(string key)
    {
        Key = key;
    }
}
