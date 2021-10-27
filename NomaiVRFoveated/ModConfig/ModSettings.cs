using OWML.Common;
using System;
using UnityEngine;

namespace NomaiVRFoveated
{
    public static class ModSettings
    {
        public static event Action OnConfigChange;

        private static readonly string[] s_ShadingRates = { "CULL", "X16_PER_PIXEL", "X8_PER_PIXEL", "X4_PER_PIXEL", "X2_PER_PIXEL", "X1_PER_PIXEL", "X1_PER_2X1_PIXELS", "X1_PER_1X2_PIXELS", "X1_PER_2X2_PIXELS", "X1_PER_4X2_PIXELS", "X1_PER_2X4_PIXELS", "X1_PER_4X4_PIXELS" };

        public static bool FoveatedRenderingEnabled { get; private set; } = true;
        public static bool DebugView { get; private set; } = false;
        public static bool CustomRegionValues { get; private set; } = false;
        public static int InnerShadingRate { get; private set; } = Array.IndexOf(s_ShadingRates, "X1_PER_PIXEL");
        public static uint InnerRadiusX { get; private set; } = 15;
        public static uint InnerRadiusY { get; private set; } = 15;
        public static int MidShadingRate { get; private set; } = Array.IndexOf(s_ShadingRates, "X1_PER_1X2_PIXELS");
        public static uint MidRadiusX { get; private set; } = 25;
        public static uint MidRadiusY { get; private set; } = 25;
        public static int PeripheralShadingRate { get; private set; } = Array.IndexOf(s_ShadingRates, "X1_PER_2X4_PIXELS");
        public static uint PeripheralRadiusX { get; private set; } = 115;
        public static uint PeripheralRadiusY { get; private set; } = 115;


        public static void SetConfig(IModConfig config)
        {
            FoveatedRenderingEnabled = config.GetSettingsValue<bool>("foveatedRenderingEnabled");
            DebugView = config.GetSettingsValue<bool>("debugView");
            CustomRegionValues = config.GetSettingsValue<bool>("customRegionValues");
            InnerShadingRate = config.GetSettingsValue<int>("innerRateOverride") + 1;
            InnerRadiusX = config.GetSettingsValue<uint>("innerRadiusX") % 360;
            InnerRadiusY = config.GetSettingsValue<uint>("innerRadiusY") % 360;
            MidShadingRate = config.GetSettingsValue<int>("midRateOverride") + 1;
            MidRadiusX = config.GetSettingsValue<uint>("middleRadiusX") % 360;
            MidRadiusY = config.GetSettingsValue<uint>("middleRadiusY") % 360;
            PeripheralShadingRate = config.GetSettingsValue<int>("peripheralRateOverride") + 1;
            PeripheralRadiusX = config.GetSettingsValue<uint>("peripheralRadiusX") % 360;
            PeripheralRadiusY = config.GetSettingsValue<uint>("peripheralRadiusY") % 360;
            OnConfigChange?.Invoke();
        }
    }
}
