using _Game.Scripts.Core.Data;
using _Game.Scripts.Core.Enums;

namespace _Game.Scripts.Core.FortuneWheel
{
    public interface IFortuneWheelController
    {
        void Init(WheelLevelData levelData, WheelConfigData[] configs);
        void ShowWheel(int levelNumber, LevelType levelType);
    }
}
