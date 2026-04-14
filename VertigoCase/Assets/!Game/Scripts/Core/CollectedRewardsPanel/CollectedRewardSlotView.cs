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

        public Sprite Sprite { get; private set; }
        public int Total { get; private set; }
        public RectTransform IconRect => icon.rectTransform;

        public void Setup(Sprite sprite, int multiplier)
        {
            Sprite = sprite;
            icon.sprite = sprite;
            Total = multiplier;
            RefreshText();

            canvasGroup.alpha = 0f;
            transform.localScale = Vector3.one * 0.6f;
        }

        public void PlayAppear()
        {
            canvasGroup.DOFade(1f, 0.25f);
            transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutBack);
        }

        public void Add(int multiplier)
        {
            Total += multiplier;
            RefreshText();
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOPunchScale(Vector3.one * 0.25f, 0.3f, 6, 0.6f);
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
