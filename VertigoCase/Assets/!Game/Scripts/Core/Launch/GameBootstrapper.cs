using _Game.Scripts.Core.CollectedRewardsPanel;
using _Game.Scripts.Core.Data;
using _Game.Scripts.Core.DeathPanel;
using _Game.Scripts.Core.Economy;
using _Game.Scripts.Core.FortuneWheel;
using _Game.Scripts.Core.LevelSelector;
using _Game.Scripts.Core.RewardPanel;
using _Game.Scripts.Service;
using UnityEngine;

namespace _Game.Scripts.Core.Launch
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private LevelSelectorData levelSelectorData;
        [SerializeField] private WheelLevelData wheelLevelData;
        [SerializeField] private WheelConfigData[] wheelConfigs;
        [SerializeField] private EconomyConfig economyConfig;

        [Header("Controller")]
        [SerializeField] private LevelSelectorController levelSelectorController;
        [SerializeField] private FortuneWheelController fortuneWheelController;
        [SerializeField] private RewardPanelController rewardPanelController;
        [SerializeField] private CollectedRewardsPanelController collectedRewardsPanelController;
        [SerializeField] private DeathPanelController deathPanelController;
        [SerializeField] private EconomyPanelController economyPanelController;

        private ILevelSelectorController _levelSelectorController;
        private IFortuneWheelController _fortuneWheelController;
        private IRewardPanelController _rewardPanelController;
        private ICollectedRewardsPanelController _collectedRewardsPanelController;
        private IDeathPanelController _deathPanelController;
        private IEconomyPanelController _economyPanelController;
        private IEconomyService _economyService;

        private void Awake()
        {
            RegisterServices();
            Register();
        }

        private void Start()
        {
            InitSystems();
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<IEconomyService>();
        }

        private void RegisterServices()
        {
            _economyService = new EconomyService(economyConfig);
            ServiceLocator.Register<IEconomyService>(_economyService);
        }

        private void Register()
        {
            _levelSelectorController = levelSelectorController;
            _fortuneWheelController = fortuneWheelController;
            _rewardPanelController = rewardPanelController;
            _collectedRewardsPanelController = collectedRewardsPanelController;
            _deathPanelController = deathPanelController;
            _economyPanelController = economyPanelController;
        }

        private void InitSystems()
        {
            _economyPanelController.Init(_economyService);
            _fortuneWheelController.Init(wheelLevelData, wheelConfigs);
            _collectedRewardsPanelController.Init();
            _rewardPanelController.Init(_collectedRewardsPanelController);
            _deathPanelController.Init(_economyService, economyConfig);
            _levelSelectorController.Init(levelSelectorData);
        }
    }
}
