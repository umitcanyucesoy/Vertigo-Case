using _Game.Scripts.Core.Enums;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.Economy
{
    // burada herhangi bir abstract yapi kullanmadim if switch mantigi donmuyor ocp ihlali yok sadece paneldeki currency'lere update yapiyorum
    // bu sebeple aciklamak istedim.
    public class CurrencyItemView : MonoBehaviour
    {
        public CurrencyType currencyType;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI text;

        public void UpdateValue(int amount) => text.text = amount.ToString();

        public void PlayPunchAnimation(float scale, float duration, int vibrato, float elasticity)
        {
            text.transform.DOKill();
            text.transform.localScale = Vector3.one;
            text.transform.DOPunchScale(
                Vector3.one * scale, 
                duration, 
                vibrato, 
                elasticity
            );
        }
    }
}