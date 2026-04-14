using _Game.Scripts.Core.Economy;

namespace _Game.Scripts.Core.DeathPanel
{
    public interface IDeathPanelController
    {
        void Init(IEconomyService economyService, EconomyConfig economyConfig);
    }
}
