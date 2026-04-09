using _Game.Scripts.Core.Data;
using _Game.Scripts.Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.LevelSelector
{
    public class LevelItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelNumberText;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private GameObject _currentIndicator;
        [SerializeField] private GameObject _lockIcon;

        [Header("Colors")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _safeZoneColor = Color.green;
        [SerializeField] private Color _superZoneColor = Color.yellow;
        [SerializeField] private Color _lockedColor = Color.gray;

        private ILevelSelectorController _controller;
        private LevelNodeData _data;

        public void Setup(LevelNodeData data, ILevelSelectorController controller)
        {
            _data = data;
            _controller = controller;

            _levelNumberText.text = data.levelNumber.ToString();

            ApplyVisuals();
        }

        public void OnClick()
        {
            _controller?.SelectLevel(_data.levelNumber);
        }

        private void ApplyVisuals()
        {
            bool isLocked = _data.levelState == LevelState.Locked;
            bool isCurrent = _data.levelState == LevelState.Current;

            if (_lockIcon != null)
                _lockIcon.SetActive(isLocked);

            if (_currentIndicator != null)
                _currentIndicator.SetActive(isCurrent);

            if (_backgroundImage != null)
                _backgroundImage.color = ResolveColor(isLocked);
        }

        private Color ResolveColor(bool isLocked)
        {
            if (isLocked)
                return _lockedColor;

            return _data.levelType switch
            {
                LevelType.SafeZone => _safeZoneColor,
                LevelType.SuperZone => _superZoneColor,
                _ => _normalColor
            };
        }
    }
}
