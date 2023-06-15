namespace PokemonTCGClone.Pages;
using PokemonTCGClone.ViewModels;

public partial class MainPage : ContentPage
{
	//int count = 0;

	public MainPage()
	{
		var mpvm = new MainPageViewModel();
		InitializeComponent();
		this.BindingContext = mpvm;
	}

	
}

