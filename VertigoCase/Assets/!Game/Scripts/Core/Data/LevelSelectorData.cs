using System.Collections.Generic;
using _Game.Scripts.Core.Enums;
using UnityEngine;

namespace _Game.Scripts.Core.Data
{
    [CreateAssetMenu(fileName = "LevelSelectorData", menuName = "GameData/LevelSelectorData")]
    public class LevelSelectorData : ScriptableObject
    {
        [Header("Level Settings")]
        public int totalLevels = 30;
        public int safeZoneInterval = 5;
        public int superZoneLevel = 30;

        [Header("Wheel Data Reference")]
        public WheelLevelData wheelLevelData;

        [SerializeField] private List<LevelNodeData> levels = new();
        public IReadOnlyList<LevelNodeData> Levels => levels;

        public LevelType ResolveType(int levelNumber)
        {
            if (levelNumber == superZoneLevel) return LevelType.SuperZone;
            if (levelNumber % safeZoneInterval == 0) return LevelType.SafeZone;
            return LevelType.Normal;
        }
    }
}
