using System;
using HTC.UnityPlugin.FoveatedRendering;
using UnityEngine;

namespace NomaiVRFoveated
{
	public class FoveatedRenderingEnabler : MonoBehaviour
	{
		private OWCamera _playerCamera;

		public void Start()
		{
			this.UpdateFoveatedSettings();
			ModSettings.OnConfigChange += this.UpdateFoveatedSettings;
		}

		public void OnDestroy()
		{
			ModSettings.OnConfigChange -= this.UpdateFoveatedSettings;
		}

		private void UpdateFoveatedSettings()
		{
			this._playerCamera = Locator.GetPlayerCamera();
			GameObject cameraObject = this._playerCamera.mainCamera.gameObject;
			ViveFoveatedRendering foveatedRenderer = cameraObject.GetComponent<ViveFoveatedRendering>();
			ViveFoveatedGazeUpdater foveatedGazeTracker = cameraObject.GetComponent<ViveFoveatedGazeUpdater>();
			ViveFoveatedVisualizer visualizer = cameraObject.GetComponent<ViveFoveatedVisualizer>();

			if (ModSettings.FoveatedRenderingEnabled)
			{
				foveatedRenderer = foveatedRenderer ?? cameraObject.AddComponent<ViveFoveatedRendering>();
				foveatedGazeTracker = foveatedGazeTracker ?? cameraObject.AddComponent<ViveFoveatedGazeUpdater>();

				foveatedRenderer.enabled = false;
				foveatedRenderer.enabled = true;

				if (ModSettings.CustomRegionValues)
                {
					//Update settings
					foveatedRenderer.SetPatternPreset(ShadingPatternPreset.SHADING_PATTERN_CUSTOM);
					foveatedRenderer.SetRegionRadii(TargetArea.INNER, ModSettings.InnerRadii);
					foveatedRenderer.SetRegionRadii(TargetArea.MIDDLE, ModSettings.MidRadii);
					foveatedRenderer.SetRegionRadii(TargetArea.PERIPHERAL, ModSettings.PeripheralRadii);

					foveatedRenderer.SetShadingRatePreset(ShadingRatePreset.SHADING_RATE_CUSTOM);
					foveatedRenderer.SetShadingRate(TargetArea.INNER, ModSettings.InnerShading);
					foveatedRenderer.SetShadingRate(TargetArea.MIDDLE, ModSettings.MidShading);
					foveatedRenderer.SetShadingRate(TargetArea.PERIPHERAL, ModSettings.PeripheralShading);
				}
				else if(!ViveFoveatedInitParam.SetParamByHMD(foveatedRenderer))
                {
					foveatedRenderer.SetPatternPreset(ShadingPatternPreset.SHADING_PATTERN_NARROW);
					foveatedRenderer.SetShadingRatePreset(ShadingRatePreset.SHADING_RATE_HIGHEST_PERFORMANCE);
				}

				if (ModSettings.DebugView)
				{
					visualizer = visualizer ?? cameraObject.AddComponent<ViveFoveatedVisualizer>();
					visualizer.enabled = false;
					visualizer.enabled = true;
				}
				else if (visualizer != null) Destroy(visualizer);
			}
			else
			{
				if (foveatedRenderer != null) Destroy(foveatedRenderer);
				if (foveatedGazeTracker != null) Destroy(foveatedGazeTracker);
				if (visualizer != null) Destroy(visualizer);
			}
		}
	}
}