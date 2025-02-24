using PlanetLib;
using System.Text.Json;

namespace PlanetApp
{
    public partial class MainLandCreatePage : ContentPage
    {
        private TempData tempData; // Объект TempData для хранения данных
        private string _tempFilePath; // Путь к временному файлу

        // Конструктор принимает объект TempData
        public MainLandCreatePage(TempData tempData)
        {
            InitializeComponent();
            this.tempData = tempData; // Присваиваем переданный объект

            // Устанавливаем путь к временным данным (temp.json)
            _tempFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "temp.json");
        }

        // Обработка нажатия кнопки "Создать материк"
        private void OnCreateMainlandClicked(object sender, EventArgs e)
        {
            // Получаем данные с формы
            string name = MainlandNameEntry.Text;
            string areaText = MainlandAreaEntry.Text;
            string tempText = MainlandTempEntry.Text;

            // Валидация данных
            if (string.IsNullOrWhiteSpace(name) ||
                !double.TryParse(areaText, out double area) ||
                !double.TryParse(tempText, out double temperature))
            {
                DisplayAlert("Ошибка", "Пожалуйста, введите корректные данные.", "OK");
                return;
            }

            // Создание нового материка
            Mainland newMainland = new Mainland(name, area, temperature);

            // Проверка на уникальность имени материка
            if (tempData.Mainlands.Any(m => m.Name == name))
            {
                DisplayAlert("Ошибка", "Материк с таким именем уже существует", "OK");
                return;
            }

            // Добавляем новый материк в объект TempData
            tempData.Mainlands.Add(newMainland);

            DisplayAlert("Успех!", $"Материк {name} успешно создан.", "OK");

            // Очищаем поля ввода
            MainlandNameEntry.Text = string.Empty;
            MainlandAreaEntry.Text = string.Empty;
            MainlandTempEntry.Text = string.Empty;

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
