using PlanetLib;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PlanetApp.ViewModels
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }

    public class ViewPlanetViewModel : BindableObject
    {
        private ObservableCollection<Planet> _planets;
        private int _currentIndex;
        private int _sortState; // 0,1,2,3 – состояние сортировки
        private readonly string _filePath;
        private Planet _currentPlanet;

        public Planet CurrentPlanet
        {
            get => _currentPlanet;
            set
            {
                _currentPlanet = value;
                OnPropertyChanged();
            }
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                    PerformSearch(_searchQuery);
                }
            }
        }
        private string _sortButtonSource = "name.png";
        public string SortButtonSource
        {
            get => _sortButtonSource;
            set
            {
                if (_sortButtonSource != value)
                {
                    _sortButtonSource = value;
                    OnPropertyChanged(nameof(SortButtonSource));
                }
            }
        }

        // Команды для переключения планет и сортировки
        public ICommand LeftArrowCommand { get; }
        public ICommand RightArrowCommand { get; }
        public ICommand SortCommand { get; }
        public ICommand DeletePlanetCommand { get; }
        public ICommand EditPlanetCommand { get; }

        public ViewPlanetViewModel()
        {
            _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "planets.json");
            _planets = Planet.LoadPlanetsFromJson(_filePath) ?? new ObservableCollection<Planet>();
            _currentIndex = 0;
            _sortState = 0;
            UpdateCurrentPlanet();

            LeftArrowCommand = new Command(async () => await AnimatePlanetChange("left"));
            RightArrowCommand = new Command(async () => await AnimatePlanetChange("right"));
            SortCommand = new Command(OnSort);
            EditPlanetCommand = new Command(() => OnEditPlanet(this, EventArgs.Empty));
            DeletePlanetCommand = new Command(OnDeletePlanet);
        }

        private void UpdateCurrentPlanet()
        {
            if (_planets == null || _planets.Count == 0)
            {
                // Создаем "пустой" объект, чтобы отобразить сообщение
                CurrentPlanet = new Planet { ImagePath = null, Name = "Ничего не найдено" };
            }
            else
            {
                CurrentPlanet = _planets[_currentIndex];
            }
        }

        private async Task AnimatePlanetChange(string direction)
        {
            // Посылаем сообщение для запуска анимации ухода
            MessagingCenter.Send(this, "AnimatePlanetChangeOut", direction);
            // Ждем завершения анимации ухода
            await Task.Delay(250);

            // Изменяем индекс в зависимости от направления
            if (_planets != null && _planets.Count > 0)
            {
                if (direction == "left")
                    _currentIndex = (_currentIndex - 1 + _planets.Count) % _planets.Count;
                else
                    _currentIndex = (_currentIndex + 1) % _planets.Count;
            }
            UpdateCurrentPlanet();

            // Посылаем сообщение для запуска анимации входа
            MessagingCenter.Send(this, "AnimatePlanetChangeIn", direction);
        }

        public void PerformSearch(string query)
        {
            var allPlanets = Planet.LoadPlanetsFromJson(_filePath) ?? new ObservableCollection<Planet>();

            _planets = string.IsNullOrWhiteSpace(query)
                ? allPlanets
                : allPlanets.Where(p =>
                        p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                        p.Islands.Any(i => i.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                        p.Mainlands.Any(m => m.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                        p.Oceans.Any(o => o.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                        p.Satellites.Any(s => s.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                    ).ToObservableCollection();

            _currentIndex = _planets.Count > 0 ? 0 : -1;
            UpdateCurrentPlanet();
        }

        private void OnSort()
        {
            if (_planets == null || _planets.Count == 0) return;

            switch (_sortState)
            {
                case 0:
                    _planets = _planets.OrderBy(p => p.Name).ToObservableCollection();
                    SortButtonSource = "reversedname.png";
                    _sortState = 1;
                    break;
                case 1:
                    _planets = _planets.OrderByDescending(p => p.Name).ToObservableCollection();
                    SortButtonSource = "number.png";
                    _sortState = 2;
                    break;
                case 2:
                    _planets = _planets.OrderByDescending(p => p.Oceans.Count + p.Mainlands.Count + p.Islands.Count + p.Satellites.Count).ToObservableCollection();
                    SortButtonSource = "reversednumber.png";
                    _sortState = 3;
                    break;
                case 3:
                    _planets = _planets.OrderBy(p => p.Oceans.Count + p.Mainlands.Count + p.Islands.Count + p.Satellites.Count).ToObservableCollection();
                    SortButtonSource = "name.png";
                    _sortState = 0;
                    break;
            }
            _currentIndex = 0;
            UpdateCurrentPlanet();
        }
        private async void OnEditPlanet(object sender, EventArgs e)
        {
            if (CurrentPlanet != null)
                await Application.Current.MainPage.Navigation.PushAsync(new EditPlanet(CurrentPlanet));
        }
        private async void OnDeletePlanet()
        {
            if (_planets.Count == 0) return;
            bool isConfirmed = await Application.Current.MainPage.DisplayAlert("Подтверждение",
                "Вы уверены, что хотите удалить эту планету?",
                "Удалить",
                "Отмена");


            if (!isConfirmed)
            {
                await Application.Current.MainPage.DisplayAlert("Отмена", "Удаление отменено.", "Ок");
                return;
            }
            // Логика удаления планеты из списка

            var planetToDelete = _planets[_currentIndex];
            _planets.Remove(planetToDelete);

            // Сохранение изменений в файл
            Planet.SavePlanetsToJson(_planets, _filePath);

            // Обновление текущего индекса
            _currentIndex = 0;
            // Отправка сообщения для запуска анимации удаления
            MessagingCenter.Send(this, "DeletePlanet");
            await Task.Delay(2000);

            //Обновление планеты
            UpdateCurrentPlanet();

            await Application.Current.MainPage.DisplayAlert("Успех", "Планета успешно удалена.", "Ок");
        }
    }
}
