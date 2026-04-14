using _Game.Scripts.Core.Enums;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts.Core.Data.Editor
{
    [CustomEditor(typeof(WheelLevelData))]
    public class WheelLevelDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            EditorGUILayout.Space(16);

            var data = (WheelLevelData)target;

            if (GUILayout.Button("Fill Normal Zones", GUILayout.Height(36)))
            {
                FillZone(data, LevelType.Normal, data.normalZoneSlots);
            }

            if (GUILayout.Button("Fill Safe Zones", GUILayout.Height(36)))
            {
                FillZone(data, LevelType.SafeZone, data.safeZoneSlots);
            }

            if (GUILayout.Button("Fill Super Zones", GUILayout.Height(36)))
            {
                FillZone(data, LevelType.SuperZone, data.superZoneSlots);
            }
        }

        private void FillZone(WheelLevelData data, LevelType targetType, ZoneSlotTemplate[] templates)
        {
            if (templates == null || templates.Length == 0)
            {
                Debug.LogError($"[WheelLevelData] {targetType} slot templates are empty!");
                return;
            }

            Undo.RecordObject(data, $"Fill {targetType} Zones");

            var levelsProp = serializedObject.FindProperty("levels");
            int filledCount = 0;

            for (int i = 0; i < levelsProp.arraySize; i++)
            {
                var entry = levelsProp.GetArrayElementAtIndex(i);
                var wheelType = (LevelType)entry.FindPropertyRelative("wheelType").enumValueIndex;

                if (wheelType != targetType) continue;

                var slotsProp = entry.FindPropertyRelative("slots");
                slotsProp.ClearArray();

                for (int s = 0; s < templates.Length; s++)
                {
                    slotsProp.InsertArrayElementAtIndex(s);
                    var slot = slotsProp.GetArrayElementAtIndex(s);
                    slot.FindPropertyRelative("rewardType").enumValueIndex = (int)templates[s].rewardType;
                    slot.FindPropertyRelative("icon").objectReferenceValue = templates[s].icon;
                    slot.FindPropertyRelative("multiplier").intValue = 0;
                }

                filledCount++;
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();

            Debug.Log($"[WheelLevelData] Filled {filledCount} {targetType} entries with {templates.Length} slots each.");
        }
    }
}

