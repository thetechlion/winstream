using System.Windows;
using Velopack;
using Velopack.Sources;

namespace WinStream;

public static class Updater
{
    // Public repo, so no access token is needed for update checks.
    private const string RepoUrl = "https://github.com/thetechlion/winstream";

    /// <summary>
    /// Checks GitHub Releases for a newer version, downloads it in the background,
    /// then asks the user whether to restart now. Does nothing when running from
    /// a dev build (not installed) and never throws.
    /// </summary>
    public static async Task CheckForUpdatesAsync()
    {
        try
        {
            var mgr = new UpdateManager(new GithubSource(RepoUrl, null, false));
            if (!mgr.IsInstalled) return;

            var update = await mgr.CheckForUpdatesAsync();
            if (update == null) return;

            await mgr.DownloadUpdatesAsync(update);

            var answer = MessageBox.Show(
                $"WinStream {update.TargetFullRelease.Version} is ready to install.\n\nRestart now to update?",
                "WinStream update", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (answer == MessageBoxResult.Yes)
                mgr.ApplyUpdatesAndRestart(update);
            else
                mgr.WaitExitThenApplyUpdates(update); // installs when the app is closed
        }
        catch
        {
            // Offline, rate-limited, etc. Updating is best-effort; ignore.
        }
    }
}
