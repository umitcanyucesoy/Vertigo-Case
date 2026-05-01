using _Game.Scripts.Core.Enums;
using _Game.Scripts.Core.ScriptableObjects.Config;
using _Game.Scripts.Core.ScriptableObjects.Data;
using _Game.Scripts.Core.ScriptableObjects.UIPanelData;

namespace _Game.Scripts.Core.FortuneWheel
{
    public interface IFortuneWheelController
    {
        void Init(WheelLevelData levelData, WheelConfigData[] configs);
        void ShowWheel(int levelNumber, LevelType levelType);
    }
}
