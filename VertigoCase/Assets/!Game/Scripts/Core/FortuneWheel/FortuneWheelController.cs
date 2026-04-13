using System.Collections.Generic;
using _Game.Scripts.Core.Data;
using _Game.Scripts.Core.Enums;
using _Game.Scripts.Event;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.FortuneWheel
{
    public class FortuneWheelController : MonoBehaviour, IFortuneWheelController
    {
        [SerializeField] private Transform wheelTransform;
        [SerializeField] private Image frameImage;
        [SerializeField] private Image indicatorImage;
        [SerializeField] private Button spinButton;
        [SerializeField] private List<WheelSlotView> slotViews;
        [SerializeField] private CanvasGroup canvasGroup;

        private WheelLevelData _levelData;
        private Dictionary<LevelType, WheelConfigData> _configs;
        private WheelConfigData _currentConfig;
        private List<WheelSlotData> _currentSlots;
        private int _currentLevelNumber;
        private bool _isSpinning;

        public void Init(WheelLevelData levelData, WheelConfigData[] configs)
        {
            _levelData = levelData;
            _configs = new Dictionary<LevelType, WheelConfigData>();

            foreach (var config in configs) _configs[config.wheelType] = config;

            spinButton.onClick.AddListener(OnSpinButtonClicked);
            EventBus.Subscribe<LevelSelectedEvent>(OnLevelSelected);

            HideWheel();
        }

        private void OnDestroy()
        {
            spinButton.onClick.RemoveAllListeners();
            EventBus.Unsubscribe<LevelSelectedEvent>(OnLevelSelected);
        }
        
        private void OnLevelSelected(LevelSelectedEvent handle) => ShowWheel(handle.LevelNumber, handle.LevelType);
        
        public void ShowWheel(int levelNumber, LevelType levelType)
        {
            _currentLevelNumber = levelNumber;

            if (!_configs.TryGetValue(levelType, out _currentConfig))
                _currentConfig = _configs[LevelType.Normal];

            var entry = _levelData.GetEntryForLevel(levelNumber);
            _currentSlots = entry.slots;

            ApplyVisuals();
            PopulateSlots();

            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.DOFade(1f, 0.3f);
        }

        private void ApplyVisuals()
        {
            frameImage.sprite = _currentConfig.frameSprite;
            indicatorImage.sprite = _currentConfig.indicatorSprite;
        }

        private void PopulateSlots()
        {
            for (int i = 0; i < slotViews.Count; i++)
            {
                if (i >= _currentSlots.Count)
                {
                    slotViews[i].gameObject.SetActive(false);
                    continue;
                }

                slotViews[i].gameObject.SetActive(true);
                slotViews[i].Setup(_currentSlots[i]);
            }
        }

        private void OnSpinButtonClicked()
        {
            if (_isSpinning) return;
            SpinAsync().Forget();
        }

        private async UniTaskVoid SpinAsync()
        {
            _isSpinning = true;
            spinButton.interactable = false;

            var winningIndex = PickWinningSlot();
            var targetAngle = CalculateTargetAngle(winningIndex);
            var targetRotation = new Vector3(0f, 0f, targetAngle);

            wheelTransform.rotation = Quaternion.identity;
            frameImage.transform.rotation = Quaternion.identity;

            var sequence = DOTween.Sequence();
            sequence.Append(
                wheelTransform.DORotate(
                    targetRotation,
                    _currentConfig.spinDuration,
                    RotateMode.FastBeyond360
                ).SetEase(Ease.OutQuart)
            );
            sequence.Join(
                frameImage.transform.DORotate(
                    targetRotation,
                    _currentConfig.spinDuration,
                    RotateMode.FastBeyond360
                ).SetEase(Ease.OutQuart)
            );

            await sequence.AsyncWaitForCompletion();

            await OnSpinCompleteAsync(winningIndex);
        }

        private async UniTask OnSpinCompleteAsync(int slotIndex)
        {
            _isSpinning = false;

            var reward = _currentSlots[slotIndex];

            await UniTask.Delay(500);

            await canvasGroup.DOFade(0f, 0.3f).AsyncWaitForCompletion();
            canvasGroup.gameObject.SetActive(false);

            spinButton.interactable = true;

            if (reward.rewardType == RewardType.Death)
            {
                EventBus.Publish(new ShowDeathPanelEvent
                {
                    LevelNumber = _currentLevelNumber
                });
            }
            else
            {
                EventBus.Publish(new ShowRewardPanelEvent
                {
                    LevelNumber = _currentLevelNumber,
                    RewardType = reward.rewardType,
                    Icon = reward.icon,
                    Amount = reward.amount,
                    Multiplier = reward.multiplier
                });
            }
        }

        private int PickWinningSlot() => Random.Range(0, _currentSlots.Count);

        private float CalculateTargetAngle(int slotIndex)
        {
            const float slotAngle = 45f;
            int fullRotations = _currentConfig.minFullRotations + Random.Range(0, 3);
            return fullRotations * 360f - slotIndex * slotAngle;
        }

        private void HideWheel()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(false);
        }
    }
}
