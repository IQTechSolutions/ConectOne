using ConectOne.Blazor.Settings;
using Microsoft.Maui.Devices;
using MudBlazor;
using NeuralTech.Services;

namespace ConectOne.Blazor.Services
{
    /// <summary>
    /// Provides information about the current device and manages the application's default theme.
    /// </summary>
    public class DeviceInfoService : IDeviceInfoService
    {
        /// <summary>
        /// Gets a value indicating whether the application is running on a mobile platform, such as Android or iOS.
        /// </summary>
        public bool IsMobilePlatform => Microsoft.Maui.Devices.DeviceInfo.Current.Platform == DevicePlatform.Android || Microsoft.Maui.Devices.DeviceInfo.Current.Platform == DevicePlatform.iOS;

        /// <summary>
        /// Gets or sets the default theme applied to the application.
        /// </summary>
        public MudTheme DefaultTheme { get; set; } = ApplciationTheme.LightTheme;
    }
}
