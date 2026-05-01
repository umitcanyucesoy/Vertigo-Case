using System.Collections.Generic;
using _Game.Scripts.Core.Enums;
using _Game.Scripts.Core.ScriptableObjects.Config;
using _Game.Scripts.Core.ScriptableObjects.UIPanelData;
using _Game.Scripts.Event;
using UnityEngine;

namespace _Game.Scripts.Core.Economy
{
    public class EconomyPanelController : MonoBehaviour, IEconomyPanelController
    {
        [Header("Currency Views")] 
        [SerializeField] private EconomyPanelData panelData;
        [SerializeField] private List<CurrencyItemView> currencyViews = new(); 

        private IEconomyService _economyService;
        private EconomyPanelData _panelData;
        private readonly Dictionary<CurrencyType, CurrencyItemView> _viewMap = new();

        public void Init(IEconomyService economyService)
        {
            _economyService = economyService;
            _panelData = panelData;

            foreach (var view in currencyViews)
            {
                _viewMap[view.currencyType] = view;
                view.UpdateValue(_economyService.GetBalance(view.currencyType));
            }

            EventBus.Subscribe<CurrencyChangedEvent>(OnCurrencyChanged);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<CurrencyChangedEvent>(OnCurrencyChanged);
        }

        private void OnCurrencyChanged(CurrencyChangedEvent e)
        {
            if (_viewMap.TryGetValue(e.CurrencyType, out var view))
            {
                view.UpdateValue(e.NewBalance);
                view.PlayPunchAnimation(
                    _panelData.punchScale,
                    _panelData.punchDuration,
                    _panelData.punchVibrato,
                    _panelData.punchElasticity
                );
            }
        }
    }
}