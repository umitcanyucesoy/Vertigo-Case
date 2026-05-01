using _Game.Scripts.Core.ScriptableObjects.Config;
using _Game.Scripts.Core.ScriptableObjects.UIPanelData;

namespace _Game.Scripts.Core.Economy
{
    public interface IEconomyPanelController
    {
        public void Init(IEconomyService economyService);
    }
}
