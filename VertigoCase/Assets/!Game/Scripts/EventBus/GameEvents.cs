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
        public int Multiplier;
        public LevelType LevelType;
    }

    public struct ShowDeathPanelEvent : IEvent
    {
        public int LevelNumber;
    }

    public struct WheelRewardCollectedEvent : IEvent
    {
        public int LevelNumber;
        public RewardType RewardType;
        public int Multiplier;
    }

    public struct GiveUpEvent : IEvent { }

    public struct ReviveEvent : IEvent
    {
        public int LevelNumber;
    }

    public struct CurrencyChangedEvent : IEvent
    {
        public CurrencyType CurrencyType;
        public int NewBalance;
        public int Delta;
    }

    public struct ExitGameEvent : IEvent { }

    public struct WheelSpinStartedEvent : IEvent { }
}
