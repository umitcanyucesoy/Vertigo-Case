using System.Collections.Generic;
using _Game.Scripts.Core.Enums;
using _Game.Scripts.Core.ScriptableObjects.Data;
using _Game.Scripts.Event;

namespace _Game.Scripts.Core.Economy
{
    public class EconomyService : IEconomyService
    {
        private readonly Dictionary<CurrencyType, int> _balances = new();
        private readonly Dictionary<RewardType, CurrencyType> _rewardMappings = new();
        private readonly Dictionary<RewardType, int> _pendingRewards = new();

        public EconomyService(EconomyData data)
        {
            foreach (var balance in data.startingBalances)
                _balances[balance.currencyType] = balance.amount;

            foreach (var map in data.rewardMappings)
                _rewardMappings[map.rewardType] = map.currencyType;

            EventBus.Subscribe<WheelRewardCollectedEvent>(OnRewardCollected);
            EventBus.Subscribe<ExitGameEvent>(OnExitGame);
            EventBus.Subscribe<GiveUpEvent>(OnGiveUp);
        }

        ~EconomyService()
        {
            EventBus.Unsubscribe<WheelRewardCollectedEvent>(OnRewardCollected);
            EventBus.Unsubscribe<ExitGameEvent>(OnExitGame);
            EventBus.Unsubscribe<GiveUpEvent>(OnGiveUp);
        }

        private void OnGiveUp(GiveUpEvent e) => _pendingRewards.Clear();
        public int GetBalance(CurrencyType currencyType) => _balances.GetValueOrDefault(currencyType, 0);
        public bool CanAfford(CurrencyType currencyType, int amount) => amount >= 0 && GetBalance(currencyType) >= amount;
        
        public bool TrySpend(CurrencyType currencyType, int amount)
        {
            if (!CanAfford(currencyType, amount))
                return false;
                
            _balances[currencyType] -= amount;

            EventBus.Publish(new CurrencyChangedEvent
            {
                CurrencyType = currencyType,
                NewBalance = _balances[currencyType],
                Delta = -amount
            });

            return true;
        }

        private void OnRewardCollected(WheelRewardCollectedEvent e)
        {
            if (!_pendingRewards.TryAdd(e.RewardType, e.Multiplier))
                _pendingRewards[e.RewardType] += e.Multiplier;
        }

        private void OnExitGame(ExitGameEvent e)
        {
            foreach (var pendingReward in _pendingRewards)
                ProcessReward(pendingReward.Key, pendingReward.Value);
            
            _pendingRewards.Clear();
        }


        public void ProcessReward(RewardType rewardType, int amount)
        {
            if (_rewardMappings.TryGetValue(rewardType, out var currencyType))
                Add(currencyType, amount);
        }

        public void Add(CurrencyType currencyType, int amount)
        {
            if (amount <= 0) return;
            
            if (!_balances.TryAdd(currencyType, amount))
                _balances[currencyType] += amount;
                
            EventBus.Publish(new CurrencyChangedEvent
            {
                CurrencyType = currencyType,
                NewBalance = _balances[currencyType],
                Delta = amount
            });
        }
    }
}