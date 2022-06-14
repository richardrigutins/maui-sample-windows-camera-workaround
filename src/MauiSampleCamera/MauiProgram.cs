using MauiSampleCamera.ViewModels;

namespace MauiSampleCamera;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<IMediaPicker, CustomMediaPicker>();
		builder.Services.AddSingleton<GalleryViewModel>();
		builder.Services.AddSingleton<MainPage>();

		return builder.Build();
	}
}
