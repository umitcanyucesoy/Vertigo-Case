using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.FortuneWheel
{
    public class FortuneWheelView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Transform wheelTransform;
        [SerializeField] private Image frameImage;
        [SerializeField] private Image indicatorImage;
        [SerializeField] private Button spinButton;
        [SerializeField] private List<WheelSlotView> slotViews;

        public CanvasGroup CanvasGroup => canvasGroup;
        public Transform WheelTransform => wheelTransform;
        public Image FrameImage => frameImage;
        public Image IndicatorImage => indicatorImage;
        public Button SpinButton => spinButton;
        public IReadOnlyList<WheelSlotView> SlotViews => slotViews;
    }
}
