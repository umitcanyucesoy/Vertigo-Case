using _Game.Scripts.Event;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.DeathPanel
{
    public class DeathPanelController : MonoBehaviour, IDeathPanelController
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button giveUpButton;

        public void Init()
        {
            giveUpButton.onClick.AddListener(OnGiveUpClicked);
            EventBus.Subscribe<ShowDeathPanelEvent>(OnShowDeath);
            Hide();
        }

        private void OnDestroy()
        {
            giveUpButton.onClick.RemoveAllListeners();
            EventBus.Unsubscribe<ShowDeathPanelEvent>(OnShowDeath);
        }

        private void OnShowDeath(ShowDeathPanelEvent e)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.DOFade(1f, 0.3f);
        }

        private void OnGiveUpClicked()
        {
            canvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
            {
                canvasGroup.gameObject.SetActive(false);
                EventBus.Publish(new GiveUpEvent());
            });
        }

        private void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(false);
        }
    }
}

