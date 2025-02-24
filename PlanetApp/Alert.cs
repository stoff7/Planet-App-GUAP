using Microsoft.Maui.ApplicationModel;
namespace PlanetApp.Services
{
    public interface IAlertService
    {
        Task DisplayAlert(string title, string message, string cancel);
    }





    public class AlertService : IAlertService
    {
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            // Вызов метода в главном потоке с использованием MainThread
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(title, message, cancel);
            });
        }
    }


}
