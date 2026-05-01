using UnityEngine;

namespace _Game.Scripts.Core.ScriptableObjects.UIPanelData
{
    [CreateAssetMenu(fileName = "EconomyPanelConfig", menuName = "GameData/EconomyPanelConfig")]
    public class EconomyPanelData : ScriptableObject
    {
        [Header("Punch Animation")]
        public float punchScale = 0.2f;
        public float punchDuration = 0.25f;
        public int punchVibrato = 6;
        public float punchElasticity = 0.6f;
    }
}
