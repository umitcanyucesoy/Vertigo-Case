using _Game.Scripts.Core.Data;
using _Game.Scripts.Core.Enums;
using _Game.Scripts.Event;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.LevelSelector
{
    public class LevelSelectorController : MonoBehaviour, ILevelSelectorController
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Transform content;
        [SerializeField] private LevelItemView levelItemPrefab;

        private LevelNodeData[] _levels;
        private int _currentLevel;

        public void Init(LevelSelectorData data)
        {
            _currentLevel = 1;
            _levels = BuildLevels(data);
            SpawnItems();
            ScrollToCurrentLevel(data.totalLevels);
        }

        public int GetCurrentLevel() => _currentLevel;

        public void SelectLevel(int levelNumber)
        {
            var data = _levels[levelNumber - 1];

            if (data.levelState == LevelState.Locked)
                return;

            EventBus.Publish(new LevelSelectedEvent
            {
                LevelNumber = data.levelNumber,
                LevelType = data.levelType
            });
        }

        private void SpawnItems()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);

            foreach (var data in _levels)
            {
                var item = Instantiate(levelItemPrefab, content);
                item.Setup(data, this);
            }
        }

        private void ScrollToCurrentLevel(int totalLevels)
        {
            float targetPos = (float)(_currentLevel - 1) / (totalLevels - 1);

            DOTween.To(
                () => scrollRect.horizontalNormalizedPosition,
                x => scrollRect.horizontalNormalizedPosition = x,
                targetPos,
                0.5f
            ).SetEase(Ease.OutCubic);
        }

        private LevelNodeData[] BuildLevels(LevelSelectorData data)
        {
            var levels = new LevelNodeData[data.totalLevels];

            for (int i = 0; i < data.totalLevels; i++)
            {
                int number = i + 1;
                levels[i] = new LevelNodeData
                {
                    levelNumber = number,
                    levelType = ResolveType(number, data),
                    levelState = ResolveState(number)
                };
            }

            return levels;
        }

        private LevelType ResolveType(int levelNumber, LevelSelectorData data)
        {
            if (levelNumber == data.superZoneLevel)
                return LevelType.SuperZone;

            if (levelNumber % data.safeZoneInterval == 0)
                return LevelType.SafeZone;

            return LevelType.Normal;
        }

        private LevelState ResolveState(int levelNumber)
        {
            if (levelNumber < _currentLevel) return LevelState.Completed;
            if (levelNumber == _currentLevel) return LevelState.Current;
            return LevelState.Locked;
        }
    }
}
