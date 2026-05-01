using UnityEngine;

namespace _Game.Scripts.Core.ScriptableObjects.UIPanelData
{
    [CreateAssetMenu(fileName = "DeathPanelConfig", menuName = "GameData/DeathPanelConfig")]
    public class DeathPanelData : ScriptableObject
    {
        [Header("Fade")]
        public float fadeInDuration = 0.3f;
        public float fadeOutDuration = 0.3f;

        [Header("Shake")]
        public float shakeDuration = 0.35f;
        public float shakeStrength = 12f;
        public int shakeVibrato = 20;
    }
}
