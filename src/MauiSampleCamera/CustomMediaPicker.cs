namespace MauiSampleCamera;

public partial class CustomMediaPicker : IMediaPicker
{
	public bool IsCaptureSupported
		=> MediaPicker.Default.IsCaptureSupported;

	public partial Task<FileResult> CapturePhotoAsync(MediaPickerOptions options = null);

	public partial Task<FileResult> CaptureVideoAsync(MediaPickerOptions options = null);

	public Task<FileResult> PickPhotoAsync(MediaPickerOptions options = null)
		=> MediaPicker.Default.PickPhotoAsync(options);

	public Task<FileResult> PickVideoAsync(MediaPickerOptions options = null)
		=> MediaPicker.Default.PickVideoAsync(options);
}
