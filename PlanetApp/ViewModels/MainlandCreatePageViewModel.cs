using System.Text.Json;
using System.Windows.Input;
using PlanetLib;
using System.ComponentModel;

public class MainlandCreatePageViewModel : BindableObject, INotifyPropertyChanged
{
    // Свойства для привязки с полей ввода
    private string _mainlandName;
    public string MainlandName
    {
        get => _mainlandName;
        set { _mainlandName = value; OnPropertyChanged(); }
    }

    private string _mainlandArea;
    public string MainlandArea
    {
        get => _mainlandArea;
        set { _mainlandArea = value; OnPropertyChanged(); }
    }

    private string _mainlandTemperature;
    public string MainlandTemperature
    {
        get => _mainlandTemperature;
        set { _mainlandTemperature = value; OnPropertyChanged(); }
    }

    // TempData для хранения временных данных
    public PlanetApp.TempData TempData { get; set; }

    // Путь к временному файлу
    private readonly string _tempFilePath;

    // Команды для создания материка и навигации назад
    public ICommand CreateMainlandCommand { get; }

    public MainlandCreatePageViewModel(PlanetApp.TempData tempData)
    {
        TempData = tempData;
        _tempFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "temp.json");

        CreateMainlandCommand = new Command(async () => await OnCreateMainland());
    }

    // Логика создания материка
    private async Task OnCreateMainland()
    {
        // Валидация данных из полей ввода
        if (string.IsNullOrWhiteSpace(MainlandName) ||
            !double.TryParse(MainlandArea, out double area) ||
            !double.TryParse(MainlandTemperature, out double temperature))
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите корректные данные.", "OK");
            return;
        }

        // Проверка уникальности имени материка
        if (TempData.Mainlands.Any(m => m.Name == MainlandName))
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Материк с таким именем уже существует", "OK");
            return;
        }

        // Создание нового материка и добавление в TempData
        Mainland newMainland = new Mainland(MainlandName, area, temperature);
        TempData.Mainlands.Add(newMainland);

        await Application.Current.MainPage.DisplayAlert("Успех!", $"Материк {MainlandName} успешно создан.", "OK");

        // Очистка полей ввода
        MainlandName = string.Empty;
        MainlandArea = string.Empty;
        MainlandTemperature = string.Empty;

        SaveTempData();
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
