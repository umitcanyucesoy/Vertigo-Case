using System.Collections.Generic;
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

        private List<LevelNodeData> _levels;
        private int _currentLevel;
        private int _totalLevels;

        public void Init(LevelSelectorData data)
        {
            _currentLevel = 1;
            _totalLevels = data.totalLevels;
            _levels = BuildRuntimeLevels(data);
            SpawnItems();
            ScrollToCurrentLevel(_totalLevels);
            SelectLevel(_currentLevel);

            EventBus.Subscribe<WheelRewardCollectedEvent>(OnRewardCollected);
            EventBus.Subscribe<GiveUpEvent>(OnGiveUp);
            EventBus.Subscribe<ReviveEvent>(OnRevive);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<WheelRewardCollectedEvent>(OnRewardCollected);
            EventBus.Unsubscribe<GiveUpEvent>(OnGiveUp);
            EventBus.Unsubscribe<ReviveEvent>(OnRevive);
        }

        private void OnRewardCollected(WheelRewardCollectedEvent e)
        {
            AdvanceLevel();
        }

        private void OnGiveUp(GiveUpEvent e)
        {
            ResetLevel();
        }

        private void OnRevive(ReviveEvent e)
        {
            AdvanceLevel();
        }

        private void ResetLevel()
        {
            _currentLevel = 1;
            RefreshLevelStates();
            SpawnItems();
            ScrollToCurrentLevel(_totalLevels);
            SelectLevel(_currentLevel);
        }

        private void AdvanceLevel()
        {
            if (_currentLevel >= _totalLevels) return;

            _currentLevel++;
            RefreshLevelStates();
            SpawnItems();
            ScrollToCurrentLevel(_totalLevels);
            SelectLevel(_currentLevel);
        }

        private void RefreshLevelStates()
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                var node = _levels[i];
                _levels[i] = new LevelNodeData
                {
                    levelNumber = node.levelNumber,
                    levelType = node.levelType,
                    levelState = ResolveState(node.levelNumber)
                };
            }
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

        private List<LevelNodeData> BuildRuntimeLevels(LevelSelectorData data)
        {
            var list = new List<LevelNodeData>(data.Levels.Count);

            foreach (var node in data.Levels)
            {
                list.Add(new LevelNodeData
                {
                    levelNumber = node.levelNumber,
                    levelType = node.levelType,
                    levelState = ResolveState(node.levelNumber)
                });
            }

            return list;
        }

        private LevelState ResolveState(int levelNumber)
        {
            if (levelNumber < _currentLevel) return LevelState.Completed;
            if (levelNumber == _currentLevel) return LevelState.Current;
            return LevelState.Locked;
        }

        private void SpawnItems()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);

            foreach (var data in _levels)
            {
                var item = Instantiate(levelItemPrefab, content);
                item.Setup(data);
            }
        }

        private void ScrollToCurrentLevel(int totalLevels)
        {
            var targetPos = (float)(_currentLevel - 1) / (totalLevels - 1);

            DOTween.To(
                () => scrollRect.horizontalNormalizedPosition,
                x => scrollRect.horizontalNormalizedPosition = x,
                targetPos,
                0.5f
            ).SetEase(Ease.OutCubic);
        }
    }
}
