using System.Windows.Input;
using PlanetLib;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using PlanetApp.Services;
using System.Collections.ObjectModel;
namespace PlanetApp.ViewModels
{
    public class EditPlanetViewModel : BindableObject, INotifyPropertyChanged
    {
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }


        private string _planetNameEntry;
        private Planet _planet;
        private string _islandNameEntry;
        private string _islandAreaEntry;

        private string _islandTemperatureEntry;

        private ObservableCollection<Satellite> _satellites;
        private string _satelliteNameEntry;
        private string _satelliteMassEntry;
        private Ocean _selectedOcean;
        public Ocean SelectedOcean
        {
            get => _selectedOcean;
            set
            {
                if (_selectedOcean != value)
                {

                    _selectedOcean = value;
                    OnPropertyChanged(nameof(SelectedOcean));

                    if (_selectedIsland != null)
                    {
                        OceanNameEntry = _selectedOcean.Name;
                        OceanAreaEntry = _selectedOcean.Area.ToString();
                        OceanTemperatureEntry = _selectedOcean.AverageTemperature.ToString();
                        OnPropertyChanged();
                    }
                }
            }
        }
        private Mainland _selectedMainland;
        public Mainland SelectedMainland
        {
            get => _selectedMainland;
            set
            {
                if (_selectedMainland != value)
                {

                    _selectedMainland = value;
                    OnPropertyChanged(nameof(SelectedMainland));

                    if (_selectedIsland != null)
                    {
                        MainlandNameEntry = _selectedMainland.Name;
                        MainlandAreaEntry = _selectedMainland.Area.ToString();
                        MainlandTemperatureEntry = _selectedMainland.AverageTemperature.ToString();
                        OnPropertyChanged();
                    }
                }
            }
        }
        private Satellite _selectedSatellite;
        public Satellite SelectedSatellite
        {
            get => _selectedSatellite;
            set
            {
                if (_selectedSatellite != value)
                {

                    _selectedSatellite = value;
                    OnPropertyChanged(nameof(SelectedSatellite));

                    if (_selectedSatellite != null)
                    {
                        SatelliteNameEntry = _selectedSatellite.Name;
                        SatelliteMassEntry = _selectedSatellite.Mass.ToString();
                        OnPropertyChanged();
                    }
                }
            }
        }
        public IAlertService AlertService { get; set; }
        private Island _selectedIsland;
        public Island SelectedIsland
        {
            get => _selectedIsland;
            set
            {
                if (_selectedIsland != value)
                {

                    _selectedIsland = value;
                    OnPropertyChanged(nameof(SelectedIsland));

                    if (_selectedIsland != null)
                    {
                        IslandNameEntry = _selectedIsland.Name;
                        IslandAreaEntry = _selectedIsland.Area.ToString();
                        IslandTemperatureEntry = _selectedIsland.AverageTemperature.ToString();
                        OnPropertyChanged();
                    }
                }
            }
        }


        public string PlanetNameEntry
        {
            get => _planetNameEntry;

            set
            {
                _planetNameEntry = value;
                OnPropertyChanged(PlanetNameEntry);
            }

        }

        public ObservableCollection<Island> Islands { get; set; }
        public string IslandNameEntry
        {
            get => _islandNameEntry;
            set
            {

                _islandNameEntry = value;
                OnPropertyChanged(nameof(IslandNameEntry));
            }
        }
        public string IslandTemperatureEntry
        {
            get => _islandTemperatureEntry;
            set
            {
                _islandTemperatureEntry = value;
                OnPropertyChanged(nameof(IslandTemperatureEntry));
            }
        }
        public string IslandAreaEntry
        {
            get => _islandAreaEntry; set
            {
                _islandAreaEntry = value;
                OnPropertyChanged(nameof(IslandAreaEntry));
            }
        }
        private ObservableCollection<Ocean> _oceans;
        private string _oceanNameEntry;
        private string _oceanAreaEntry;
        private string _oceanTemperatureEntry;

        public ObservableCollection<Ocean> Oceans
        {
            get => _oceans;
            set
            {
                _oceans = value;
                OnPropertyChanged(nameof(Oceans));
            }
        }
        public string OceanNameEntry
        {
            get => _oceanNameEntry;
            set
            {
                _oceanNameEntry = value;
                OnPropertyChanged(nameof(OceanNameEntry));
            }
        }
        public string OceanAreaEntry
        {
            get => _oceanAreaEntry;
            set
            {

                _oceanAreaEntry = value;
                OnPropertyChanged(nameof(OceanAreaEntry));
            }
        }
        public string OceanTemperatureEntry
        {
            get => _oceanTemperatureEntry;
            set
            {
                _oceanTemperatureEntry = value;
                OnPropertyChanged(nameof(OceanTemperatureEntry));

            }
        }

