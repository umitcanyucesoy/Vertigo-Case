using _Game.Scripts.Core.Data;
using _Game.Scripts.Core.FortuneWheel;
using _Game.Scripts.Core.LevelSelector;
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

        private ILevelSelectorController _levelSelectorController;
        private IFortuneWheelController _fortuneWheelController;

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
        }

        private void InitSystems()
        {
            _fortuneWheelController.Init(wheelLevelData, wheelConfigs);
            _levelSelectorController.Init(levelSelectorData);
        }
    }
}
