using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using EversdalPrimary.Blazor.Maui.Services;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Firebase.CloudMessaging;

namespace EversdalPrimary.Blazor.Maui
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HandleIntent(Intent);
            CreateNotificationChannelIfNeeded();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            HandleIntent(intent);
        }

        private static void HandleIntent(Intent intent)
        {
            FirebaseCloudMessagingImplementation.OnNewIntent(intent);

            if (intent?.Extras is null || MauiProgram.Services is null)
            {
                return;
            }

            var targetUrl = intent.Extras.GetString("Url") ?? intent.Extras.GetString("NavigationID");
            var navigationService = MauiProgram.Services.GetService<NotificationNavigationService>();

            navigationService?.NavigateFromNotification(targetUrl);
        }

        private void CreateNotificationChannelIfNeeded()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                CreateNotificationChannel();
            }
        }

        private void CreateNotificationChannel()
        {
            var channelId = $"{PackageName}.general";
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            var channel = new NotificationChannel(channelId, "General", NotificationImportance.Default);
            notificationManager.CreateNotificationChannel(channel);
            FirebaseCloudMessagingImplementation.ChannelId = channelId;
        }
    }
}
