using _Game.Scripts.Core.Enums;

namespace _Game.Scripts.Event
{
    public struct LevelSelectedEvent : IEvent
    {
        public int LevelNumber;
        public LevelType LevelType;
    }

    public struct WheelRewardCollectedEvent : IEvent
    {
        public int LevelNumber;
        public RewardType RewardType;
        public int Amount;
        public int Multiplier;
    }
}
