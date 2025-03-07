namespace PlanetApp
{
	public partial class CreatePlanetPart : ContentPage
	{
		public CreatePlanetPart(TempData tempData, string destination)
		{
			InitializeComponent();
			BindingContext = new CreatePlanetPartViewModel(tempData, destination);
		}
	}
}
