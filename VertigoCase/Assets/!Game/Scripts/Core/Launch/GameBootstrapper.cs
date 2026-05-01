using _Game.Scripts.Core.CollectedRewardsPanel;
using _Game.Scripts.Core.DeathPanel;
using _Game.Scripts.Core.Economy;
using _Game.Scripts.Core.FortuneWheel;
using _Game.Scripts.Core.LevelSelector;
using _Game.Scripts.Core.RewardPanel;
using _Game.Scripts.Core.ScriptableObjects;
using _Game.Scripts.Core.ScriptableObjects.Config;
using _Game.Scripts.Core.ScriptableObjects.Data;
using _Game.Scripts.Core.ScriptableObjects.UIPanelData;
using _Game.Scripts.Service;
using UnityEngine;

namespace _Game.Scripts.Core.Launch
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private LevelZoneData levelZoneData;
        [SerializeField] private WheelLevelData wheelLevelData;
        [SerializeField] private WheelConfigData[] wheelConfigs;
        [SerializeField] private EconomyData economyData;
        
        [Header("Controller")]
        [SerializeField] private LevelZoneController levelZoneController;
        [SerializeField] private FortuneWheelController fortuneWheelController;
        [SerializeField] private RewardPanelController rewardPanelController;
        [SerializeField] private CollectedRewardsPanelController collectedRewardsPanelController;
        [SerializeField] private DeathPanelController deathPanelController;
        [SerializeField] private EconomyPanelController economyPanelController;

        private ILevelZoneController _levelZoneController;
        private IFortuneWheelController _fortuneWheelController;
        private IRewardPanelController _rewardPanelController;
        private ICollectedRewardsPanelController _collectedRewardsPanelController;
        private IDeathPanelController _deathPanelController;
        private IEconomyPanelController _economyPanelController;
        private IEconomyService _economyService;

        private void Awake() => Register();
        private void Start() => InitSystems();
        
        private void OnDestroy() => ServiceLocator.Unregister<IEconomyService>();
        
        private void Register()
        {
            _economyService = new EconomyService(economyData);
            ServiceLocator.Register(_economyService);
            
            _levelZoneController = levelZoneController;
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
            _deathPanelController.Init(_economyService, economyData);
            _levelZoneController.Init(levelZoneData);
        }
    }
}
