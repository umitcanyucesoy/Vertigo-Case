using UnityEditor;
using UnityEngine;

namespace _Game.Scripts.Core.Data.Editor
{
    [CustomEditor(typeof(LevelSelectorData))]
    public class LevelSelectorDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            EditorGUILayout.Space(12);

            var data = (LevelSelectorData)target;

            GUI.enabled = data.totalLevels > 0;
            if (GUILayout.Button("Generate Levels", GUILayout.Height(36)))
            {
                Generate(data);
            }
            GUI.enabled = true;
        }

        private void Generate(LevelSelectorData data)
        {
            Undo.RecordObject(data, "Generate Levels");

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
            AssetDatabase.SaveAssets();
            Debug.Log($"[LevelSelectorData] Generated {data.totalLevels} levels.");
        }
    }
}

