using System;
using System.Collections.Generic;
using _Game.Scripts.Core.Enums;
using UnityEngine;

namespace _Game.Scripts.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "LevelSelectorData", menuName = "GameData/LevelSelectorData")]
    public class LevelZoneData : ScriptableObject
    {
        [Header("Level Settings")]
        public int totalLevels = 30;
        public int safeZoneInterval = 5;
        public int superZoneLevel = 30;
        
        [SerializeField] private List<LevelNodeData> levels = new();
        public IReadOnlyList<LevelNodeData> Levels => levels;

        [Header("Scroll Animation")]
        public float scrollDuration = 0.5f;

        public LevelType ResolveType(int levelNumber)
        {
            if (levelNumber == superZoneLevel) return LevelType.SuperZone;
            if (levelNumber % safeZoneInterval == 0) return LevelType.SafeZone;
            return LevelType.Normal;
        }
    }
    
    [Serializable]
    public struct LevelNodeData
    {
        public int levelNumber;
        public LevelType levelType;
        public LevelState levelState;
    }
}
