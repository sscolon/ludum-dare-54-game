using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DDCore.Editor
{
    public static class PublishUtility
    {
        private static PublishConfig _config;
        private static void Build(BuildTarget target)
        {
            using (var watch = new Watch("Building Game"))
            {
                string path = $"{_config.rootFolder}/{target}";
                if (!Directory.Exists(_config.rootFolder))
                {
                    Directory.CreateDirectory(_config.rootFolder);
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
                string exe = Path.Combine(path, "Terralora.exe");
                BuildPipeline.BuildPlayer(scenes, exe, target, BuildOptions.None);
            }
        }

        [MenuItem("Build/WebGL")]
        public static void BuildWebGL()
        {
            Build(BuildTarget.WebGL);
        }

        [MenuItem("Build/Windows")]
        public static void BuildWindows64()
        {
            Build(BuildTarget.StandaloneWindows64);
        }

        private static string ZipBuild(string path)
        {
            string output = $"{path}.zip";
            if (File.Exists(output))
                File.Delete(output);
            ZipFile.CreateFromDirectory(path, output, System.IO.Compression.CompressionLevel.NoCompression, false);
            return output;
        }

        [MenuItem("Build/Publish")]
        private static void Deploy()
        {
            //Get directories in build folder.
            string[] directories = Directory.GetDirectories(_config.rootFolder);
            for (int i = 0; i < directories.Length; i++)
            {
                string directory = directories[i];
                string zipPath = ZipBuild(directory);

                //Use directory name for the channel.
                //Definitely gonna adapt this for my other games later.
                string directoryName = Path.GetFileName(directory);
                DeployToItchIO(zipPath, _config.itchGame, $"{directoryName}");
            }
        }

        //Reminder that deployment is built to work with itch.io butler, and won't work if you don't have that properly installed.
        public static void DeployToItchIO(string zipPath, string game, string channel)
        {
            UnityEngine.Debug.Log($"Pushing {zipPath} {Application.version} to {_config.itchUser}/{game}:{channel}");
            string pushCommand = $"/C butler push {zipPath} {_config.itchUser}/{game}:{channel} --userversion {Application.version}";
            Process process = new Process();

            //Hidden process style makes it more clean, just click the button and keep working on whatever you're working on!
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = pushCommand;
            process.StartInfo = startInfo;
            process.Start();
        }

        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            FetchConfig();
        }

        private static void FetchConfig()
        {
            while (true)
            {
                if (_config != null) return;

                var path = GetConfigPath();

                if (path == null)
                {
                    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PublishConfig>(), $"Assets/{nameof(PublishConfig)}.asset");
                    DebugWrapper.Log("A  publish file has been created at the root of your project.<b> You can move this anywhere you'd like.</b>");
                    continue;
                }

                _config = AssetDatabase.LoadAssetAtPath<PublishConfig>(path);
                break;
            }
        }

        private static string GetConfigPath()
        {
            var paths = AssetDatabase.FindAssets(nameof(PublishConfig)).Select(AssetDatabase.GUIDToAssetPath).Where(c => c.EndsWith(".asset")).ToList();
            if (paths.Count > 1) DebugWrapper.LogWarning("Multiple auto save config assets found. Delete one.");
            return paths.FirstOrDefault();
        }

    }
}
