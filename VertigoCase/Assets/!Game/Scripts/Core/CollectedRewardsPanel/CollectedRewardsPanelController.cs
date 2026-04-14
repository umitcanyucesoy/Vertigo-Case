using System.Collections.Generic;
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

        [Header("Animation")]
        [SerializeField] private float flyDuration = 0.6f;
        [SerializeField] private float flyingIconScale = 2f;
        [SerializeField] private Ease flyEase = Ease.InOutQuad;

        private readonly Dictionary<Sprite, CollectedRewardSlotView> _slots = new();

        public void Init()
        {
            ClearSlots();
            EventBus.Subscribe<GiveUpEvent>(OnGiveUp);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<GiveUpEvent>(OnGiveUp);
        }

        private void OnGiveUp(GiveUpEvent e)
        {
            ClearSlots();
        }

        private void ClearSlots()
        {
            foreach (Transform child in slotsParent)
                Destroy(child.gameObject);

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
