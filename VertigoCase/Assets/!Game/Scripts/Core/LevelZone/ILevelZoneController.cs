using _Game.Scripts.Core.ScriptableObjects.Data;

namespace _Game.Scripts.Core.LevelSelector
{
    public interface ILevelZoneController
    {
        public void Init(LevelZoneData data);
        public int GetCurrentLevel();
        public void SelectLevel(int levelNumber);
    }
}
