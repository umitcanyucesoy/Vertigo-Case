// Core/LevelSelector/LevelSelectorController.cs
using System;
using System.Collections.Generic;
using _Game.Scripts.Core.Enums;
using _Game.Scripts.Core.ScriptableObjects.Data;
using _Game.Scripts.Event;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Core.LevelSelector
{
    [Serializable]
    public struct LevelPrefabMapping
    {
        public LevelType levelType;
        public LevelItemViewBase prefab;
    }

    public class LevelZoneController : MonoBehaviour, ILevelZoneController
    {
        [Header("UI References")]
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Transform content;

        [Header("Level Zone Mappings")]
        [SerializeField] private List<LevelPrefabMapping> prefabMappings = new(); 

        private readonly Dictionary<LevelType, LevelItemViewBase> _prefabMap = new();
        private LevelZoneData _data;
        private List<LevelNodeData> _levels;
        private int _currentLevel;
        private int _totalLevels;

        public void Init(LevelZoneData data)
        {
            _data = data;
            _currentLevel = 1;
            _totalLevels = data.totalLevels;
            _levels = BuildRuntimeLevels(data);

            foreach (var mapping in prefabMappings)
                _prefabMap[mapping.levelType] = mapping.prefab;

            SpawnItems();
            ScrollToCurrentLevel(_totalLevels);
            SelectLevel(_currentLevel);

            EventBus.Subscribe<WheelRewardCollectedEvent>(OnRewardCollected);
            EventBus.Subscribe<GiveUpEvent>(OnGiveUp);
            EventBus.Subscribe<ReviveEvent>(OnRevive);
            EventBus.Subscribe<ExitGameEvent>(OnExitGame);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<WheelRewardCollectedEvent>(OnRewardCollected);
            EventBus.Unsubscribe<GiveUpEvent>(OnGiveUp);
            EventBus.Unsubscribe<ReviveEvent>(OnRevive);
            EventBus.Unsubscribe<ExitGameEvent>(OnExitGame);
        }

        private void OnRewardCollected(WheelRewardCollectedEvent e) => AdvanceLevel();
        private void OnGiveUp(GiveUpEvent e) => ResetLevel();
        private void OnRevive(ReviveEvent e) => AdvanceLevel();
        private void OnExitGame(ExitGameEvent e) => ResetLevel();
        public int GetCurrentLevel() => _currentLevel;

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

        private List<LevelNodeData> BuildRuntimeLevels(LevelZoneData data)
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
                if (!_prefabMap.TryGetValue(data.levelType, out var prefab))
                {
                    Debug.LogError($"[LevelSelector] Prefab missing for LevelType: {data.levelType}");
                    continue;
                }

                var item = Instantiate(prefab, content);
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
                _data.scrollDuration
            ).SetEase(Ease.OutCubic);
        }
    }
}