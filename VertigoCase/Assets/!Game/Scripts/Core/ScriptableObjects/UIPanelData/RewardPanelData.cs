using UnityEngine;

namespace _Game.Scripts.Core.ScriptableObjects.UIPanelData
{
    [CreateAssetMenu(fileName = "RewardPanelConfig", menuName = "GameData/RewardPanelConfig")]
    public class RewardPanelData : ScriptableObject
    {
        [Header("Fade")]
        public float fadeInDuration = 0.3f;
        public float fadeOutDuration = 0.3f;
    }
}
