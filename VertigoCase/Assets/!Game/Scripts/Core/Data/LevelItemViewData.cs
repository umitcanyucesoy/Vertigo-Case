using UnityEngine;

namespace _Game.Scripts.Core.Data
{
    [CreateAssetMenu(fileName = "LevelItemViewData", menuName = "GameData/LevelItemViewData")]
    public class LevelItemViewData : ScriptableObject
    {
        public Sprite currentSprite;
        public Sprite safeZoneSprite;
        public Sprite superZoneSprite;
    }
}
