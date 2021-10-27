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
				foveatedRenderer.enabled = false;
				foveatedRenderer.enabled = true;

				if (ModSettings.DebugView)
				{
					ViveFoveatedVisualizer viveFoveatedVisualizer = visualizer ?? cameraObject.AddComponent<ViveFoveatedVisualizer>();
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