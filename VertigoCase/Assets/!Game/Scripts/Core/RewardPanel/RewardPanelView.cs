using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.RewardPanel
{
    public class RewardPanelView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TextMeshProUGUI multiplierText;
        [SerializeField] private Button collectButton;

        public CanvasGroup CanvasGroup => canvasGroup;
        public Image RewardIcon => rewardIcon;
        public TextMeshProUGUI MultiplierText => multiplierText;
        public Button CollectButton => collectButton;
    }
}