        private ObservableCollection<Mainland> _mainlands;
        private string _mainlandNameEntry;
        private string _mainlandAreaEntry;
        private string _mainlandTemperatureEntry;

        public ObservableCollection<Mainland> Mainlands
        {
            get => _mainlands;
            set { _mainlands = value; }
        }
        public string MainlandNameEntry
        {
            get => _mainlandNameEntry;
            set
            {
                if (_mainlandNameEntry != value)
                {
                    _mainlandNameEntry = value;
                    OnPropertyChanged(nameof(MainlandNameEntry));
                }
            }
        }
        public string MainlandAreaEntry
        {
            get => _mainlandAreaEntry;
            set
            {

                _mainlandAreaEntry = value;
                OnPropertyChanged(nameof(MainlandAreaEntry));
            }
        }
        public string MainlandTemperatureEntry
        {
            get => _mainlandTemperatureEntry;
            set
            {
                _mainlandTemperatureEntry = value;
                OnPropertyChanged(nameof(MainlandTemperatureEntry));

            }

        }

        public ObservableCollection<Satellite> Satellites
        {
            get => _satellites;
            set
            {
                _satellites = value;
            }
        }

        public string SatelliteNameEntry
        {
            get => _satelliteNameEntry;
            set
            {

                _satelliteNameEntry = value;
                OnPropertyChanged(nameof(SatelliteNameEntry));

            }
        }
        public string SatelliteMassEntry
        {
            get => _satelliteMassEntry;
            set
            {

                _satelliteMassEntry = value;
                OnPropertyChanged(nameof(SatelliteMassEntry));

            }
        }
        public ICommand OnMainlandSelectedCommand { get; }
        public ICommand OnOceanSelectedCommand { get; }
        public ICommand OnIslandSelectedCommand { get; }
        public ICommand OnSatelliteSelectedCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddSatelliteCommand { get; }
        public ICommand AddMainlandCommand { get; }
        public ICommand AddOceanCommand { get; }
        public ICommand AddIslandCommand { get; }

        public EditPlanetViewModel(Planet planet)
        {
            _planet = planet;
            PlanetNameEntry = _planet.Name;
            Islands = _planet.Islands;
            Oceans = _planet.Oceans;
            Mainlands = _planet.Mainlands;
            Satellites = _planet.Satellites;

            OnIslandSelectedCommand = new Command<SelectionChangedEventArgs>(OnIsland);
            OnMainlandSelectedCommand = new Command<SelectionChangedEventArgs>(OnMainland);
            OnOceanSelectedCommand = new Command<SelectionChangedEventArgs>(OnOcean);
            OnSatelliteSelectedCommand = new Command<SelectionChangedEventArgs>(OnSatellite);
            SaveCommand = new Command(async () => await OnSaveClicked());
            DeleteCommand = new Command<string>(OnDeleteItem);
            AddSatelliteCommand = new Command(async () => await OnAddSatellite());
            AddMainlandCommand = new Command(async () => await OnAddMainland());
            AddOceanCommand = new Command(async () => await OnAddOcean());
            AddIslandCommand = new Command(async () => await OnAddIsland());
        }

        private void ClearFields()
        {
            IslandNameEntry = string.Empty;
            IslandAreaEntry = string.Empty;
            IslandTemperatureEntry = string.Empty;

            MainlandNameEntry = string.Empty;
            MainlandAreaEntry = string.Empty;
            MainlandTemperatureEntry = string.Empty;


            OceanNameEntry = string.Empty;
            OceanAreaEntry = string.Empty;
            OceanTemperatureEntry = string.Empty;


            SatelliteNameEntry = string.Empty;
            SatelliteMassEntry = string.Empty;

        }

        private void OnIsland(SelectionChangedEventArgs e)
        {
            var selectedIsland = SelectedIsland;
            IslandNameEntry = selectedIsland.Name;
            IslandAreaEntry = selectedIsland.Area.ToString();
            IslandTemperatureEntry = selectedIsland.AverageTemperature.ToString();

        }
        private void OnOcean(SelectionChangedEventArgs e)
        {
            ClearFields();
            var selectedOcean = SelectedOcean;
            OceanNameEntry = selectedOcean.Name;
            OceanAreaEntry = selectedOcean.Area.ToString();
            OceanTemperatureEntry = selectedOcean.AverageTemperature.ToString();

        }

        private void OnMainland(SelectionChangedEventArgs e)
        {
            var selectedMainland = SelectedMainland;
            MainlandNameEntry = selectedMainland.Name;
            MainlandAreaEntry = selectedMainland.Area.ToString();
            MainlandTemperatureEntry = selectedMainland.AverageTemperature.ToString();

        }
        private void OnSatellite(SelectionChangedEventArgs e)
        {
            var selectedSatellite = SelectedSatellite;
            SatelliteNameEntry = selectedSatellite.Name;
            SatelliteMassEntry = selectedSatellite.Mass.ToString();

        }


