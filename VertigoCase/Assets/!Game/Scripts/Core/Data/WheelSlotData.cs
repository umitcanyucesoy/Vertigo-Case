using System;
using _Game.Scripts.Core.Enums;
using UnityEngine;

namespace _Game.Scripts.Core.Data
{
    [Serializable]
    public struct WheelSlotData
    {
        public RewardType rewardType;
        public Sprite icon;
        public int multiplier;
    }
}
