using _Game.Scripts.Core.Enums;
using _Game.Scripts.Core.ScriptableObjects.Data;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Core.LevelSelector
{
    public abstract class LevelItemViewBase : MonoBehaviour
    {
        [Header("Base UI References")]
        [SerializeField] public TextMeshProUGUI levelNumberText;
        [SerializeField] public GameObject borderFrame;

        protected LevelNodeData Data { get; private set; }

        public void Setup(LevelNodeData data)
        {
            Data = data;
            levelNumberText.text = data.levelNumber.ToString();
            borderFrame.SetActive(data.levelState == LevelState.Current);

            ApplySpecificVisuals();
        }

        protected abstract void ApplySpecificVisuals();
    }
}