using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using PlanetLib;
using System.ComponentModel;

namespace PlanetApp
{
    public class OceanCreatePageViewModel : BindableObject, INotifyPropertyChanged
    {
        private string _oceanName;
        public string OceanName
        {
            get => _oceanName;
            set { _oceanName = value; OnPropertyChanged(); }
        }

        private string _oceanArea;
        public string OceanArea
        {
            get => _oceanArea;
            set { _oceanArea = value; OnPropertyChanged(); }
        }

        private string _oceanTemperature;
        public string OceanTemperature
        {
            get => _oceanTemperature;
            set { _oceanTemperature = value; OnPropertyChanged(); }
        }

        // TempData для хранения временных данных
        public TempData TempData { get; set; }

        // Путь к временному файлу
        private readonly string _tempFilePath;

        // Команды для создания океана и навигации назад
        public ICommand CreateOceanCommand { get; }


        public OceanCreatePageViewModel(TempData tempData)
        {
            TempData = tempData;
            _tempFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "temp.json");

            CreateOceanCommand = new Command(async () => await OnCreateOcean());
        }

        // Логика создания океана
        private async Task OnCreateOcean()
        {
            // Валидация введённых данных
            if (string.IsNullOrWhiteSpace(OceanName) ||
                !double.TryParse(OceanArea, out double area) ||
                !double.TryParse(OceanTemperature, out double Temperature))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите корректные данные.", "OK");
                return;
            }

            // Проверка уникальности имени океана
            if (TempData.Oceans.Any(o => o.Name == OceanName))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Океан с таким именем уже существует", "OK");
                return;
            }

            // Создание нового океана и добавление в TempData
            Ocean newOcean = new Ocean(OceanName, area, Temperature);
            TempData.Oceans.Add(newOcean);

            await Application.Current.MainPage.DisplayAlert("Успех!", $"Океан {OceanName} успешно создан.", "OK");

            // Очистка полей ввода
            OceanName = string.Empty;
            OceanArea = string.Empty;
            OceanTemperature = string.Empty;

            SaveTempData();
        }

        // Логика навигации назад
        private async Task OnBack()
        {
            SaveTempData();
            await Shell.Current.GoToAsync("..");
        }

        // Сохранение TempData в файл temp.json
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
