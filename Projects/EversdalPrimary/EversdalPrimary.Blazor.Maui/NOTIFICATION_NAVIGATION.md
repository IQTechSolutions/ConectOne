# Notification navigation usage

The MAUI app wires push notification payloads to in-app navigation through `NotificationNavigationService`.
Use these guidelines when sending a push notification (e.g., via Firebase):

1. **Include a navigation URL/key in the data payload.**
   - Preferred key: `"Url"`
   - Alternate key supported today: `"NavigationID"`
   - Example Firebase data message:
     ```json
     {
       "to": "<device-token>",
       "data": {
         "Url": "/messages" ,
         "title": "New message",
         "body": "Tap to open your inbox"
       }
     }
     ```

2. **Use app-relative routes.**
   - Supply the route as you would navigate inside the Blazor app (e.g., `/messages`, `/notifications/123`).
   - If the value does not start with `/`, the service will prepend it for you.

3. **Android intent handling.**
   - `MainActivity` extracts the `Url`/`NavigationID` extras from the notification intent and forwards them to `NotificationNavigationService` as soon as the app opens or when the intent is delivered while the app is running.

4. **Deferred navigation until Blazor is ready.**
   - `Routes.razor` registers the Blazor `NavigationManager` with `NotificationNavigationService` after the first render.
   - If a notification arrives before that point, the target URL is queued and executed once routing is available, ensuring navigation still happens after the app finishes loading.

5. **Foreground/background behavior.**
   - Navigation is invoked on the UI thread, so tapping the notification will bring the app to the foreground and navigate to the specified route.

Use the above payload structure so that a message notification can open the Messages page (e.g., `/messages`) or any other in-app route you need.
