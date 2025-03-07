using System.Windows.Input;
using System.Text.Json;
using PlanetLib;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.ObjectModel;
namespace PlanetApp.ViewModels
{
    public class CreatePlanetViewModel : BindableObject, INotifyPropertyChanged
    {
        // Константы для файлов
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        private const string JsonFileName = "planets.json";
        private const string TempFileName = "temp.json";

        // Пути к файлам
        private readonly string _filePath;
        private readonly string _tempFilePath;

        // Хранение временных данных
        private PlanetApp.TempData _tempData;

        // Свойства для привязки (например, имя планеты и количество спутников)
        private string _planetName;
        public string PlanetName
        {
            get => _planetName;
            set { _planetName = value; OnPropertyChanged(); }
        }

        private string _satelliteCount;
        public string SatelliteCount
        {
            get => _satelliteCount;
            set { _satelliteCount = value; OnPropertyChanged(); }
        }

        // Команды
        public ICommand SavePlanetCommand { get; }
        public ICommand NavigateToIslandCommand { get; }
        public ICommand NavigateToMainlandCommand { get; }
        public ICommand NavigateToOceanCommand { get; }

        public CreatePlanetViewModel()
        {
            // Инициализация путей
            _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), JsonFileName);
            _tempFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), TempFileName);

            _tempData = new TempData();
            LoadTempData();

            // Инициализация команд
            SavePlanetCommand = new Command(async () => await SavePlanet());
            // Навигация будет осуществляться через события/сервисы навигации (например, через MessagingCenter или внедрение зависимостей)
            NavigateToIslandCommand = new Command(() => SaveTempDataAndNavigate("Island"));
            NavigateToMainlandCommand = new Command(() => SaveTempDataAndNavigate("Mainland"));
            NavigateToOceanCommand = new Command(() => SaveTempDataAndNavigate("Ocean"));
        }

        private void SaveTempData()
        {
            string json = JsonSerializer.Serialize(_tempData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_tempFilePath, json);

        }

        // Очистка временных данных
        private void ClearTempData()
        {
            _tempData.Islands.Clear();
            _tempData.Mainlands.Clear();
            _tempData.Oceans.Clear();
            SaveTempData();
        }

        private void LoadTempData()
        {
            try
            {
                string json = File.ReadAllText(_tempFilePath);
                _tempData = JsonSerializer.Deserialize<TempData>(json) ?? new TempData(); // Десериализуем временные данные
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось загрузить временные данные: {ex.Message}", "OK");
            }
        }

        private async Task SavePlanet()
        {

            Planet newPlanet;

            if (string.IsNullOrWhiteSpace(SatelliteCount))
            {
                if (string.IsNullOrWhiteSpace(PlanetName) && _tempData.Islands.Count == 0 && _tempData.Mainlands.Count == 0 && _tempData.Oceans.Count == 0)
                {
                    newPlanet = new Planet();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(PlanetName))
                    {
                        newPlanet = new Planet("NoName")
                        {
                            Islands = _tempData.Islands ?? new ObservableCollection<Island>(),
                            Mainlands = _tempData.Mainlands ?? new ObservableCollection<Mainland>(),
                            Oceans = _tempData.Oceans ?? new ObservableCollection<Ocean>(),
                        };
                    }
                    else
                    {
                        newPlanet = new Planet(PlanetName)
                        {
                            Islands = _tempData.Islands ?? new ObservableCollection<Island>(),
                            Mainlands = _tempData.Mainlands ?? new ObservableCollection<Mainland>(),
                            Oceans = _tempData.Oceans ?? new ObservableCollection<Ocean>()
                        };
                    }

                }
            }
            else
            {
                int satelliteCount;
                int.TryParse(SatelliteCount, out satelliteCount);

                if (string.IsNullOrWhiteSpace(PlanetName))
                {
                    PlanetName = "NoName";
                }

                newPlanet = new Planet(PlanetName, satelliteCount)
                {
                    Islands = new ObservableCollection<Island>(_tempData.Islands),
                    Mainlands = new ObservableCollection<Mainland>(_tempData.Mainlands),
                    Oceans = new ObservableCollection<Ocean>(_tempData.Oceans)
                };
            }

            // Загрузка существующих планет из файла
            ObservableCollection<Planet> planets = Planet.LoadPlanetsFromJson(_filePath);

            planets.Add(newPlanet);
            Planet.SavePlanetsToJson(planets, _filePath);

            // Здесь можно уведомить пользователя об успехе, опять же через событие или свойство для привязки
            ClearTempData();
            Application.Current.MainPage.DisplayAlert("Успех!", "Планета создана", "ОК");
            SatelliteCount = string.Empty;
            PlanetName = string.Empty;
        }

        private async void SaveTempDataAndNavigate(string destination)
        {

            // Сначала сохраняем временные данные в файл
            SaveTempData();

            await Application.Current.MainPage.Navigation.PushAsync(new CreatePlanetPart(_tempData, destination));
        }

    }
}