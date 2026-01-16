using System;
using System.Collections.Generic;
using BinaryPack.Attributes;
using Nitrox.Model.DataStructures;

namespace Nitrox.Model.Subnautica.DataStructures.GameLogic
{
    [Serializable]
    public class InitialPDAData
    {
        public List<NitroxTechType> KnownTechTypes { get; set; }
        public List<NitroxTechType> AnalyzedTechTypes { get; set; }
        public List<PDALogEntry> PDALogEntries { get; set; }
        public List<string> EncyclopediaEntries { get; set; }
        
        // PDA Scanner data
        public List<NitroxId> ScannerFragments { get; set; }
        public List<PDAEntry> ScannerPartial { get; set; }
        public List<NitroxTechType> ScannerComplete { get; set; }

        /// <summary>
        /// PDA log entries that have been read (notification dismissed).
        /// </summary>
        public List<string> ReadPDALogEntries { get; set; }

        [IgnoreConstructor]
        protected InitialPDAData()
        {
            //Constructor for serialization. Has to be "protected" for json serialization.
        }

        public InitialPDAData(List<NitroxTechType> knownTechTypes, List<NitroxTechType> analyzedTechTypes, List<PDALogEntry> pdaLogEntries, List<string> encyclopediaEntries, List<NitroxId> scannerFragments, List<PDAEntry> scannerPartial, List<NitroxTechType> scannerComplete, List<string> readPDALogEntries)
        {
            KnownTechTypes = knownTechTypes;
            AnalyzedTechTypes = analyzedTechTypes;
            PDALogEntries = pdaLogEntries;
            EncyclopediaEntries = encyclopediaEntries;
            ScannerFragments = scannerFragments;
            ScannerPartial = scannerPartial;
            ScannerComplete = scannerComplete;
            ReadPDALogEntries = readPDALogEntries;
        }
    }
}
