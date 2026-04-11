using System;
using System.Collections.Generic;
using _Game.Scripts.Core.Enums;
using UnityEngine;

namespace _Game.Scripts.Core.Data
{
    [CreateAssetMenu(fileName = "WheelLevelData", menuName = "GameData/WheelLevelData")]
    public class WheelLevelData : ScriptableObject
    {
        public List<WheelLevelEntry> levels = new();

        public WheelLevelEntry GetEntryForLevel(int levelNumber)
        {
            foreach (var entry in levels)
            {
                if (entry.levelNumber == levelNumber)
                    return entry;
            }

            return levels[^1];
        }
    }

    [Serializable]
    public struct WheelLevelEntry
    {
        public int levelNumber;
        public LevelType wheelType;
        public List<WheelSlotData> slots;
    }
}
