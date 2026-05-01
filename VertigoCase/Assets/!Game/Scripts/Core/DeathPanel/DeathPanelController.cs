using _Game.Scripts.Core.Economy;
using _Game.Scripts.Core.ScriptableObjects.Config;
using _Game.Scripts.Core.ScriptableObjects.Data;
using _Game.Scripts.Core.ScriptableObjects.UIPanelData;
using _Game.Scripts.Event;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Core.DeathPanel
{
    [RequireComponent(typeof(DeathPanelView))]
    public class DeathPanelController : MonoBehaviour, IDeathPanelController
    {
        [SerializeField] private DeathPanelView view;
        [SerializeField] private DeathPanelData panelData;
        
        private IEconomyService _economyService;
        private EconomyData _economyData;
        private int _pendingLevelNumber;

        public void Init(IEconomyService economyService, EconomyData economyData)
        {
            _economyService = economyService;
            _economyData = economyData;
            
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
            view.CanvasGroup.DOFade(1f, panelData.fadeInDuration);
        }

        private void OnGiveUpClicked()
        {
            view.CanvasGroup.DOFade(0f, panelData.fadeOutDuration).OnComplete(() =>
            {
                view.CanvasGroup.gameObject.SetActive(false);
                EventBus.Publish(new GiveUpEvent());
            });
        }

        private void OnSaveClicked()
        {
            var cost = _economyData.reviveCost;
            var currency = _economyData.reviveCurrency;

            if (!_economyService.TrySpend(currency, cost))
            {
                view.SaveButton.transform.DOKill();
                view.SaveButton.transform.DOShakePosition(
                    panelData.shakeDuration,
                    panelData.shakeStrength,
                    panelData.shakeVibrato
                );
                return;
            }

            var levelNumber = _pendingLevelNumber;
            view.CanvasGroup.DOFade(0f, panelData.fadeOutDuration).OnComplete(() =>
            {
                view.CanvasGroup.gameObject.SetActive(false);
                EventBus.Publish(new ReviveEvent { LevelNumber = levelNumber });
            });
        }

        private void RefreshSaveState()
        {
            var cost = _economyData.reviveCost;
            var currency = _economyData.reviveCurrency;
            
            view.SaveCostText.text = cost.ToString();
            view.SaveButton.interactable = _economyService.CanAfford(currency, cost);
        }

        private void Hide()
        {
            view.CanvasGroup.alpha = 0f;
            view.CanvasGroup.gameObject.SetActive(false);
        }
    }
}