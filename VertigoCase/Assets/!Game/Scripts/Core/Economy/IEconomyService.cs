using _Game.Scripts.Core.Enums;
using _Game.Scripts.Service;

namespace _Game.Scripts.Core.Economy
{
    public interface IEconomyService : IService
    {
        public int GetBalance(CurrencyType currencyType);
        public bool CanAfford(CurrencyType currencyType, int amount);
        public bool TrySpend(CurrencyType currencyType, int amount);
    }
}