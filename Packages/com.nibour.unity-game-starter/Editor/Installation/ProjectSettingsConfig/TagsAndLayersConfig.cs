using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityGameStarter.EditorUtilities.Installation
{
    [InitializeOnLoad]
    static class TagsAndLayersConfig
    {
        private const string _tags = "tags";
        private const string _layers = "layers";
        private const string _sortingLayers = "m_SortingLayers";
        private const string _renderingLayers = "m_RenderingLayers";

        static TagsAndLayersConfig()
        {
            ProjectSettingsInitializer.BindToInitializers(Initialize);
        }

        private static string GetTemplatePath(UnityEditor.PackageManager.PackageInfo info)
            => $"{info.assetPath}/Templates/ProjectSettings/TagManager.asset";

        private static void Initialize(UnityEditor.PackageManager.PackageInfo info) 
        {
            const string targetPath = "ProjectSettings/TagManager.asset";

            var templateAssets = AssetDatabase.LoadAllAssetsAtPath(GetTemplatePath(info));
            var targetAssets = AssetDatabase.LoadAllAssetsAtPath(targetPath);

            if (templateAssets.Length == 0 || targetAssets.Length == 0) return;

            var template = new SerializedObject(templateAssets[0]);
            var target = new SerializedObject(targetAssets[0]);

            ApplyTags(template.FindProperty(_tags), target.FindProperty(_tags));
            ApplyLayers(template.FindProperty(_layers), target.FindProperty(_layers));
            ApplySortingLayers(template.FindProperty(_sortingLayers), target.FindProperty(_sortingLayers));
            ApplyRenderingLayers(template.FindProperty(_renderingLayers), target.FindProperty(_renderingLayers));

            target.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();

            Debug.Log("[Unity Game Starter] Tag Manager Updated");
        }

        private static void ApplyTags(SerializedProperty source, SerializedProperty target)
        {
            var existing = new HashSet<string>();

            for (int i = 0; i < target.arraySize; i++)
                existing.Add(target.GetArrayElementAtIndex(i).stringValue);

            for (int i = 0; i < source.arraySize; i++)
            {
                string tag = source.GetArrayElementAtIndex(i).stringValue;

                if (string.IsNullOrWhiteSpace(tag) || !existing.Add(tag)) continue;

                int index = target.arraySize;
                target.InsertArrayElementAtIndex(index);
                target.GetArrayElementAtIndex(index).stringValue = tag;
            }
        }

        private static void ApplyLayers(SerializedProperty source, SerializedProperty target)
        {
            for (int i = 0; i < source.arraySize && i < target.arraySize; i++) 
            {
                string layer = source.GetArrayElementAtIndex(i).stringValue;

                if (string.IsNullOrWhiteSpace(layer)) continue;

                var targetLayer = target.GetArrayElementAtIndex(i);

                if (string.IsNullOrWhiteSpace(targetLayer.stringValue))
                    targetLayer.stringValue = layer;
            }
        }

        private static void ApplySortingLayers(SerializedProperty source, SerializedProperty target)
        {
            var existingNames = new HashSet<string>();
            var existingIds = new HashSet<int>();

            for (int i = 0; i < target.arraySize; i++)
            {
                var element = target.GetArrayElementAtIndex(i);

                existingNames.Add(element.FindPropertyRelative("name").stringValue);
                existingIds.Add(element.FindPropertyRelative("uniqueID").intValue);
            }

            for (int i = 0; i < source.arraySize; i++)
            {
                var element = source.GetArrayElementAtIndex(i);

                string name = element.FindPropertyRelative("name").stringValue;
                int id = element.FindPropertyRelative("uniqueID").intValue;
                bool locked = element.FindPropertyRelative("locked").boolValue;

                if (string.IsNullOrWhiteSpace(name) || existingNames.Contains(name)) continue;

                if (id == 0 || existingIds.Contains(id))
                {
                    Debug.LogWarning($"Unity Game Starter: Sorting Layer ID conflict: {name} ({id}).");
                    continue;
                }

                int index = target.arraySize;
                target.InsertArrayElementAtIndex(index);

                var newElement = target.GetArrayElementAtIndex(index);

                newElement.FindPropertyRelative("name").stringValue = name;
                newElement.FindPropertyRelative("uniqueID").intValue = id;
                newElement.FindPropertyRelative("locked").boolValue = locked;

                existingNames.Add(name);
                existingIds.Add(id);
            }
        }

        private static void ApplyRenderingLayers(SerializedProperty source, SerializedProperty target)
        {
            for (int i = 1; i < source.arraySize; i++)
            {
                string layer = source.GetArrayElementAtIndex(i).stringValue;

                if (string.IsNullOrWhiteSpace(layer)) continue;

                if (i >= target.arraySize)
                    target.arraySize = i + 1;

                var targetLayer = target.GetArrayElementAtIndex(i);

                if (string.IsNullOrWhiteSpace(targetLayer.stringValue))
                    targetLayer.stringValue = layer;
                else if (targetLayer.stringValue != layer)
                {
                    Debug.LogWarning($"Unity Game Starter: Rendering Layer conflict at index {i}. " +
                        $"Existing: {targetLayer.stringValue}, Template: {layer}");
                }
            }
        }
    }
}
