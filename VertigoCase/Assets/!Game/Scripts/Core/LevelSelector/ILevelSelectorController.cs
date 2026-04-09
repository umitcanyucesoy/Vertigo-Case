using _Game.Scripts.Core.Data;

namespace _Game.Scripts.Core.LevelSelector
{
    public interface ILevelSelectorController
    {
        public void Init(LevelSelectorData data);
        public int GetCurrentLevel();
        public void SelectLevel(int levelNumber);
    }
}
