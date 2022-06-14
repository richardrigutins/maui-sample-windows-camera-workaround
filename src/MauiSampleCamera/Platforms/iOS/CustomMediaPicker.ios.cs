namespace MauiSampleCamera;

public partial class CustomMediaPicker
{
	public partial Task<FileResult> CapturePhotoAsync(MediaPickerOptions options = null)
		=> MediaPicker.Default.CapturePhotoAsync(options);

	public partial Task<FileResult> CaptureVideoAsync(MediaPickerOptions options = null)
		=> MediaPicker.Default.CaptureVideoAsync(options);
}
