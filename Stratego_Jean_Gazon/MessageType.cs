csharp Network/NetworkMessage.cs
using System;

namespace Stratego_Jean_Gazon.Network
{
    public enum MessageType
    {
        Move,
        PlacementDone,
        SyncState
    }

    public class MoveMessage
    {
        public int FromX { get; set; }
        public int FromY { get; set; }
        public int ToX { get; set; }
        public int ToY { get; set; }
        public bool IsBlue { get; set; }
    }

    // Envelope used for transport (serialized)
    internal class JsonEnvelope
    {
        public string Type { get; set; }
        public string PayloadJson { get; set; }
    }
}