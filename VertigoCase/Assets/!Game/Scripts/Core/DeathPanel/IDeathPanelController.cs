using _Game.Scripts.Core.Economy;
using _Game.Scripts.Core.ScriptableObjects.Config;
using _Game.Scripts.Core.ScriptableObjects.Data;
using _Game.Scripts.Core.ScriptableObjects.UIPanelData;

namespace _Game.Scripts.Core.DeathPanel
{
    public interface IDeathPanelController
    {
        public void Init(IEconomyService economyService, EconomyData economyData);
    }
}
