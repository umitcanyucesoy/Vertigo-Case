using System;
using _Game.Scripts.Core.Enums;
using UnityEngine;

namespace _Game.Scripts.Core.Data
{
    [CreateAssetMenu(fileName = "LevelSelectorData", menuName = "GameData/LevelSelectorData")]
    public class LevelSelectorData : ScriptableObject
    {
        public int totalLevels  = 30;
        public int safeZoneInterval = 5;
        public int superZoneLevel = 30;
    }
}
