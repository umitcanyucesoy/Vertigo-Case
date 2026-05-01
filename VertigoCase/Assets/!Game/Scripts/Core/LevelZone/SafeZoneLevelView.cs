using _Game.Scripts.Core.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.LevelSelector
{
    public class SafeZoneLevelView : LevelItemViewBase
    {
        [Header("Safe Zone UI")]
        [SerializeField] private Image safeZoneImage;

        protected override void ApplySpecificVisuals()
        {
            var isCurrent = Data.levelState == LevelState.Current;
            safeZoneImage.gameObject.SetActive(isCurrent);
        }
    }
}