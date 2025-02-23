using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace PlanetApp.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        public ICommand CreatePlanetCommand { get; }
        public ICommand ViewPlanetsCommand { get; }

        public MainPageViewModel()
        {
            CreatePlanetCommand = new Command(OnCreatePlanet);
            ViewPlanetsCommand = new Command(OnViewPlanets);
        }

        private async void OnCreatePlanet()
        {
            // Здесь можно использовать абстракцию навигации, для простоты:
            await Application.Current.MainPage.Navigation.PushAsync(new CreatePlanet());
        }

        private async void OnViewPlanets()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewPlanet());
        }
    }
}
