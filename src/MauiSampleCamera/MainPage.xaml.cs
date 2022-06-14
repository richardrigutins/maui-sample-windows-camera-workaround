using MauiSampleCamera.ViewModels;

namespace MauiSampleCamera;

public partial class MainPage : ContentPage
{
	public MainPage(GalleryViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}
