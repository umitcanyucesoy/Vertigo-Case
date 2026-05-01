using System;
using System.Collections.Generic;
using _Game.Scripts.Core.Enums;
using UnityEngine;

namespace _Game.Scripts.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "EconomyConfig", menuName = "GameData/EconomyConfig")]
    public class EconomyData : ScriptableObject
    {
        [Header("Starting Balances")]
        public List<CurrencyBalance> startingBalances = new();

        [Header("Revive Settings")]
        public CurrencyType reviveCurrency = CurrencyType.Gold;
        public int reviveCost = 150;

        [Header("Reward to Currency Mappings")]
        public List<RewardCurrencyMapping> rewardMappings = new();
    }

    [Serializable]
    public struct CurrencyBalance
    {
        public CurrencyType currencyType;
        public int amount;
    }

    [Serializable]
    public struct RewardCurrencyMapping
    {
        public RewardType rewardType;
        public CurrencyType currencyType;
    }
}