using System.Collections.Generic;
using _Game.Scripts.Core.Enums;
using _Game.Scripts.Event;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.CollectedRewardsPanel
{
    public class CollectedRewardsPanelController : MonoBehaviour, ICollectedRewardsPanelController
    {
        [Header("Grid")]
        [SerializeField] private RectTransform slotsParent;
        [SerializeField] private CollectedRewardSlotView slotPrefab;

        [Header("Flying Icon")]
        [SerializeField] private RectTransform flyingIconsParent;
        [SerializeField] private Image flyingIconPrefab;
        [SerializeField] private Canvas rootCanvas;

        [Header("Exit")]
        [SerializeField] private Button exitButton;
        [SerializeField] private CanvasGroup exitButtonCanvasGroup;
        [SerializeField] private float disabledAlpha = 0.35f;

        [Header("Animation")]
        [SerializeField] private float flyDuration = 0.6f;
        [SerializeField] private float flyingIconScale = 2f;
        [SerializeField] private Ease flyEase = Ease.InOutQuad;

        private readonly Dictionary<Sprite, CollectedRewardSlotView> _slots = new();

        public void Init()
        {
            ClearSlots();
            SetExitButtonState(false);

            exitButton.onClick.AddListener(OnExitClicked);
            EventBus.Subscribe<GiveUpEvent>(OnGiveUp);
            EventBus.Subscribe<ExitGameEvent>(OnExitGame);
            EventBus.Subscribe<LevelSelectedEvent>(OnLevelSelected);
            EventBus.Subscribe<WheelSpinStartedEvent>(OnWheelSpinStarted);
        }

        private void OnDestroy()
        {
            exitButton.onClick.RemoveAllListeners();
            EventBus.Unsubscribe<GiveUpEvent>(OnGiveUp);
            EventBus.Unsubscribe<ExitGameEvent>(OnExitGame);
            EventBus.Unsubscribe<LevelSelectedEvent>(OnLevelSelected);
            EventBus.Unsubscribe<WheelSpinStartedEvent>(OnWheelSpinStarted);
        }

        private void OnLevelSelected(LevelSelectedEvent e)
        {
            var isExitAllowed = e.LevelType == LevelType.SafeZone || e.LevelType == LevelType.SuperZone;
            SetExitButtonState(isExitAllowed);
        }

        private void SetExitButtonState(bool active)
        {
            exitButton.interactable = active;

            if (exitButtonCanvasGroup != null)
            {
                exitButtonCanvasGroup.DOKill();
                exitButtonCanvasGroup.DOFade(active ? 1f : disabledAlpha, 0.25f);
            }
        }

        private void OnExitClicked()
        {
            exitButton.interactable = false;
            EventBus.Publish(new ExitGameEvent());
        }

        private void OnWheelSpinStarted(WheelSpinStartedEvent e)
        {
            SetExitButtonState(false);
        }

        private void OnGiveUp(GiveUpEvent e)
        {
            ClearSlots();
            SetExitButtonState(false);
        }

        private void OnExitGame(ExitGameEvent e)
        {
            ClearSlots();
            SetExitButtonState(false);
        }

        private void ClearSlots()
        {
            foreach (Transform child in slotsParent)
            {
                child.DOKill();
                var cg = child.GetComponent<CanvasGroup>();
                if (cg != null) cg.DOKill();
                Destroy(child.gameObject);
            }

            _slots.Clear();
        }

        public void CollectReward(Sprite icon, int multiplier, Vector3 worldStartPosition)
        {
            var flyingIcon = Instantiate(flyingIconPrefab, flyingIconsParent);
            flyingIcon.sprite = icon;
            flyingIcon.rectTransform.localScale = Vector3.one * flyingIconScale;

            var startLocal = WorldToLocal(flyingIconsParent, worldStartPosition);
            flyingIcon.rectTransform.anchoredPosition = startLocal;

            var hasExisting = _slots.TryGetValue(icon, out var targetSlot);

            if (!hasExisting)
            {
                targetSlot = Instantiate(slotPrefab, slotsParent);
                targetSlot.Setup(icon, multiplier);
                _slots.Add(icon, targetSlot);
                Canvas.ForceUpdateCanvases();
            }

            var endLocal = WorldToLocal(flyingIconsParent, targetSlot.IconRect.position);

            var sequence = DOTween.Sequence();
            sequence.Append(flyingIcon.rectTransform.DOAnchorPos(endLocal, flyDuration).SetEase(flyEase));
            sequence.Join(flyingIcon.rectTransform.DOScale(Vector3.one * (flyingIconScale * 0.9f), flyDuration).SetEase(Ease.InOutSine));
            sequence.OnComplete(() =>
            {
                Destroy(flyingIcon.gameObject);

                if (hasExisting)
                    targetSlot.Add(multiplier);
                else
                    targetSlot.PlayAppear();
            });
        }

        private Vector2 WorldToLocal(RectTransform target, Vector3 worldPosition)
        {
            var cam = rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : rootCanvas.worldCamera;
            var screenPoint = RectTransformUtility.WorldToScreenPoint(cam, worldPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(target, screenPoint, cam, out var local);
            return local;
        }
    }
}
