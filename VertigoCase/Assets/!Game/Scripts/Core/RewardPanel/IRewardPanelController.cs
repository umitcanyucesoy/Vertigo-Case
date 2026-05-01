using _Game.Scripts.Core.CollectedRewardsPanel;
using _Game.Scripts.Core.ScriptableObjects;
using _Game.Scripts.Core.ScriptableObjects.Config;
using _Game.Scripts.Core.ScriptableObjects.UIPanelData;

namespace _Game.Scripts.Core.RewardPanel
{
    public interface IRewardPanelController
    {
        void Init(ICollectedRewardsPanelController collectedRewardsPanel);
    }
}
