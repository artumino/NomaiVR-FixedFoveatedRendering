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
        public static Assembly ViveFFRPlugin;
        public static Dictionary<string, Shader> Shaders;

        internal void Start()
        {
            Helper.Console.WriteLine("Start NomaiVRFoveated");

            //Load Shaders
            LoadShaders();

            SetupFoveatedEnabler();
            LoadManager.OnCompleteSceneLoad += OnSceneLoad;
        }

        private void OnSceneLoad(OWScene originalScene, OWScene loadScene)
        {
            SetupFoveatedEnabler();
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
}
