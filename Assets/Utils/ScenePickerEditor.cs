using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

    /// <summary>
    /// Allow drag-drop of scenes into the event channel
    /// copied from unity scripting help
    /// https://docs.unity3d.com/ScriptReference/SceneAsset.html
    /// </summary>
    // [CustomEditor(typeof(LevelChanger), true)]
    /*
    public class ScenePickerEditor : Editor
    {
        SerializedProperty levelToLoad;
        SerializedProperty transitionTime;

        private void OnEnable()
        {
            levelToLoad = serializedObject.FindProperty("levelToLoad");
            transitionTime = serializedObject.FindProperty("transitionTime");
        }

        public override void OnInspectorGUI()
        {
            var levelChanger = target as LevelChanger; // type
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(levelChanger.levelToLoad); // coupled field

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            var newScene =
                EditorGUILayout.ObjectField("Level To Load", oldScene, typeof(SceneAsset), false) as SceneAsset;
            EditorGUILayout.FloatField("Transition Time", levelChanger.transitionTime);

            if (EditorGUI.EndChangeCheck())
            {
                var newPath = AssetDatabase.GetAssetPath(newScene);
                var scenePathProperty = serializedObject.FindProperty("levelToLoad"); // coupled field
                scenePathProperty.stringValue = newPath;
                levelChanger.transitionTime = transitionTime.floatValue;
            }

            serializedObject.ApplyModifiedProperties();
        }

    }
    */
