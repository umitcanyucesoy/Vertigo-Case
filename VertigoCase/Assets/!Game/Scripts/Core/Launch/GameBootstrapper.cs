using _Game.Scripts.Core.Data;
using _Game.Scripts.Core.LevelSelector;
using UnityEngine;

namespace _Game.Scripts.Core.Launch
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private LevelSelectorData levelSelectorData;
        
        [Header("Controller")]
        [SerializeField] private LevelSelectorController levelSelectorController;
        
        private ILevelSelectorController _levelSelectorController;

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
        }

        private void InitSystems()
        {
            _levelSelectorController.Init(levelSelectorData);
        }
    }
}
