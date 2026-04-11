using _Game.Scripts.Core.Enums;
using UnityEngine;

namespace _Game.Scripts.Core.Data
{
    [CreateAssetMenu(fileName = "WheelConfigData", menuName = "GameData/WheelConfigData")]
    public class WheelConfigData : ScriptableObject
    {
        public LevelType wheelType;
        public Sprite frameSprite;
        public Sprite indicatorSprite;

        [Header("Spin Settings")]
        [Range(1f, 10f)] public float spinDuration = 4f;
        [Range(2, 8)] public int minFullRotations = 3;
    }
}
