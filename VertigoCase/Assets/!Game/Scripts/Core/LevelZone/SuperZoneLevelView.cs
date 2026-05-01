using _Game.Scripts.Core.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.LevelSelector
{
    public class SuperZoneLevelView : LevelItemViewBase
    {
        [Header("Super Zone UI")]
        [SerializeField] private Image superZoneImage;

        protected override void ApplySpecificVisuals()
        {
            var isCurrent = Data.levelState == LevelState.Current;
            superZoneImage.gameObject.SetActive(isCurrent);
        }
    }
}