using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.DeathPanel
{
    public class DeathPanelView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private TextMeshProUGUI saveCostText;

        public CanvasGroup CanvasGroup => canvasGroup;
        public Button GiveUpButton => giveUpButton;
        public Button SaveButton => saveButton;
        public TextMeshProUGUI SaveCostText => saveCostText;
    }
}
