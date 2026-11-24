using System;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace UnityGameStarter.EditorUtilities.Installation
{
    [InitializeOnLoad]
    static class PackageCopier
    {
        private static FileSystemWatcher[] _watchers;
        private static int _copyPending;
        private static bool _isCoping;

        private static readonly (string source, string target)[] _directories =
        {
            ("GameStarterTemplates", "templates~"),
            ("ProjectSettings", "ProjectSettingsTemplates~")
        };

        static PackageCopier()
        {
            InitializeWatchers();

            EditorApplication.update += OnEditorUpdate;
            EditorApplication.delayCall += Copy;
        }

        private static void InitializeWatchers()
        {
            string projectPath = Directory.GetParent(Application.dataPath).FullName;
            _watchers = new FileSystemWatcher[_directories.Length];

            for (int i = 0; i < _directories.Length; i++)
            {
                string sourcePath = Path.Combine(projectPath, _directories[i].source);

                if (!Directory.Exists(sourcePath)) continue;

                var watcher = new FileSystemWatcher(sourcePath)
                {
                    IncludeSubdirectories = true,
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName |
                        NotifyFilters.LastWrite | NotifyFilters.Size
                };

                watcher.Changed += OnSourceChanged;
                watcher.Created += OnSourceChanged;
                watcher.Deleted += OnSourceChanged;
                watcher.Renamed += OnSourceRenamed;

                watcher.EnableRaisingEvents = true;
                _watchers[i] = watcher;
            }
        }

        private static void OnSourceChanged(object sender, FileSystemEventArgs args)
            => Interlocked.Exchange(ref _copyPending, 1);

        private static void OnSourceRenamed(object sender, RenamedEventArgs args)
            => Interlocked.Exchange(ref _copyPending, 1);

        private static void OnEditorUpdate()
        {
            if (Interlocked.Exchange(ref _copyPending, 0) == 0) return;

            EditorApplication.delayCall += Copy;
        }

        public static void Copy()
        {
            if (_isCoping) return;

            _isCoping = true;

            try
            {
                var info = UnityEditor.PackageManager.PackageInfo.FindForAssembly(
                    typeof(PackageCopier).Assembly);

                if (info == null)
                {
                    Debug.LogError("[Unity Game Starter] Unable to locate package.");
                    return;
                }

                string projectPath = Directory.GetParent(Application.dataPath).FullName;
                string packagePath = Path.Combine(projectPath, info.assetPath);

                foreach (var (source, target) in _directories)
                {
                    string sourcePath = Path.Combine(projectPath, source);
                    string targetPath = Path.Combine(packagePath, target);

                    CopyDirectory(sourcePath, targetPath);
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                _isCoping = false;
            }
        }

        private static void CopyDirectory(string source, string target)
        {
            if (!Directory.Exists(source)) return;

            Directory.CreateDirectory(target);

            foreach (string file in Directory.GetFiles(source))
            {
                string targetFile = Path.Combine(target, Path.GetFileName(file));
                File.Copy(file, targetFile, true);
            }

            foreach (string directory in Directory.GetDirectories(source))
            {
                string targetDirectory = Path.Combine(target, Path.GetFileName(directory));
                CopyDirectory(directory, targetDirectory);
            }
        }
    }
}
