using _Game.Scripts.Core.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.LevelSelector
{
    public class NormalLevelView : LevelItemViewBase
    {
        [Header("Normal Zone UI")]
        [SerializeField] private Image normalImage;

        protected override void ApplySpecificVisuals()
        {
            var isCurrent = Data.levelState == LevelState.Current;
            normalImage.gameObject.SetActive(isCurrent);
        }
    }
}