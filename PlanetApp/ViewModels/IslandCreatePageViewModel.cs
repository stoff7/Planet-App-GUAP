using System.Text.Json;
using System.Windows.Input;
using PlanetLib;
using System.ComponentModel;

namespace PlanetApp
{
    public class IslandCreatePageViewModel : BindableObject, INotifyPropertyChanged
    {
        private string _islandName;
        public string IslandName
        {
            get => _islandName;
            set { _islandName = value; OnPropertyChanged(); }
        }

        private string _islandArea;
        public string IslandArea
        {
            get => _islandArea;
            set { _islandArea = value; OnPropertyChanged(); }
        }

        private string _islandTemperature;
        public string IslandTemperature
        {
            get => _islandTemperature;
            set { _islandTemperature = value; OnPropertyChanged(); }
        }

        // Хранение временных данных
        public TempData TempData { get; set; }

        // Путь к временному файлу temp.json
        private readonly string _tempFilePath;

        // Команды для создания острова и навигации назад
        public ICommand CreateIslandCommand { get; }

        public IslandCreatePageViewModel(TempData tempData)
        {
            TempData = tempData;
            _tempFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "temp.json");

            CreateIslandCommand = new Command(async () => await OnCreateIsland());
        }

        // Логика создания острова
        private async Task OnCreateIsland()
        {
            // Валидация введённых данных
            if (string.IsNullOrWhiteSpace(IslandName) ||
                !double.TryParse(IslandArea, out double area) ||
                !double.TryParse(IslandTemperature, out double temperature))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите корректные данные.", "OK");
                return;
            }

            // Проверка уникальности имени острова
            if (TempData.Islands.Any(i => i.Name == IslandName))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Остров с таким именем уже существует", "OK");
                return;
            }

            // Создание нового острова и добавление его в TempData
            Island newIsland = new Island(IslandName, area, temperature);
            TempData.Islands.Add(newIsland);

            await Application.Current.MainPage.DisplayAlert("Успех!", $"Остров {IslandName} успешно создан.", "OK");

            // Очистка полей ввода
            IslandName = string.Empty;
            IslandArea = string.Empty;
            IslandTemperature = string.Empty;

            SaveTempData();
        }

        // Логика навигации назад
        private async Task OnBack()
        {
            SaveTempData();
            await Shell.Current.GoToAsync("..");
        }

        // Метод для сохранения TempData в файл temp.json
        private void SaveTempData()
        {
            try
            {
                string tempJson = JsonSerializer.Serialize(TempData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_tempFilePath, tempJson);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось сохранить временные данные: {ex.Message}", "OK");
            }
        }
    }
}
