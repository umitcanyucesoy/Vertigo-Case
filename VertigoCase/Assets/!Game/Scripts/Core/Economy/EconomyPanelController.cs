using _Game.Scripts.Event;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.Economy
{
    public class EconomyPanelController : MonoBehaviour, IEconomyPanelController
    {
        [SerializeField] private Image goldIcon;
        [SerializeField] private TextMeshProUGUI goldText;

        private IEconomyService _economyService;

        public void Init(IEconomyService economyService)
        {
            _economyService = economyService;
            Refresh(_economyService.Gold);

            EventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<GoldChangedEvent>(OnGoldChanged);
        }

        private void OnGoldChanged(GoldChangedEvent e)
        {
            Refresh(e.NewBalance);
            goldText.transform.DOKill();
            goldText.transform.localScale = Vector3.one;
            goldText.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f, 6, 0.6f);
        }

        private void Refresh(int amount)
        {
            goldText.text = amount.ToString();
        }
    }
}
