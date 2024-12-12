using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using PlanetLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PlanetApp
{
    public partial class EditPlanet : ContentPage
    {
        private Planet _planet;

        public EditPlanet(Planet planet)
        {
            InitializeComponent();
            _planet = planet;

            // Инициализируем поля названия планеты
            PlanetNameEntry.Text = _planet.Name;

            Planet.PlanetsSaved += OnPlanetsSaved;


            // Инициализируем списки островов, материков, океанов и спутников
            IslandCollectionView.ItemsSource = _planet.Islands;
            MainlandCollectionView.ItemsSource = _planet.Mainlands;
            OceanCollectionView.ItemsSource = _planet.Oceans;
            SatelliteCollectionView.ItemsSource = _planet.satellites;
        }



    private async void OnPlanetsSaved(object? sender, bool success)
    {
        if (success)
        {
            await DisplayAlert("Успех", "Планеты успешно сохранены.", "OK");
        }
        else
        {
            await DisplayAlert("Ошибка", "Не удалось сохранить планеты.", "OK");
        }
    }

        // Обработчик выбора острова
        private void OnIslandSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Island selectedIsland)
            {
                IslandNameEntry.Text = selectedIsland.Name;
                IslandAreaEntry.Text = selectedIsland.Area.ToString();
                IslandTemperatureEntry.Text = selectedIsland.AverageTemperature.ToString();
            }
        }

        // Обработчик выбора материка
        private void OnMainlandSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Mainland selectedMainland)
            {
                MainlandNameEntry.Text = selectedMainland.Name;
                MainlandAreaEntry.Text = selectedMainland.Area.ToString();
                MainlandTemperatureEntry.Text = selectedMainland.AverageTemperature.ToString();
            }
        }

        // Обработчик выбора океана
        private void OnOceanSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Ocean selectedOcean)
            {
                OceanNameEntry.Text = selectedOcean.Name;
                OceanAreaEntry.Text = selectedOcean.Area.ToString();
                OceanTemperatureEntry.Text = selectedOcean.AverageTemperature.ToString();
            }
        }

        // Обработчик выбора спутника
        private void OnSatelliteSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Satellite selectedSatellite)
            {
                SatelliteNameEntry.Text = selectedSatellite.Name;
                SatelliteMassEntry.Text = selectedSatellite.Mass.ToString();
            }
        }

        // Сохранение изменений
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "planets.json");

            // Загружаем список планет из файла
            List<Planet> planets = Planet.LoadPlanetsFromJson(filePath);

            // Находим соответствующую планету в списке
            var foundPlanet = planets.FirstOrDefault(p => p.Equals(_planet));

            if (foundPlanet != null)
            {
                // Обновляем данные планеты
                foundPlanet.Name = PlanetNameEntry.Text;

                foundPlanet.Oceans = _planet.Oceans;
                foundPlanet.Islands =_planet.Islands;
                foundPlanet.Mainlands = _planet.Mainlands;
                foundPlanet.satellites = _planet.satellites;

                // Обновляем данные выбранного острова
                if (IslandCollectionView.SelectedItem is Island selectedIsland)
                {
                    var foundIsland = foundPlanet.Islands.FirstOrDefault(i => i.Equals(selectedIsland));
                    if (foundIsland != null)
                    {
                        foundIsland.Name = IslandNameEntry.Text;
                        if (double.TryParse(IslandAreaEntry.Text, out double islandArea))
                            foundIsland.Area = islandArea;
                        if (double.TryParse(IslandTemperatureEntry.Text, out double islandTemp))
                            foundIsland.AverageTemperature = islandTemp;
                    }
                }

                // Обновляем данные выбранного материка
                if (MainlandCollectionView.SelectedItem is Mainland selectedMainland)
                {
                    var foundMainland = foundPlanet.Mainlands.FirstOrDefault(m => m.Equals(selectedMainland));
                    if (foundMainland != null)
                    {
                        foundMainland.Name = MainlandNameEntry.Text;
                        if (double.TryParse(MainlandAreaEntry.Text, out double mainlandArea))
                            foundMainland.Area = mainlandArea;
                        if (double.TryParse(MainlandTemperatureEntry.Text, out double mainlandTemp))
                            foundMainland.AverageTemperature = mainlandTemp;
                    }
                }

                // Обновляем данные выбранного океана
                if (OceanCollectionView.SelectedItem is Ocean selectedOcean)
                {
                    var foundOcean = foundPlanet.Oceans.FirstOrDefault(o => o.Equals(selectedOcean));
                    if (foundOcean != null)
                    {
                        foundOcean.Name = OceanNameEntry.Text;
                        if (double.TryParse(OceanAreaEntry.Text, out double oceanArea))
                            foundOcean.Area = oceanArea;
                        if (double.TryParse(OceanTemperatureEntry.Text, out double oceanTemperature))
                            foundOcean.AverageTemperature = oceanTemperature;
                    }
                }

                // Обновляем данные выбранного спутника
                if (SatelliteCollectionView.SelectedItem is Satellite selectedSatellite)
                {
                    int satelliteIndex = Array.IndexOf(foundPlanet.satellites, selectedSatellite);
                    if (satelliteIndex >= 0)
                    {
                        // Используем индексатор для обновления данных спутника
                        var satelliteToUpdate = foundPlanet.satellites[satelliteIndex];
                        satelliteToUpdate.Name = SatelliteNameEntry.Text;

                        if (int.TryParse(SatelliteMassEntry.Text, out int satelliteMass))
                            satelliteToUpdate.Mass = satelliteMass;
                    }
                }
            }
            else
            {
                // Если планета не найдена, выводим сообщение об ошибке
                await DisplayAlert("Ошибка", "Планета не найдена.", "OK");
                return;
            }

            // Сохраняем изменения обратно в файл JSON
            Planet.SavePlanetsToJson(planets,filePath);

            Planet.PlanetsSaved -= OnPlanetsSaved;

            // Возвращаемся к просмотру планет
            await Navigation.PushAsync(new ViewPlanet());
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;

            switch (button?.ClassId)
            {
                case "Island":
                    var islandsList = _planet.Islands.ToList();
                    DeleteSelectedItem(ref islandsList, IslandCollectionView, "Выберите остров для удаления.");
                    IslandNameEntry.Text = string.Empty;
                    IslandAreaEntry.Text = string.Empty;
                    IslandTemperatureEntry.Text = string.Empty;
                    IslandCollectionView.SelectedItem = null;
                    _planet.Islands = islandsList; // Обновляем ссылку
                    break;

                case "Mainland":
                    var mainlandsList = _planet.Mainlands.ToList();
                    DeleteSelectedItem(ref mainlandsList, MainlandCollectionView, "Выберите материк для удаления.");
                    MainlandNameEntry.Text = string.Empty;
                    MainlandAreaEntry.Text = string.Empty;
                    MainlandTemperatureEntry.Text = string.Empty;
                    MainlandCollectionView.SelectedItem = null;
                    _planet.Mainlands = mainlandsList;
                    break;

                case "Ocean":
                    var oceansList = _planet.Oceans.ToList();
                    DeleteSelectedItem(ref oceansList, OceanCollectionView, "Выберите океан для удаления.");
                    OceanNameEntry.Text = string.Empty;
                    OceanAreaEntry.Text = string.Empty;
                    OceanTemperatureEntry.Text = string.Empty;
                    OceanCollectionView.SelectedItem = null;
                    _planet.Oceans = oceansList;
                    break;

                case "Satellite":
                    var satellitesList = _planet.satellites.ToList();
                    DeleteSelectedItem(ref satellitesList, SatelliteCollectionView, "Выберите спутник для удаления.");
                    SatelliteNameEntry.Text = string.Empty;
                    SatelliteMassEntry.Text = string.Empty;
                    SatelliteCollectionView.SelectedItem = null;
                    var temp = new Satellite[satellitesList.Count+1];
                    satellitesList.CopyTo(temp);
                    _planet.satellites = temp;
                    break;

                default:
                    DisplayAlert("Ошибка", "Неизвестный тип для удаления.", "OK");
                    break;
            }
        }

        private void DeleteSelectedItem<T>(ref List<T> collection, CollectionView collectionView, string errorMessage) where T : class
        {       
            if (collectionView.SelectedItem is T selectedItem)
            {
                collection.Remove(selectedItem);
                DisplayAlert("Успех!","Часть была удалена!", "OK");
        
            // Обновляем ItemsSource
                collectionView.ItemsSource = null;
                collectionView.ItemsSource = collection;
            }
            else
            {
                DisplayAlert("Ошибка", "Не удалось удалить", "OK");
            }
        }
        private void OnAddSatelliteClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SatelliteNameEntry.Text))
            {
                DisplayAlert("Ошибка", "Название не может быть пустым", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(SatelliteMassEntry.Text) || !double.TryParse(SatelliteMassEntry.Text, out double satelliteMass))
            {
                DisplayAlert("Ошибка", "Введите корректную массу", "OK");
                return;
            }
            if (_planet.satellites.FirstOrDefault(i => i != null && i.Name == SatelliteNameEntry.Text) != null)
            {
                // Спутник с таким именем уже существует
                DisplayAlert("Ошибка", "Спутник с таким именем уже существует", "OK");
                return;
            }

            for (int i = 0; i < _planet.satellites.Length; i++)
            {
                if (_planet.satellites[i] == null) // Проверяем, есть ли свободное место
                {
                    _planet.satellites[i] = new Satellite(SatelliteNameEntry.Text, satelliteMass);
                    SatelliteCollectionView.ItemsSource = null;
                    SatelliteCollectionView.SelectedItem = null;
                    SatelliteCollectionView.ItemsSource = _planet.satellites;
                    return;
                }
            }
            DisplayAlert("Ошибка", "Нет места для нового спутника", "OK");
            return;
        }
        private void OnAddMainLandClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MainlandNameEntry.Text))
            {
                DisplayAlert("Ошибка", "Название не может быть пустым", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(MainlandAreaEntry.Text) || !double.TryParse(MainlandAreaEntry.Text, out double MainlandArea))
            {
                DisplayAlert("Ошибка", "Введите корректную площадь", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(MainlandTemperatureEntry.Text) || !double.TryParse(MainlandTemperatureEntry.Text, out double MainlandTemp))
            {
                DisplayAlert("Ошибка", "Введите корректную температуру", "OK");
                return;
            }
            if (_planet.Mainlands.FirstOrDefault(i => i != null && i.Name == MainlandNameEntry.Text) != null)
            {
                // Спутник с таким именем уже существует
                DisplayAlert("Ошибка", "Материк с таким именем уже существует", "OK");
                return;
            }
            _planet.Mainlands.Add(new Mainland(MainlandNameEntry.Text,MainlandArea, MainlandTemp));
            MainlandCollectionView.ItemsSource = null;
            MainlandCollectionView.SelectedItem = null;
            MainlandCollectionView.ItemsSource = _planet.Mainlands;
            DisplayAlert("Успех!", "Новый материк добавлен", "OK");
            return;
        }
        private void OnAddOceanClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OceanNameEntry.Text))
            {
                DisplayAlert("Ошибка", "Название не может быть пустым", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(OceanAreaEntry.Text) || !double.TryParse(OceanAreaEntry.Text, out double OceanArea))
            {
                DisplayAlert("Ошибка", "Введите корректную площадь", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(OceanTemperatureEntry.Text) || !double.TryParse(OceanTemperatureEntry.Text, out double OceanTemp))
            {
                DisplayAlert("Ошибка", "Введите корректную температуру", "OK");
                return;
            }
            if (_planet.Oceans.FirstOrDefault(i => i != null && i.Name == OceanNameEntry.Text) != null)
            {
                // Спутник с таким именем уже существует
                DisplayAlert("Ошибка", "Океан с таким именем уже существует", "OK");
                return;
            }
            _planet.Oceans.Add(new Ocean(OceanNameEntry.Text,OceanArea, OceanTemp));
            OceanCollectionView.ItemsSource = null;
            OceanCollectionView.SelectedItem = null;
            OceanCollectionView.ItemsSource = _planet.Oceans;
            DisplayAlert("Успех!", "Новый океан добавлен", "OK");
            return;
        }
        private void OnAddIslandClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(IslandNameEntry.Text))
            {
                DisplayAlert("Ошибка", "Название не может быть пустым", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(IslandAreaEntry.Text) || !double.TryParse(IslandAreaEntry.Text, out double IslandArea))
            {
                DisplayAlert("Ошибка", "Введите корректную площадь", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(IslandTemperatureEntry.Text) || !double.TryParse(IslandTemperatureEntry.Text, out double IslandTemp))
            {
                DisplayAlert("Ошибка", "Введите корректную площадь", "OK");
                return;
            }
            _planet.Islands.Add(new Island(IslandNameEntry.Text,IslandArea, IslandTemp));
            IslandCollectionView.ItemsSource = null;
            IslandCollectionView.SelectedItem = null;
            IslandCollectionView.ItemsSource = _planet.Islands;
            DisplayAlert("Успех!", "Новый остров добавлен", "OK");
            return;
        }
    }    
}
