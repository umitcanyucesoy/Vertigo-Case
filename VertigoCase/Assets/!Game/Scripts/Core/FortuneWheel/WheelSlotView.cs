using _Game.Scripts.Core.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.FortuneWheel
{
    public class WheelSlotView : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;

        public void Setup(WheelSlotData data)
        {
            iconImage.sprite = data.icon;
            amountText.text = data.multiplier <= 0 ? string.Empty : $"x{data.multiplier}";
        }
    }
}
