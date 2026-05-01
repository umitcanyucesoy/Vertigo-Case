using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Core.ScriptableObjects.Config
{
    [CreateAssetMenu(fileName = "CollectedRewardsPanelConfig", menuName = "GameData/CollectedRewardsPanelConfig")]
    public class CollectedRewardsPanelData : ScriptableObject
    {
        [Header("Exit Button")]
        public float exitButtonFadeDuration = 0.25f;
        public float disabledAlpha = 0.35f;

        [Header("Flying Icon")]
        public float flyDuration = 0.6f;
        public float flyingIconScale = 2f;
        public float flyingIconEndScaleMultiplier = 0.9f;
        public Ease flyEase = Ease.InOutQuad;

        [Header("Slot - Appear")]
        public float slotInitialScale = 0.6f;
        public float slotAppearFadeDuration = 0.25f;
        public float slotAppearScaleDuration = 0.35f;
        public Ease slotAppearEase = Ease.OutBack;

        [Header("Slot - Punch")]
        public float slotPunchScale = 0.25f;
        public float slotPunchDuration = 0.3f;
        public int slotPunchVibrato = 6;
        public float slotPunchElasticity = 0.6f;
    }
}
