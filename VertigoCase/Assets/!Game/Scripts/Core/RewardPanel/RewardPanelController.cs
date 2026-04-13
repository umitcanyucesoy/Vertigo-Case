using _Game.Scripts.Event;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.RewardPanel
{
    public class RewardPanelController : MonoBehaviour, IRewardPanelController
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TextMeshProUGUI multiplierText;
        [SerializeField] private Button collectButton;

        private ShowRewardPanelEvent _pendingReward;

        public void Init()
        {
            collectButton.onClick.AddListener(OnCollectClicked);
            EventBus.Subscribe<ShowRewardPanelEvent>(OnShowReward);
            Hide();
        }

        private void OnDestroy()
        {
            collectButton.onClick.RemoveAllListeners();
            EventBus.Unsubscribe<ShowRewardPanelEvent>(OnShowReward);
        }

        private void OnShowReward(ShowRewardPanelEvent e)
        {
            _pendingReward = e;

            rewardIcon.sprite = e.Icon;

            multiplierText.text = e.Multiplier > 1 ? $"x{e.Multiplier}" : $"{e.Amount}";

            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.DOFade(1f, 0.3f);
        }

        private void OnCollectClicked()
        {
            canvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
            {
                canvasGroup.gameObject.SetActive(false);

                EventBus.Publish(new WheelRewardCollectedEvent
                {
                    LevelNumber = _pendingReward.LevelNumber,
                    RewardType = _pendingReward.RewardType,
                    Amount = _pendingReward.Amount,
                    Multiplier = _pendingReward.Multiplier
                });
            });
        }

        private void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(false);
        }
    }
}

