namespace MauiSampleCamera;

public partial class CustomMediaPicker
{
	public partial Task<FileResult> CapturePhotoAsync(MediaPickerOptions options)
		=> MediaPicker.Default.CapturePhotoAsync(options);

	public partial Task<FileResult> CaptureVideoAsync(MediaPickerOptions options)
		=> MediaPicker.Default.CaptureVideoAsync(options);
}
