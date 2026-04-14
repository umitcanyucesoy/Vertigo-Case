using _Game.Scripts.Event;

namespace _Game.Scripts.Core.Economy
{
    public class EconomyService : IEconomyService
    {
        public int Gold { get; private set; }

        public EconomyService(EconomyConfig config)
        {
            Gold = config.StartingGold;
        }

        public bool CanAfford(int amount) => amount >= 0 && Gold >= amount;

        public bool TrySpend(int amount)
        {
            if (!CanAfford(amount))
                return false;

            Gold -= amount;
            EventBus.Publish(new GoldChangedEvent { NewBalance = Gold, Delta = -amount });
            return true;
        }

        public void Add(int amount)
        {
            if (amount <= 0) return;

            Gold += amount;
            EventBus.Publish(new GoldChangedEvent { NewBalance = Gold, Delta = amount });
        }
    }
}
