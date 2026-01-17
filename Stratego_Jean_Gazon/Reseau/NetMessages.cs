using System;
using System.Drawing;

namespace Stratego_Jean_Gazon.Reseau
{
    public enum NetMessageType
    {
        Hello = 1,
        MoveRequest = 2,
        MoveResult = 3,
        Error = 4
    }

    public sealed class NetEnvelope
    {
        public NetMessageType Type { get; set; }
        public string JsonPayload { get; set; }
    }

    public sealed class HelloMessage
    {
        public string Nom { get; set; }
    }

    public sealed class MoveRequest
    {
        public int FromX { get; set; }
        public int FromY { get; set; }
        public int ToX { get; set; }
        public int ToY { get; set; }
    }

    public sealed class MoveResult
    {
        public int FromX { get; set; }
        public int FromY { get; set; }
        public int ToX { get; set; }
        public int ToY { get; set; }

        public bool IsCombat { get; set; }
        public byte Victoire { get; set; } // 1,2,3,4 comme ton moteur

        public bool RemoveAttacker { get; set; }
        public bool RemoveDefender { get; set; }
        public bool MoveAttackerToDestination { get; set; }

        public string AttackerGrade { get; set; }
        public bool AttackerCouleur { get; set; }

        public string DefenderGrade { get; set; }
        public bool DefenderCouleur { get; set; }

        public bool IsEndGame { get; set; }
        public Player Winner { get; set; }
    }
}