using System;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace UnityGameStarter.EditorUtilities.Installation
{
    [InitializeOnLoad]
    static class PackageSynchronizer
    {
        private static FileSystemWatcher[] _watchers;
        private static int _syncPending;
        private static bool _isSync;

        private static readonly (string source, string target)[] _directories =
        {
            (starterTemplatesSource, starterTemplatesPackage),
            (projectSettingsSource, projectSettingsPackage)
        };

        private const string developmentMarker = ".unity-game-starter-dev";
        private const string packageName = "com.nibour.unity-game-starter";

        internal const string starterTemplatesSource = "Assets/GameStarterTemplates";
        internal const string starterTemplatesPackage = "Templates~";
        internal const string projectSettingsSource = "ProjectSettings";
        internal const string projectSettingsPackage = "ProjectSettingsTemplates~";

        static PackageSynchronizer()
        {
            if (!IsDevelopmentProject(out string projectPath)) return;
            
            InitializeWatchers(projectPath);

            EditorApplication.update += OnEditorUpdate;
            EditorApplication.delayCall += Sync;
        }

        private static bool IsDevelopmentProject(out string projectPath)
        {
            projectPath = Directory.GetParent(Application.dataPath).FullName;

            if (!File.Exists(Path.Combine(projectPath, developmentMarker))) return false;

            var info = UnityEditor.PackageManager.PackageInfo.FindForAssembly(
                typeof(PackageSynchronizer).Assembly);

            return info != null && info.name == packageName;
        }

        private static void InitializeWatchers(string projectPath)
        {
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
            => Interlocked.Exchange(ref _syncPending, 1);

        private static void OnSourceRenamed(object sender, RenamedEventArgs args)
            => Interlocked.Exchange(ref _syncPending, 1);

        private static void OnEditorUpdate()
        {
            if (Interlocked.Exchange(ref _syncPending, 0) == 0) return;

            EditorApplication.delayCall += Sync;
        }

        private static void Sync()
        {
            if (_isSync) return;

            if (!IsDevelopmentProject(out string projectPath)) return;

            _isSync = true;

            try
            {
                var info = UnityEditor.PackageManager.PackageInfo.FindForAssembly(
                    typeof(PackageSynchronizer).Assembly);

                if (info == null)
                {
                    Debug.LogError("[Unity Game Starter] Unable to locate package.");
                    return;
                }

                string packagePath = info.resolvedPath;

                foreach (var (source, target) in _directories)
                {
                    string sourcePath = Path.Combine(projectPath, source);
                    string targetPath = Path.Combine(packagePath, target);

                    SyncDirectory(sourcePath, targetPath);
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                _isSync = false;
            }
        }

        private static void SyncDirectory(string source, string target)
        {
            if (!Directory.Exists(source))
            {
                if (Directory.Exists(target))
                    Directory.Delete(target, true);

                return;
            }

            Directory.CreateDirectory(target);

            foreach (string file in Directory.GetFiles(source))
            {
                string targetFile = Path.Combine(target, Path.GetFileName(file));
                File.Copy(file, targetFile, true);
            }

            foreach (string file in Directory.GetFiles(target))
            {
                string sourceFile = Path.Combine(source, Path.GetFileName(file));

                if (!File.Exists(sourceFile))
                    File.Delete(file);
            }

            foreach (string directory in Directory.GetDirectories(source))
            {
                string targetDirectory = Path.Combine(target, Path.GetFileName(directory));
                SyncDirectory(directory, targetDirectory);
            }

            foreach (string directory in Directory.GetDirectories(target))
            {
                string sourceDirectory = Path.Combine(source, Path.GetFileName(directory));

                if (!Directory.Exists(sourceDirectory))
                    Directory.Delete(directory, true);
            }
        }
    }
}
