using UnityEngine;

namespace _Game.Scripts.Core.CollectedRewardsPanel
{
    public interface ICollectedRewardsPanelController
    {
        void Init();
        void CollectReward(Sprite icon, int multiplier, Vector3 worldStartPosition);
    }
}