        private async Task OnSaveClicked()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "planets.json");

            // Загружаем список планет из файла
            ObservableCollection<Planet> planets = Planet.LoadPlanetsFromJson(filePath);

            // Находим соответствующую планету
            var foundPlanet = planets.FirstOrDefault(p => p.Equals(_planet));

            if (foundPlanet != null)
            {
                // Обновляем данные планеты
                foundPlanet.Name = PlanetNameEntry;

                // Обновляем коллекции (если они редактируются через UI, то они уже обновлены в ViewModel)
                foundPlanet.Islands = Islands;
                foundPlanet.Oceans = Oceans;
                foundPlanet.Mainlands = Mainlands;
                foundPlanet.Satellites = Satellites;

                // Обновляем данные выбранного острова
                if (SelectedIsland != null)
                {
                    var foundIsland = foundPlanet.Islands.FirstOrDefault(i => i.Equals(SelectedIsland));
                    if (foundIsland != null)
                    {
                        foundIsland.Name = IslandNameEntry;
                        foundIsland.Area = double.Parse(IslandAreaEntry);
                        foundIsland.AverageTemperature = double.Parse(IslandTemperatureEntry);
                    }
                }

                // Обновляем данные выбранного материка
                if (SelectedMainland != null)
                {
                    var foundMainland = foundPlanet.Mainlands.FirstOrDefault(m => m.Equals(SelectedMainland));
                    if (foundMainland != null)
                    {
                        foundMainland.Name = MainlandNameEntry;
                        foundMainland.Area = double.Parse(MainlandAreaEntry);
                        foundMainland.AverageTemperature = double.Parse(MainlandTemperatureEntry);
                    }
                }

                // Обновляем данные выбранного океана
                if (SelectedOcean != null)
                {
                    var foundOcean = foundPlanet.Oceans.FirstOrDefault(o => o.Equals(SelectedOcean));
                    if (foundOcean != null)
                    {
                        foundOcean.Name = OceanNameEntry;
                        foundOcean.Area = double.Parse(OceanAreaEntry);
                        foundOcean.AverageTemperature = double.Parse(OceanTemperatureEntry);
                    }
                }

                // Обновляем данные выбранного спутника
                if (SelectedSatellite != null)
                {
                    var foundSatellite = foundPlanet.Satellites.FirstOrDefault(o => o.Equals(SelectedSatellite));
                    if (foundSatellite != null)
                    {
                        foundSatellite.Name = SatelliteNameEntry;
                        foundSatellite.Mass = double.Parse(SatelliteMassEntry);

                    }
                }

                // Сохраняем обновлённые данные в JSON-файл
                Planet.SavePlanetsToJson(planets, filePath);

                await Application.Current.MainPage.Navigation.PushAsync(new ViewPlanet());
            }
        }

        private async void OnDeleteItem(string itemType)
        {
            if (SelectedIsland == null && SelectedSatellite == null && SelectedOcean == null && SelectedMainland == null)
            {
                await Application.Current.MainPage.DisplayAlert("Не выбран предмет для удаления", "Неудача!", "OK");
                return;
            }
            switch (itemType)
            {
                case "Island":
                    if (SelectedIsland != null)
                    {
                        Islands.Remove(SelectedIsland);
                        _planet.Islands = Islands;
                        await Application.Current.MainPage.DisplayAlert("Остров удален", "Успех!", "OK");
                    }

                    break;

                case "Mainland":
                    if (SelectedMainland != null)
                    {
                        Mainlands.Remove(SelectedMainland);
                        _planet.Mainlands = Mainlands;
                        await Application.Current.MainPage.DisplayAlert("Материк удален", "Успех!", "OK");
                    }

                    break;

                case "Ocean":
                    if (SelectedOcean != null)
                    {
                        Oceans.Remove(SelectedOcean);
                        _planet.Oceans = Oceans;
                        await Application.Current.MainPage.DisplayAlert("Океан удален", "Успех!", "OK");

                    }
                    break;

                case "Satellite":
                    if (SelectedSatellite != null)
                    {
                        Satellites.Remove(SelectedSatellite);
                        _planet.Satellites = Satellites;
                        await Application.Current.MainPage.DisplayAlert("Спутник удален", "Успех!", "OK");
                    }
                    break;
            }


            // Уведомляем UI об изменениях коллекций (если требуется)
            ClearFields();
            OnPropertyChanged(nameof(Islands));
            OnPropertyChanged(nameof(Mainlands));
            OnPropertyChanged(nameof(Oceans));
            OnPropertyChanged(nameof(Satellites));
        }

        private async Task OnAddSatellite()
        {
            SelectedSatellite = null;
            // Проверка ввода имени спутника
            if (string.IsNullOrWhiteSpace(SatelliteNameEntry))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Название не может быть пустым", "OK");
                return;
            }
            // Проверка ввода массы спутника
            if (string.IsNullOrWhiteSpace(SatelliteMassEntry) || !double.TryParse(SatelliteMassEntry, out double satelliteMass))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Введите корректную массу", "OK");
                return;
            }
            // Проверяем, существует ли спутник с таким именем
            if (_planet.Satellites.FirstOrDefault(s => s != null && s.Name == SatelliteNameEntry) != null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Спутник с таким именем уже существует", "OK");
                return;
            }
            Satellites.Add(new Satellite(SatelliteNameEntry, double.Parse(SatelliteMassEntry)));
            await Application.Current.MainPage.DisplayAlert("Успех!", "Новый спутник добавлен", "OK");

            // Очистка полей ввода
            OnPropertyChanged(nameof(SatelliteNameEntry));
            OnPropertyChanged(nameof(SatelliteMassEntry));
            OnPropertyChanged(nameof(SelectedSatellite));
            ClearFields();
        }

        private async Task OnAddMainland()
        {
            SelectedMainland = null;
            if (string.IsNullOrWhiteSpace(MainlandNameEntry))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Название не может быть пустым", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(MainlandAreaEntry) || !double.TryParse(MainlandAreaEntry, out double mainlandArea))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Введите корректную площадь", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(MainlandTemperatureEntry) || !double.TryParse(MainlandTemperatureEntry, out double mainlandTemp))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Введите корректную температуру", "OK");
                return;
            }
            if (Mainlands.FirstOrDefault(m => m != null && m.Name == MainlandNameEntry) != null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Материк с таким именем уже существует", "OK");
                return;
            }
            Mainlands.Add(new Mainland(MainlandNameEntry, mainlandArea, mainlandTemp));
            await Application.Current.MainPage.DisplayAlert("Успех!", "Новый материк добавлен", "OK");
            // Очистка полей ввода

            OnPropertyChanged(nameof(MainlandNameEntry));
            OnPropertyChanged(nameof(MainlandAreaEntry));
            OnPropertyChanged(nameof(MainlandTemperatureEntry));
            OnPropertyChanged(nameof(SelectedMainland));
            ClearFields();
        }

        private async Task OnAddOcean()
        {
            SelectedOcean = null;
            if (string.IsNullOrWhiteSpace(OceanNameEntry))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Название не может быть пустым", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(OceanAreaEntry) || !double.TryParse(OceanAreaEntry, out double oceanArea))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Введите корректную площадь", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(OceanTemperatureEntry) || !double.TryParse(OceanTemperatureEntry, out double oceanTemp))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Введите корректную температуру", "OK");
                return;
            }
            if (Oceans.FirstOrDefault(o => o != null && o.Name == OceanNameEntry) != null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Океан с таким именем уже существует", "OK");
                return;
            }
            Oceans.Add(new Ocean(OceanNameEntry, oceanArea, oceanTemp));
            OnPropertyChanged(nameof(Oceans));
            await Application.Current.MainPage.DisplayAlert("Успех!", "Новый океан добавлен", "OK");
            // Очистка полей ввода
            OnPropertyChanged(nameof(OceanNameEntry));
            OnPropertyChanged(nameof(OceanAreaEntry));
            OnPropertyChanged(nameof(OceanTemperatureEntry));
            OnPropertyChanged(nameof(SelectedOcean));
            ClearFields();
        }

        private async Task OnAddIsland()
        {
            SelectedIsland = null;
            if (string.IsNullOrWhiteSpace(IslandNameEntry))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Название не может быть пустым", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(IslandAreaEntry) || !double.TryParse(IslandAreaEntry, out double islandArea))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Введите корректную площадь", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(IslandTemperatureEntry) || !double.TryParse(IslandTemperatureEntry, out double islandTemp))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Введите корректную температуру", "OK");
                return;
            }
            if (Islands.FirstOrDefault(i => i != null && i.Name == IslandNameEntry) != null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Остров с таким именем уже существует", "OK");
                return;
            }
            Islands.Add(new Island(IslandNameEntry, islandArea, islandTemp));
            OnPropertyChanged(nameof(Islands));
            await Application.Current.MainPage.DisplayAlert("Успех!", "Новый остров добавлен", "OK");
            // Очистка полей ввода
            OnPropertyChanged(nameof(IslandNameEntry));
            OnPropertyChanged(nameof(IslandAreaEntry));
            OnPropertyChanged(nameof(IslandTemperatureEntry));
            OnPropertyChanged(nameof(SelectedIsland));
            ClearFields();
        }
    }
}
