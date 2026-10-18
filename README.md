# WinStream

A small Windows app that puts your streaming services in one window. It uses the
Microsoft Edge WebView2 runtime embedded in the app, so nothing opens in the
separate Edge app. You sign in with your own accounts, and sign-ins are
remembered between launches.

Services: Netflix, Disney+, Peacock, Prime Video, HBO Max, Hulu.
### If installing via the .exe installer, Windows SmartScreen may give a warning. This does NOT mean that it contains a virus, its just because it's not signed. You can safely bypass the warning.
## Requirements
- Windows 10/11
- .NET 8 SDK (https://dotnet.microsoft.com/download)
- Microsoft Edge WebView2 Runtime (already included with current Windows 11 and Edge)

## Run
```
dotnet run
```

## Build a standalone exe
```
dotnet publish -c Release -r win-x64 --self-contained false
```
Output is in `bin\Release\net8.0-windows\win-x64\publish\`.

## Use
- Click a service tile to open it inside the app.
- Click the small home icon (top left) to go back to the service list.
- Video fullscreen works inside the app window; press Esc in the player to exit.

## Notes
- Saved sign-ins live in `%LOCALAPPDATA%\WinStream\WebView2Profile`. Delete that
  folder to sign out of everything.
