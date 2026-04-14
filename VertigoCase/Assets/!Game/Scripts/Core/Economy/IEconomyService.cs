using _Game.Scripts.Service;

namespace _Game.Scripts.Core.Economy
{
    public interface IEconomyService : IService
    {
        int Gold { get; }
        bool CanAfford(int amount);
        bool TrySpend(int amount);
        void Add(int amount);
    }
}
