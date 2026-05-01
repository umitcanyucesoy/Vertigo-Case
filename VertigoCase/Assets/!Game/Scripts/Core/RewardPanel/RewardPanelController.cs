using _Game.Scripts.Core.CollectedRewardsPanel;
using _Game.Scripts.Core.ScriptableObjects;
using _Game.Scripts.Core.ScriptableObjects.Config;
using _Game.Scripts.Core.ScriptableObjects.UIPanelData;
using _Game.Scripts.Event;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Core.RewardPanel
{
    [RequireComponent(typeof(RewardPanelView))]
    public class RewardPanelController : MonoBehaviour, IRewardPanelController
    {
        [SerializeField] private RewardPanelData panelData;
        [SerializeField] private RewardPanelView view;

        private ICollectedRewardsPanelController _collectedRewardsPanel;
        private ShowRewardPanelEvent _pendingReward;

        public void Init(ICollectedRewardsPanelController collectedRewardsPanel)
        {
            _collectedRewardsPanel = collectedRewardsPanel;
            view.CollectButton.onClick.AddListener(OnCollectClicked);
            EventBus.Subscribe<ShowRewardPanelEvent>(OnShowReward);
            Hide();
        }

        private void OnDestroy()
        {
            view.CollectButton.onClick.RemoveAllListeners();
            EventBus.Unsubscribe<ShowRewardPanelEvent>(OnShowReward);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            view = GetComponent<RewardPanelView>();
        }
#endif

        private void OnShowReward(ShowRewardPanelEvent e)
        {
            _pendingReward = e;

            view.RewardIcon.sprite = e.Icon;
            view.MultiplierText.text = e.Multiplier <= 0 ? string.Empty : $"x{e.Multiplier}";

            view.CanvasGroup.alpha = 0f;
            view.CanvasGroup.gameObject.SetActive(true);
            view.CanvasGroup.DOFade(1f, panelData.fadeInDuration);
        }

        private void OnCollectClicked()
        {
            view.CollectButton.interactable = false;

            var iconWorldPos = view.RewardIcon.rectTransform.position;
            var icon = _pendingReward.Icon;
            var multiplier = _pendingReward.Multiplier;
            var collectedEvent = new WheelRewardCollectedEvent
            {
                LevelNumber = _pendingReward.LevelNumber,
                RewardType = _pendingReward.RewardType,
                Multiplier = _pendingReward.Multiplier
            };

            view.CanvasGroup.DOFade(0f, panelData.fadeOutDuration).OnComplete(() =>
            {
                view.CanvasGroup.gameObject.SetActive(false);
                view.CollectButton.interactable = true;

                _collectedRewardsPanel.CollectReward(icon, multiplier, iconWorldPos);

                EventBus.Publish(collectedEvent);
            });
        }

        private void Hide()
        {
            view.CanvasGroup.alpha = 0f;
            view.CanvasGroup.gameObject.SetActive(false);
        }
    }
}
