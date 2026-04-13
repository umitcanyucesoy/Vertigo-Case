using _Game.Scripts.Core.Data;
using _Game.Scripts.Core.DeathPanel;
using _Game.Scripts.Core.FortuneWheel;
using _Game.Scripts.Core.LevelSelector;
using _Game.Scripts.Core.RewardPanel;
using UnityEngine;

namespace _Game.Scripts.Core.Launch
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private LevelSelectorData levelSelectorData;
        [SerializeField] private WheelLevelData wheelLevelData;
        [SerializeField] private WheelConfigData[] wheelConfigs;

        [Header("Controller")]
        [SerializeField] private LevelSelectorController levelSelectorController;
        [SerializeField] private FortuneWheelController fortuneWheelController;
        [SerializeField] private RewardPanelController rewardPanelController;
        [SerializeField] private DeathPanelController deathPanelController;

        private ILevelSelectorController _levelSelectorController;
        private IFortuneWheelController _fortuneWheelController;
        private IRewardPanelController _rewardPanelController;
        private IDeathPanelController _deathPanelController;

        private void Awake()
        {
            Register();
        }

        private void Start()
        {
            InitSystems();
        }

        private void Register()
        {
            _levelSelectorController = levelSelectorController;
            _fortuneWheelController = fortuneWheelController;
            _rewardPanelController = rewardPanelController;
            _deathPanelController = deathPanelController;
        }

        private void InitSystems()
        {
            _fortuneWheelController.Init(wheelLevelData, wheelConfigs);
            _rewardPanelController.Init();
            _deathPanelController.Init();
            _levelSelectorController.Init(levelSelectorData);
        }
    }
}
