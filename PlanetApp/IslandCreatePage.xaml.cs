using Microsoft.Maui.Controls;
using PlanetLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PlanetApp
{
    public partial class IslandCreatePage : ContentPage
    {
        private TempData tempData; // Объект TempData для хранения данных
        private string _tempFilePath; // Путь к временному файлу

        // Конструктор принимает объект TempData
        public IslandCreatePage(TempData tempData)
        {
            InitializeComponent();
            this.tempData = tempData; // Присваиваем переданный объект

            // Устанавливаем путь к временным данным (temp.json)
            _tempFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "temp.json");
        }

        private void LoadTempData()
        {
            if (File.Exists(_tempFilePath))
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
        }
        // Обработка нажатия кнопки "Создать остров"
        private void OnCreateIslandClicked(object sender, EventArgs e)
        {
            // Получаем данные с формы
            string name = IslandNameEntry.Text;
            string areaText = IslandAreaEntry.Text;
            string tempText = IslandTempEntry.Text;

            // Валидация данных
            if (string.IsNullOrWhiteSpace(name) ||
                !double.TryParse(areaText, out double area) ||
                !double.TryParse(tempText, out double temperature))
            {
                DisplayAlert("Ошибка", "Пожалуйста, введите корректные данные.", "OK");
                return;
            }

            // Создание нового острова
            Island newIsland = new Island(name, area, temperature);

            // Проверка на уникальность имени острова
            if (tempData.Islands.Any(i => i.Name == name))
            {
                DisplayAlert("Ошибка", "Остров с таким именем уже существует", "OK");
                return;
            }

            // Добавляем новый остров в объект TempData
            tempData.Islands.Add(newIsland);

            DisplayAlert("Успех!", $"Остров {name} успешно создан.", "OK");

            // Очищаем поля ввода
            IslandNameEntry.Text = string.Empty;
            IslandAreaEntry.Text = string.Empty;
            IslandTempEntry.Text = string.Empty;

            // Сохраняем обновленные временные данные в файл
            SaveTempData();
        }

        // Метод для сохранения временных данных в файл temp.json
        private void SaveTempData()
        {
            try
            {
                string tempJson = JsonSerializer.Serialize(tempData, new JsonSerializerOptions { WriteIndented = true });

                // Сохраняем данные в файл
                File.WriteAllText(_tempFilePath, tempJson);
            }
            catch (Exception ex)
            {
                // Обработка ошибок при сохранении
                DisplayAlert("Ошибка", $"Не удалось сохранить временные данные: {ex.Message}", "OK");
            }
        }

        // Обработка кнопки "Назад"
        private async void OnBackClicked(object sender, EventArgs e)
        {
            // Сохраняем временные данные перед возвратом на предыдущую страницу
            SaveTempData();
            await Navigation.PopAsync(); // Возврат на предыдущую страницу
        }
    }
}
