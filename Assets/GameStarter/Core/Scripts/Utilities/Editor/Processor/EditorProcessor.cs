using System;
using System.Linq;
using System.Reflection;

using UnityEditor;
using UnityEngine;

using UnityGameStarter.StarterAttributes;

namespace UnityGameStarter.EditorUtilities.EditorProcessor 
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class EditorProcessAttribute : OrderedAttribute 
    {
        public EditorProcessAttribute(int order = 0) : base(order) { }
    }
    
    [InitializeOnLoad]
    static class EditorProcessor
    {
        private readonly static MethodInfo[] _processors;

        private static bool _isProcessing = false;
        private static bool _scheduled = false;

        static EditorProcessor()
        {
            _processors = TypeCache.GetMethodsWithAttribute<EditorProcessAttribute>().OrderBy(GetOrder).ToArray();
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private static int GetOrder(MethodInfo method)
        {
            var attribute = method.GetCustomAttribute<EditorProcessAttribute>();
            return attribute?.Order ?? 0;
        }

        private static void OnHierarchyChanged()
        {
            if (_scheduled) return;

            _scheduled = true;

            EditorApplication.delayCall += () =>
            {
                try
                {
                    Process();
                }
                finally
                {
                    _scheduled = false;
                }
            };
        }

        private static void Process()
        {
            if (_isProcessing) return;

            _isProcessing = true;

            try
            {
                foreach (var processor in _processors)
                {
                    if (!processor.IsStatic)
                    {
                        Debug.LogError(
                            $"EditorProcessor: {processor.DeclaringType}.{processor.Name} must be static.");
                        continue;
                    }

                    if (processor.GetParameters().Length != 0)
                    {
                        Debug.LogError(
                            $"EditorProcessor: {processor.DeclaringType}.{processor.Name} must have no parameters.");
                        continue;
                    }

                    processor.Invoke(null, null);
                }
            }
            finally
            {
                _isProcessing = false;
            }
        }
    }
}