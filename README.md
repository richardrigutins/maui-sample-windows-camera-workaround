# maui-sample-windows-camera-workaround

This repository contains a sample .NET MAUI camera app that illustrates a workaround to allow capturing photos and videos on Windows.

## The problem

Currently, the `CapturePhotoAsync` and `CaptureVideoAsync` methods in the default `IMediaPicker` implementation (`MediaPicker.Default`) always return null when running on Windows.

The workaround consists of creating a custom `IMediaPicker` implementation, that uses the launcher to open the Windows Camera app.
On the other platform, the default `IMediaPicker` implementation is used.

## Workaround logic

The workaround logic is contained in the platform specific `CustomMediaPicker` class implementation for Windows:

```csharp
// Based on code by https://github.com/GiampaoloGabba
// See https://github.com/dotnet/maui/issues/7660

using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.System;
using WinRT.Interop;

namespace MauiSampleCamera;

public partial class CustomMediaPicker : IMediaPicker
{
	public partial Task<FileResult> CapturePhotoAsync(MediaPickerOptions options = null)
		=> CaptureAsync(false);

	public partial Task<FileResult> CaptureVideoAsync(MediaPickerOptions options = null)
		=> CaptureAsync(true);

	private async Task<FileResult> CaptureAsync(bool isVideo)
	{
		var captureUi = new CustomCameraCaptureUI();

		StorageFile file = await captureUi.CaptureFileAsync(isVideo ? CameraCaptureUIMode.Video : CameraCaptureUIMode.Photo);

		if (file != null)
		{
			return new FileResult(file.Path, file.ContentType);
		}

		return null;
	}

	private class CustomCameraCaptureUI
	{
		private readonly LauncherOptions _launcherOptions;

		public CustomCameraCaptureUI()
		{
			var window = WindowStateManager.Default.GetActiveWindow();
			var handle = WindowNative.GetWindowHandle(window);

			_launcherOptions = new LauncherOptions();
			InitializeWithWindow.Initialize(_launcherOptions, handle);

			_launcherOptions.TreatAsUntrusted = false;
			_launcherOptions.DisplayApplicationPicker = false;
			_launcherOptions.TargetApplicationPackageFamilyName = "Microsoft.WindowsCamera_8wekyb3d8bbwe";
		}

		public async Task<StorageFile> CaptureFileAsync(CameraCaptureUIMode mode)
		{
			var extension = mode == CameraCaptureUIMode.Photo ? ".jpg" : ".mp4";

			var currentAppData = ApplicationData.Current;
			var tempLocation = currentAppData.LocalCacheFolder;
			var tempFileName = $"capture{extension}";
			var tempFile = await tempLocation.CreateFileAsync(tempFileName, CreationCollisionOption.GenerateUniqueName);
			var token = Windows.ApplicationModel.DataTransfer.SharedStorageAccessManager.AddFile(tempFile);

			var set = new ValueSet();
			if (mode == CameraCaptureUIMode.Photo)
			{
				set.Add("MediaType", "photo");
				set.Add("PhotoFileToken", token);
			}
			else
			{
				set.Add("MediaType", "video");
				set.Add("VideoFileToken", token);
			}

			var uri = new Uri("microsoft.windows.camera.picker:");
			var result = await Windows.System.Launcher.LaunchUriForResultsAsync(uri, _launcherOptions, set);
			if (result.Status == LaunchUriStatus.Success && result.Result != null)
			{
				return tempFile;
			}

			return null;
		}
	}
}
```
