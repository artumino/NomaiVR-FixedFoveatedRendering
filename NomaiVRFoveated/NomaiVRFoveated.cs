using OWML.Common;
using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NomaiVRFoveated
{
    public class NomaiVRFoveated : ModBehaviour
    {
        public static IModHelper Helper;
        public static Dictionary<string, Shader> Shaders;

        internal void Start()
        {
            Helper.Console.WriteLine("Start NomaiVRFoveated");

            //Load Shaders
            LoadShaders();

            //Shader patches
            ModHelper.HarmonyHelper.AddPrefix<Shader>(nameof(Shader.Find), typeof(NomaiVRFoveated), nameof(NomaiVRFoveated.FindShaderOverride));

            SetupFoveatedEnabler();
            LoadManager.OnCompleteSceneLoad += OnSceneLoad;
        }

        private void OnSceneLoad(OWScene originalScene, OWScene loadScene)
        {
            if (loadScene != OWScene.PostCreditsScene
                && loadScene != OWScene.Credits_Fast
                && loadScene != OWScene.Credits_Final)
            {
                SetupFoveatedEnabler();
            }
        }

        private static bool FindShaderOverride(string name, ref Shader __result)
        {
            if(Shaders.ContainsKey(name))
            {
                __result = Shaders[name];
                return false;
            }
            return true;
        }

        private void LoadShaders()
        {
            Shaders = new Dictionary<string, Shader>();
            var shaderBundle = Helper.Assets.LoadBundle($"assets/foveated-shaders");
            var shaders = shaderBundle.LoadAllAssets<Shader>();
            foreach (Shader shader in shaders)
                Shaders.Add(shader.name, shader);
        }

        private void SetupFoveatedEnabler()
        {
            Helper.Console.WriteLine($"Creating NomaiVRFoveated behaviour for {typeof(FoveatedRenderingEnabler).Name}", MessageType.Info);
            var gameObject = new GameObject();
            gameObject.AddComponent<FoveatedRenderingEnabler>();
        }

        public override void Configure(IModConfig config)
        {
            Helper = ModHelper;
            ModSettings.SetConfig(config);
        }
    }

    /// <summary>
    /// Patches the game to use both old and new input system
    /// Moves VR Plugin files to the appropriate folders
    /// </summary>
    public static class NomaiVRFoveatedPatcher
    {
        //Called by OWML
        public static void Main(string[] args)
        {
            var basePath = args.Length > 0 ? args[0] : ".";
            var pluginsPath = Path.Combine(GetDataPath(AppDomain.CurrentDomain.BaseDirectory), "Plugins", "x86_64");
            CopyGameFiles(pluginsPath, Path.Combine(basePath, "plugin"));
        }

        private static string GetExecutableName(string gamePath)
        {
            var executableNames = new[] { "Outer Wilds.exe", "OuterWilds.exe" };
            foreach (var executableName in executableNames)
            {
                var executablePath = Path.Combine(gamePath, executableName);
                if (File.Exists(executablePath))
                {
                    return Path.GetFileNameWithoutExtension(executablePath);
                }
            }

            throw new FileNotFoundException($"Outer Wilds exe file not found in {gamePath}");
        }

        private static string GetDataDirectoryName()
        {
            var gamePath = AppDomain.CurrentDomain.BaseDirectory;
            return $"{GetExecutableName(gamePath)}_Data";
        }

        private static string GetDataPath(string gamePath)
        {
            return Path.Combine(gamePath, $"{GetDataDirectoryName()}");
        }

        private static void CopyGameFiles(string gamePath, string filesPath)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(filesPath);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + filesPath);
            }

            var dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(gamePath);

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var tempPath = Path.Combine(gamePath, file.Name);
                file.CopyTo(tempPath, true);
            }

            foreach (var subdir in dirs)
            {
                var tempPath = Path.Combine(gamePath, subdir.Name);
                CopyGameFiles(tempPath.Replace("OuterWilds_Data", GetDataDirectoryName()), subdir.FullName);
            }
        }
    }
}
