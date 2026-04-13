using _Game.Scripts.Core.Enums;
using UnityEngine;

namespace _Game.Scripts.Event
{
    public struct LevelSelectedEvent : IEvent
    {
        public int LevelNumber;
        public LevelType LevelType;
    }

    public struct ShowRewardPanelEvent : IEvent
    {
        public int LevelNumber;
        public RewardType RewardType;
        public Sprite Icon;
        public int Amount;
        public int Multiplier;
    }

    public struct ShowDeathPanelEvent : IEvent
    {
        public int LevelNumber;
    }

    public struct WheelRewardCollectedEvent : IEvent
    {
        public int LevelNumber;
        public RewardType RewardType;
        public int Amount;
        public int Multiplier;
    }

    public struct GiveUpEvent : IEvent { }
}
