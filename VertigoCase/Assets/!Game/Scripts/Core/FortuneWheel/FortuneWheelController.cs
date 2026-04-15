using System.Collections.Generic;
using _Game.Scripts.Core.Data;
using _Game.Scripts.Core.Enums;
using _Game.Scripts.Event;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Core.FortuneWheel
{
    [RequireComponent(typeof(FortuneWheelView))]
    public class FortuneWheelController : MonoBehaviour, IFortuneWheelController
    {
        [SerializeField] private FortuneWheelView view;

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

            view.SpinButton.onClick.AddListener(OnSpinButtonClicked);
            EventBus.Subscribe<LevelSelectedEvent>(OnLevelSelected);

            HideWheel();
        }

        private void OnDestroy()
        {
            view.SpinButton.onClick.RemoveAllListeners();
            EventBus.Unsubscribe<LevelSelectedEvent>(OnLevelSelected);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            view = GetComponent<FortuneWheelView>();
        }
#endif

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

            view.CanvasGroup.alpha = 0f;
            view.CanvasGroup.gameObject.SetActive(true);
            view.CanvasGroup.DOFade(1f, 0.3f);
        }

        private void ApplyVisuals()
        {
            view.FrameImage.sprite = _currentConfig.frameSprite;
            view.IndicatorImage.sprite = _currentConfig.indicatorSprite;
        }

        private void PopulateSlots()
        {
            var slots = view.SlotViews;

            for (int i = 0; i < slots.Count; i++)
            {
                if (i >= _currentSlots.Count)
                {
                    slots[i].gameObject.SetActive(false);
                    continue;
                }

                slots[i].gameObject.SetActive(true);
                slots[i].Setup(_currentSlots[i]);
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
            view.SpinButton.interactable = false;
            EventBus.Publish(new WheelSpinStartedEvent());

            var winningIndex = PickWinningSlot();
            var targetAngle = CalculateTargetAngle(winningIndex);
            var targetRotation = new Vector3(0f, 0f, targetAngle);

            view.WheelTransform.rotation = Quaternion.identity;
            view.FrameImage.transform.rotation = Quaternion.identity;

            var sequence = DOTween.Sequence();
            sequence.Append(
                view.WheelTransform.DORotate(
                    targetRotation,
                    _currentConfig.spinDuration,
                    RotateMode.FastBeyond360
                ).SetEase(Ease.OutQuart)
            );
            sequence.Join(
                view.FrameImage.transform.DORotate(
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

            await view.CanvasGroup.DOFade(0f, 0.3f).AsyncWaitForCompletion();
            view.CanvasGroup.gameObject.SetActive(false);

            view.SpinButton.interactable = true;

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
                    Multiplier = reward.multiplier,
                    LevelType = _currentConfig.wheelType
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
            view.CanvasGroup.alpha = 0f;
            view.CanvasGroup.gameObject.SetActive(false);
        }
    }
}
