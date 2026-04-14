using _Game.Scripts.Core.Economy;
using _Game.Scripts.Event;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Core.DeathPanel
{
    [RequireComponent(typeof(DeathPanelView))]
    public class DeathPanelController : MonoBehaviour, IDeathPanelController
    {
        [SerializeField] private DeathPanelView view;

        private IEconomyService _economyService;
        private EconomyConfig _economyConfig;
        private int _pendingLevelNumber;

        public void Init(IEconomyService economyService, EconomyConfig economyConfig)
        {
            _economyService = economyService;
            _economyConfig = economyConfig;

            view.GiveUpButton.onClick.AddListener(OnGiveUpClicked);
            view.SaveButton.onClick.AddListener(OnSaveClicked);
            EventBus.Subscribe<ShowDeathPanelEvent>(OnShowDeath);
            Hide();
        }

        private void OnDestroy()
        {
            view.GiveUpButton.onClick.RemoveAllListeners();
            view.SaveButton.onClick.RemoveAllListeners();
            EventBus.Unsubscribe<ShowDeathPanelEvent>(OnShowDeath);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            view = GetComponent<DeathPanelView>();
        }
#endif

        private void OnShowDeath(ShowDeathPanelEvent e)
        {
            _pendingLevelNumber = e.LevelNumber;
            RefreshSaveState();

            view.CanvasGroup.alpha = 0f;
            view.CanvasGroup.gameObject.SetActive(true);
            view.CanvasGroup.DOFade(1f, 0.3f);
        }

        private void OnGiveUpClicked()
        {
            view.CanvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
            {
                view.CanvasGroup.gameObject.SetActive(false);
                EventBus.Publish(new GiveUpEvent());
            });
        }

        private void OnSaveClicked()
        {
            var cost = _economyConfig.ReviveCost;

            if (!_economyService.TrySpend(cost))
            {
                view.SaveButton.transform.DOKill();
                view.SaveButton.transform.DOShakePosition(0.35f, 12f, 20);
                return;
            }

            var levelNumber = _pendingLevelNumber;

            view.CanvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
            {
                view.CanvasGroup.gameObject.SetActive(false);
                EventBus.Publish(new ReviveEvent { LevelNumber = levelNumber });
            });
        }

        private void RefreshSaveState()
        {
            var cost = _economyConfig.ReviveCost;

            if (view.SaveCostText != null)
                view.SaveCostText.text = cost.ToString();

            view.SaveButton.interactable = _economyService.CanAfford(cost);
        }

        private void Hide()
        {
            view.CanvasGroup.alpha = 0f;
            view.CanvasGroup.gameObject.SetActive(false);
        }
    }
}
