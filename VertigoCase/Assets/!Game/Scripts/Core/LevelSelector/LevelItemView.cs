using _Game.Scripts.Core.Data;
using _Game.Scripts.Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.LevelSelector
{
    public class LevelItemView : MonoBehaviour
    {
        [SerializeField] private LevelItemViewData viewData;
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private Image levelImage;
        [SerializeField] private GameObject borderFrame;

        private LevelNodeData _data;

        public void Setup(LevelNodeData data)
        {
            _data = data;
            levelNumberText.text = data.levelNumber.ToString();
            ApplyVisuals();
        }

        private void ApplyVisuals()
        {
            var isCurrent = _data.levelState == LevelState.Current;

            levelImage.gameObject.SetActive(isCurrent);
            if (isCurrent) levelImage.sprite = ResolveCurrentSprite();
            borderFrame.SetActive(isCurrent);
        }

        private Sprite ResolveCurrentSprite()
        {
            return _data.levelType switch
            {
                LevelType.SafeZone => viewData.safeZoneSprite,
                LevelType.SuperZone => viewData.superZoneSprite,
                _ => viewData.currentSprite
            };
        }
    }
}
