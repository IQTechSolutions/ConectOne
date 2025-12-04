using ConectOne.Domain.Constants;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using Blazored.LocalStorage;
using SchoolsEnterprise.Base.Constants;

namespace MessagingModule.Application.HubServices
{
    public class NotificationHubService(IConfiguration configuration, ILocalStorageService LocalStorage) : IAsyncDisposable
    {
        private HubConnection _hubConnection;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public event Action<string, string, string, string> OnMessageReceived;
        public event Action<string, string> OnMessageRead;

        public async Task InitializeAsync()
        {
            if (_hubConnection != null)
                return;

            if (OperatingSystem.IsAndroid() && Debugger.IsAttached)
            {
                var accessToken = await SecureStorage.GetAsync("accounttoken");
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                    {
                        return true;
                    }
                };

                _hubConnection = new HubConnectionBuilder().WithUrl($"{configuration["ApiConfiguration:BaseApiAddress"]}/notificationsHub", options =>
                    {
                        options.HttpMessageHandlerFactory = _ => handler;
                        options.AccessTokenProvider = () => Task.FromResult(accessToken);
                    })
                    .WithAutomaticReconnect().Build();
            }
            else
            {
                var accessToken = await LocalStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{configuration["ApiConfiguration:BaseApiAddress"]}/notificationsHub",
                        options => { options.AccessTokenProvider = () => Task.FromResult(accessToken); })
                    .WithAutomaticReconnect().Build();
            }

            // Listen to server messages
            _hubConnection.On<string, string, string, string>(ApplicationConstants.SignalR.SendPushNotification, (type, title, message, url) =>
            {
                OnMessageReceived?.Invoke(type, title, message, url);
            });

            _hubConnection.On<string, string>(ApplicationConstants.SignalR.ReadPushNotification, (type, notificationId) =>
            {
                OnMessageRead?.Invoke(type, notificationId);
            });

            await StartAsync();
        }

        public async Task StartAsync()
        {
            if (_hubConnection.State == HubConnectionState.Connected)
                return;

            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR connected");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignalR connection failed: {ex}");
                // Optionally retry later
            }
        }

        public async Task NotificationRead(string userId, string type, string notificationId)
        {
            try
            {
                await StartAsync();
                await _hubConnection.SendAsync(ApplicationConstants.SignalR.ReadPushNotification, userId, type, notificationId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
