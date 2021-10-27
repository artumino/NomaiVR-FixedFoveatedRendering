using System.Linq;
using UnityEngine;

namespace NomaiVRFoveated
{
    internal abstract class NomaiVRModule<Behaviour>
        where Behaviour : MonoBehaviour
    {
        protected static OWScene[] PlayableScenes = new[] { OWScene.SolarSystem, OWScene.EyeOfTheUniverse };
        protected static OWScene[] TitleScene = new[] { OWScene.TitleScreen };
        protected static OWScene[] SolarSystemScene = new[] { OWScene.SolarSystem };
        protected static OWScene[] AllScenes = new OWScene[] { };

        protected abstract OWScene[] Scenes { get; }

        public NomaiVRModule()
        {
            if (IsSceneRelevant(LoadManager.GetCurrentScene()))
            {
                SetupBehaviour();
            }

            LoadManager.OnCompleteSceneLoad += OnSceneLoad;
        }

        private void OnSceneLoad(OWScene originalScene, OWScene loadScene)
        {
            if (IsSceneRelevant(loadScene))
            {
                SetupBehaviour();
            }
        }

        private bool IsSceneRelevant(OWScene scene)
        {
            return Scenes.Length == 0 || Scenes.Contains(scene);
        }

        private void SetupBehaviour()
        {
            NomaiVRFoveated.Helper.Logger.Log($"Creating NomaiVRFoveated behaviour for {GetType().Name}");
            var gameObject = new GameObject();
            gameObject.AddComponent<Behaviour>();
        }
    }
}
