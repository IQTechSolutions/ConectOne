using MudBlazor;

namespace NeuralTech.Services
{
    /// <summary>
    /// Provides information about the current device and manages the application's default theme.
    /// </summary>
    public interface IDeviceInfoService
    {
        bool IsMobilePlatform { get; }
        MudTheme DefaultTheme { get; set; }
    }
}
