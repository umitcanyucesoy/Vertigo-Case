using _Game.Scripts.Core.ScriptableObjects.Config;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.CollectedRewardsPanel
{
    public class CollectedRewardSlotView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI multiplierText;
        [SerializeField] private CanvasGroup canvasGroup;

        private CollectedRewardsPanelData _config;

        public Sprite Sprite { get; private set; }
        public int Total { get; private set; }
        public RectTransform IconRect => icon.rectTransform;

        public void Init(CollectedRewardsPanelData config)
        {
            _config = config;
        }

        public void Setup(Sprite sprite, int multiplier)
        {
            Sprite = sprite;
            icon.sprite = sprite;
            Total = multiplier;
            RefreshText();

            canvasGroup.alpha = 0f;
            transform.localScale = Vector3.one * _config.slotInitialScale;
        }

        public void PlayAppear()
        {
            canvasGroup.DOFade(1f, _config.slotAppearFadeDuration);
            transform.DOScale(Vector3.one, _config.slotAppearScaleDuration).SetEase(_config.slotAppearEase);
        }

        public void Add(int multiplier)
        {
            Total += multiplier;
            RefreshText();
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOPunchScale(
                Vector3.one * _config.slotPunchScale,
                _config.slotPunchDuration,
                _config.slotPunchVibrato,
                _config.slotPunchElasticity
            );
        }

        private void OnDestroy()
        {
            transform.DOKill();
            canvasGroup.DOKill();
        }

        private void RefreshText()
        {
            if (Total <= 0)
            {
                multiplierText.gameObject.SetActive(false);
                return;
            }

            multiplierText.gameObject.SetActive(true);
            multiplierText.text = $"x{Total}";
        }
    }
}
