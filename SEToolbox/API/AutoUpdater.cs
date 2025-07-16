using System.Diagnostics;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SEToolBox.API
{
    public class AutoUpdater
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private const string GITHUB_API_URL = "https://api.github.com/repos/Mr-Baguetter/SEToolBox/releases/latest";
        private const string UPDATE_FOLDER = "Update";
        private const string UPDATER_BATCH = "update.bat";

        static AutoUpdater()
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "SEToolBox-AutoUpdater");
        }

        public static async Task<bool> CheckForUpdatesAsync()
        {
            try
            {
                var response = await httpClient.GetStringAsync(GITHUB_API_URL);
                var release = JsonSerializer.Deserialize<GitHubRelease>(response);

                if (release?.TagName != null)
                {
                    var latestVersion = ParseVersion(release.TagName);
                    var currentVersion = MainForm.version;

                    return latestVersion > currentVersion;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates: {ex.Message}", "Update Check Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return false;
        }

        public static async Task<bool> DownloadAndInstallUpdateAsync()
        {
            try
            {
                var response = await httpClient.GetStringAsync(GITHUB_API_URL);
                var release = JsonSerializer.Deserialize<GitHubRelease>(response);

                if (release?.Assets == null) return false;

                var zipAsset = release.Assets.FirstOrDefault(a => a.Name == "SEToolBox.zip");
                if (zipAsset == null)
                {
                    MessageBox.Show("SEToolBox.zip not found in latest release!", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (Directory.Exists(UPDATE_FOLDER))
                    Directory.Delete(UPDATE_FOLDER, true);
                Directory.CreateDirectory(UPDATE_FOLDER);

                var zipPath = Path.Combine(UPDATE_FOLDER, "SEToolBox.zip");
                using (var zipResponse = await httpClient.GetAsync(zipAsset.BrowserDownloadUrl))
                {
                    using (var fileStream = File.Create(zipPath))
                    {
                        await zipResponse.Content.CopyToAsync(fileStream);
                    }
                }

                var extractPath = Path.Combine(UPDATE_FOLDER, "extracted");
                ZipFile.ExtractToDirectory(zipPath, extractPath);

                CreateUpdaterScript(extractPath);

                var updaterPath = Path.Combine(UPDATE_FOLDER, UPDATER_BATCH);
                if (File.Exists(updaterPath))
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = updaterPath,
                        UseShellExecute = true,
                        CreateNoWindow = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    };

                    Process.Start(psi);

                    Application.Exit();
                    return true;
                }
                else
                {
                    MessageBox.Show("Updater script not found!", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading update: {ex.Message}", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static void CreateUpdaterScript(string extractPath)
        {
            var currentExePath = Application.ExecutablePath;
            var currentDir = Path.GetDirectoryName(currentExePath);
            var currentExeName = Path.GetFileName(currentExePath);
            var processName = Path.GetFileNameWithoutExtension(currentExePath);

            var batchContent = $@"@echo off
                echo Updating SEToolBox...

                REM Wait for the main application to fully exit
                :wait_for_exit
                tasklist /FI ""IMAGENAME eq {currentExeName}"" 2>NUL | find /I /N ""{currentExeName}"">NUL
                if ""%ERRORLEVEL%""==""0"" (
                    echo Waiting for {currentExeName} to exit...
                    timeout /t 2 /nobreak >nul
                    goto wait_for_exit
                )

                echo Application closed, proceeding with update...
                timeout /t 1 /nobreak >nul

                echo Backing up current files...
                if exist ""{currentDir}\\backup"" rmdir /s /q ""{currentDir}\\backup""
                mkdir ""{currentDir}\\backup""

                REM Copy current files to backup (excluding update folder)
                for /d %%d in (""{currentDir}\\*"") do (
                    if /i not ""%%~nxd""==""Update"" (
                        xcopy ""%%d"" ""{currentDir}\\backup\\%%~nxd\\"" /E /Y /Q >nul
                    )
                )

                for %%f in (""{currentDir}\\*"") do (
                    if /i not ""%%~nxf""==""Update"" (
                        copy ""%%f"" ""{currentDir}\\backup\\"" /Y >nul
                    )
                )

                echo Installing new version...

                REM Find the source folder - check if there's a subfolder in extracted folder
                set ""SOURCE_DIR={extractPath}""
                if exist ""{extractPath}\\SEToolBox"" (
                    set ""SOURCE_DIR={extractPath}\\SEToolBox""
                    echo Found SEToolBox subfolder, using that as source
                ) else (
                    echo Using extracted folder directly as source
                )

                REM Copy all files from source directory to current directory, overwriting existing
                echo Copying from %SOURCE_DIR% to {currentDir}
                xcopy ""%SOURCE_DIR%\\*"" ""{currentDir}\\"" /E /Y /H /R /Q

                REM Verify the main executable exists
                if not exist ""{currentExePath}"" (
                    echo Error: Main executable not found after update! Restoring backup...
                    xcopy ""{currentDir}\\backup\\*"" ""{currentDir}\\"" /E /Y /H /R /Q
                    rmdir /s /q ""{currentDir}\\backup""
                    echo Backup restored. Update failed.
                    pause
                    exit /b 1
                )

                echo Cleaning up...
                rmdir /s /q ""{Path.GetFullPath(UPDATE_FOLDER)}""
                rmdir /s /q ""{currentDir}\\backup""

                echo Starting SEToolBox...
                start """" ""{currentExePath}""

                echo Update completed successfully!
                timeout /t 2 /nobreak >nul

                REM Self-delete the batch file
                (goto) 2>nul & del ""%~f0""
                ";

            var batchPath = Path.Combine(UPDATE_FOLDER, "update.bat");
            File.WriteAllText(batchPath, batchContent);
        }

        private static Version ParseVersion(string tagName)
        {
            var versionString = tagName.StartsWith("v") ? tagName.Substring(1) : tagName;

            if (Version.TryParse(versionString, out var version))
                return version;

            return new Version(0, 0, 0);
        }
    }

    public class GitHubRelease
    {
        [JsonPropertyName("tag_name")]
        public string TagName { get; set; }

        [JsonPropertyName("assets")]
        public GitHubAsset[] Assets { get; set; }
    }

    public class GitHubAsset
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("browser_download_url")]
        public string BrowserDownloadUrl { get; set; }
    }
}