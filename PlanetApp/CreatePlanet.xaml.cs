using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.IO;
using System.Collections.Generic;
using System.Text.Json; // Добавлено для работы с JSON
using System;
using PlanetLib;
using System.Collections.ObjectModel;

namespace PlanetApp
{
    public partial class CreatePlanet : ContentPage
    {
        private const string JsonFileName = "planets.json"; // Имя файла для сохранения планет
        private const string TempFileName = "temp.json"; // Имя временного файла для данных TempData
        private string _filePath;
        private string _tempFilePath;
        private TempData tempData = new TempData(); // Инициализация объекта TempData

        public CreatePlanet()
        {
            InitializeComponent();
            _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), JsonFileName);
            _tempFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), TempFileName);
            LoadTempData(); // Загружаем временные данные
        }

        // Метод загрузки временных данных из файла temp.json
        private void LoadTempData()
        {
            try
            {
                string json = File.ReadAllText(_tempFilePath);
                tempData = JsonSerializer.Deserialize<TempData>(json) ?? new TempData(); // Десериализуем временные данные
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", $"Не удалось загрузить временные данные: {ex.Message}", "OK");
            }
        }

        // Метод сохранения временных данных в temp.json
        private void SaveTempData()
        {
            try
            {
                string json = JsonSerializer.Serialize(tempData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_tempFilePath, json); // Сохраняем временные данные в файл
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", $"Не удалось сохранить временные данные: {ex.Message}", "OK");
            }
        }

        // Очистка временных данных и файла temp.json
        private void ClearTempData()
        {
            tempData.Islands.Clear();
            tempData.Mainlands.Clear();
            tempData.Oceans.Clear();
            SaveTempData();
        }

        // Наведение для кнопки острова
        private async void OnIslandPointerEntered(object sender, PointerEventArgs e)
        {
            await IslandButton.TranslateTo(0, -50, 250, Easing.CubicInOut);  // Поднимаем кнопку
            await IslandLabel.FadeTo(1, 250, Easing.CubicInOut);  // Плавное появление текста
        }

        private async void OnIslandPointerExited(object sender, PointerEventArgs e)
        {
            await IslandButton.TranslateTo(0, 0, 250, Easing.CubicInOut);  // Возвращаем кнопку на исходное положение
            await IslandLabel.FadeTo(0, 250, Easing.CubicInOut);  // Плавное исчезновение текста
        }

        // Наведение для кнопки материка
        private async void OnMainlandPointerEntered(object sender, PointerEventArgs e)
        {
            await MainlandButton.TranslateTo(0, -50, 250, Easing.CubicInOut);  // Поднимаем кнопку
            await MainlandLabel.FadeTo(1, 250, Easing.CubicInOut);  // Плавное появление текста
        }

        private async void OnMainlandPointerExited(object sender, PointerEventArgs e)
        {
            await MainlandButton.TranslateTo(0, 0, 250, Easing.CubicInOut);  // Возвращаем кнопку на исходное положение
            await MainlandLabel.FadeTo(0, 250, Easing.CubicInOut);  // Плавное исчезновение текста
        }

        // Наведение для кнопки океана
        private async void OnOceanPointerEntered(object sender, PointerEventArgs e)
        {
            await OceanButton.TranslateTo(0, -50, 250, Easing.CubicInOut);  // Поднимаем кнопку
            await OceanLabel.FadeTo(1, 250, Easing.CubicInOut);  // Плавное появление текста
        }

        private async void OnOceanPointerExited(object sender, PointerEventArgs e)
        {
            await OceanButton.TranslateTo(0, 0, 250, Easing.CubicInOut);  // Возвращаем кнопку на исходное положение
            await OceanLabel.FadeTo(0, 250, Easing.CubicInOut);  // Плавное исчезновение текста
        }

        // Обработчики нажатий кнопок для перехода к страницам создания островов, материков и океанов
        private async void OnIslandButtonClicked(object sender, EventArgs e)
        {
            SaveTempData(); // Сохраняем временные данные перед переходом на страницу
            await Navigation.PushAsync(new IslandCreatePage(tempData)); // Передаем список островов
        }

        private async void OnMainlandButtonClicked(object sender, EventArgs e)
        {
            SaveTempData(); // Сохраняем временные данные перед переходом на страницу
            await Navigation.PushAsync(new MainLandCreatePage(tempData)); // Передаем список материков
        }

        private async void OnOceanButtonClicked(object sender, EventArgs e)
        {
            SaveTempData(); // Сохраняем временные данные перед переходом на страницу
            await Navigation.PushAsync(new OceanCreatePage(tempData)); // Передаем список океанов
        }

        // Метод сохранения планеты в файл planets.json
        private async Task SavePlanet()
        {
            Planet? NewPlanet = null;
            if (string.IsNullOrWhiteSpace(SatelliteCountEntry.Text))
            {
                if (string.IsNullOrWhiteSpace(PlanetNameEntry.Text) && tempData.Islands.Count == 0 && tempData.Mainlands.Count == 0 && tempData.Oceans.Count == 0)
                {
                    Planet newPlanet = new Planet();
                    NewPlanet = newPlanet;
                }
                else
                {
                    Planet newPlanet = new Planet("NoName")
                    {
                        Islands = tempData.Islands ?? new ObservableCollection<Island>(),
                        Mainlands = tempData.Mainlands ?? new ObservableCollection<Mainland>(),
                        Oceans = tempData.Oceans ?? new ObservableCollection<Ocean>()
                    };
                    NewPlanet = newPlanet;
                }

            }
            else
            {
                int satelliteCount;
                int.TryParse(SatelliteCountEntry.Text, out satelliteCount);

                if (string.IsNullOrWhiteSpace(PlanetNameEntry.Text))
                {
                    Planet newPlanet = new Planet("NoName", satelliteCount);
                    NewPlanet = newPlanet;
                }
                else
                {
                    Planet newPlanet = new Planet(PlanetNameEntry.Text, satelliteCount)
                    {
                        Islands = new ObservableCollection<Island>(tempData.Islands),
                        Mainlands = new ObservableCollection<Mainland>(tempData.Mainlands),
                        Oceans = new ObservableCollection<Ocean>(tempData.Oceans)
                    };
                    NewPlanet = newPlanet;
                }
            }
            ObservableCollection<Planet> planets = new ObservableCollection<Planet>();
            planets = Planet.LoadPlanetsFromJson(_filePath);

            if (planets.Any(p => p.Name == PlanetNameEntry.Text))
            {
                await DisplayAlert("Ошибка", "Планета с таким именем уже существует.", "OK");
                return;
            }
            planets.Add(NewPlanet);

            Planet.SavePlanetsToJson(planets, _filePath);

            await DisplayAlert("Успех", $"Планета {PlanetNameEntry.Text} успешно сохранена.\n" +
                $"Сохранено: {NewPlanet.Satellites.Count} спутников, {NewPlanet.Islands.Count} островов, {NewPlanet.Mainlands.Count} материков, {NewPlanet.Oceans.Count} океанов.", "OK");

            ClearTempData(); // Очищаем временные данные после сохранения
        }


        // Обработчик нажатия кнопки сохранения
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            await SavePlanet();
        }

        // Очистка временных данных при закрытии страницы, если это не страница CreatePage
    }
}
