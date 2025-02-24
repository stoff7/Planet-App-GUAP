using PlanetLib;
using PlanetApp.ViewModels;

namespace PlanetApp
{
    public partial class EditPlanet : ContentPage
    {
        public EditPlanet(Planet planet)
        {
            InitializeComponent();
            BindingContext = new EditPlanetViewModel(planet);

        }
    }
}