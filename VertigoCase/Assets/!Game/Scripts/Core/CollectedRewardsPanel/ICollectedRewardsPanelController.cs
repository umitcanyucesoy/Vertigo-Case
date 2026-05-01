using _Game.Scripts.Core.ScriptableObjects.Config;
using UnityEngine;

namespace _Game.Scripts.Core.CollectedRewardsPanel
{
    public interface ICollectedRewardsPanelController
    {
        public void Init();
        public void CollectReward(Sprite icon, int multiplier, Vector3 worldStartPosition);
    }
}
