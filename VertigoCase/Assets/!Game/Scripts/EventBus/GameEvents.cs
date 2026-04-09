using _Game.Scripts.Core.Enums;

namespace _Game.Scripts.Event
{
    public struct LevelSelectedEvent : IEvent
    {
        public int LevelNumber;
        public LevelType LevelType;
    }
}
