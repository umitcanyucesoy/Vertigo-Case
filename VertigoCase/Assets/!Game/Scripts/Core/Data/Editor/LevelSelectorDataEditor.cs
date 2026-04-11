using _Game.Scripts.Core.Enums;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts.Core.Data.Editor
{
    [CustomEditor(typeof(LevelSelectorData))]
    public class LevelSelectorDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);

            var data = (LevelSelectorData)target;

            if (GUILayout.Button("Generate Levels", GUILayout.Height(36)))
            {
                GenerateLevels(data);
            }
        }

        private void GenerateLevels(LevelSelectorData data)
        {
            var levelsProp = serializedObject.FindProperty("levels");
            levelsProp.ClearArray();

            for (int i = 0; i < data.totalLevels; i++)
            {
                int number = i + 1;
                levelsProp.InsertArrayElementAtIndex(i);
                var element = levelsProp.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("levelNumber").intValue = number;
                element.FindPropertyRelative("levelType").enumValueIndex = (int)data.ResolveType(number);
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(data);

            GenerateWheelEntries(data);

            AssetDatabase.SaveAssets();
            Debug.Log($"[LevelSelectorData] Generated {data.totalLevels} levels + wheel entries.");
        }

        private void GenerateWheelEntries(LevelSelectorData data)
        {
            if (!data.wheelLevelData)
            {
                Debug.LogError("[LevelSelectorData] WheelLevelData reference is not assigned!");
                return;
            }

            var wheelData = data.wheelLevelData;
            var wheelSo = new SerializedObject(wheelData);
            var wheelLevelsProp = wheelSo.FindProperty("levels");

            wheelLevelsProp.ClearArray();

            for (int i = 0; i < data.totalLevels; i++)
            {
                int number = i + 1;
                LevelType type = data.ResolveType(number);

                wheelLevelsProp.InsertArrayElementAtIndex(i);
                var entry = wheelLevelsProp.GetArrayElementAtIndex(i);
                entry.FindPropertyRelative("levelNumber").intValue = number;
                entry.FindPropertyRelative("wheelType").enumValueIndex = (int)type;
                entry.FindPropertyRelative("slots").ClearArray();
            }

            wheelSo.ApplyModifiedProperties();
            EditorUtility.SetDirty(wheelData);
        }
    }
}

