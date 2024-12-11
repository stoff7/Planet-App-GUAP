using Microsoft.Maui.Controls;
using PlanetLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PlanetApp
{
    public partial class OceanCreatePage : ContentPage
    {
        private TempData tempData; // Объект TempData для хранения данных
        private string _tempFilePath; // Путь к временному файлу

        // Конструктор принимает объект TempData
        public OceanCreatePage(TempData tempData)
        {
            InitializeComponent();
            this.tempData = tempData; // Присваиваем переданный объект

            // Устанавливаем путь к временным данным (temp.json)
            _tempFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "temp.json");
        }

        // Обработка нажатия кнопки "Создать океан"
        private void OnCreateOceanClicked(object sender, EventArgs e)
        {
            // Получаем данные с формы
            string name = OceanNameEntry.Text;
            string areaText = OceanAreaEntry.Text;
            string depthText = OceanDepthEntry.Text;

            // Валидация данных
            if (string.IsNullOrWhiteSpace(name) || 
                !double.TryParse(areaText, out double area) ||
                !double.TryParse(depthText, out double depth))
            {
                DisplayAlert("Ошибка","Пожалуйста, введите корректные данные.","OK");
                return;
            }

            // Создание нового океана
            Ocean newOcean = new Ocean(name, area, depth);

            // Проверка на уникальность имени океана
            if (tempData.Oceans.Exists(o => o.Name == name))
            {
                DisplayAlert("Ошибка","Океан с таким именем уже существует","OK");
                return;
            }

            // Добавляем новый океан в объект TempData
            tempData.Oceans.Add(newOcean);

            DisplayAlert("Успех!",$"Океан {name} успешно создан.","OK");


            // Очищаем поля ввода
            OceanNameEntry.Text = string.Empty;
            OceanAreaEntry.Text = string.Empty;
            OceanDepthEntry.Text = string.Empty;

            // Сохраняем обновленные временные данные в файл
            SaveTempData();
        }

        // Метод для сохранения временных данных в файл temp.json
        private void SaveTempData()
        {
            try
            {
                string tempJson = JsonSerializer.Serialize(tempData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_tempFilePath, tempJson);
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", $"Не удалось сохранить временные данные: {ex.Message}", "OK");
            }
        }

        // Обработка кнопки "Назад"
        private async void OnBackClicked(object sender, EventArgs e)
        {
            SaveTempData(); // Сохраняем временные данные перед возвращением на предыдущую страницу
            await Navigation.PopAsync(); // Возврат на предыдущую страницу
        }
    }
}
