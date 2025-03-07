using System.Text.Json;
using System.Windows.Input;
using PlanetLib;
using System.ComponentModel;

public class CreatePlanetPartViewModel : BindableObject, INotifyPropertyChanged
{
    // Свойства для привязки с полей ввода
    private string _name;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    private string _area;
    public string Area
    {
        get => _area;
        set { _area = value; OnPropertyChanged(); }
    }

    private string _temperature;
    public string Temperature
    {
        get => _temperature;
        set { _temperature = value; OnPropertyChanged(); }
    }

    public string Destination { get; set; }
    // TempData для хранения временных данных
    public PlanetApp.TempData TempData { get; set; }

    // Путь к временному файлу
    private readonly string _tempFilePath;

    // Команды для создания материка и навигации назад
    public ICommand CreatePlanetPartCommand { get; }

    public CreatePlanetPartViewModel(PlanetApp.TempData tempData, string destination)
    {
        TempData = tempData;
        Destination = destination;
        _tempFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "temp.json");

        CreatePlanetPartCommand = new Command(async () => await OnCreate());
    }

    // Логика создания материка
    private async Task OnCreate()
    {
        // Валидация данных из полей ввода
        if (string.IsNullOrWhiteSpace(Name) ||
            !double.TryParse(Area, out double area) ||
            !double.TryParse(Temperature, out double temperature))
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите корректные данные.", "OK");
            return;
        }

        switch (Destination)
        {
            case "Island":
                if (TempData.Islands.Any(m => m.Name == Name))
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Остров с таким именем уже существует", "OK");
                    return;
                }
                // Создание нового материка и добавление в TempData
                Island newIsland = new Island(Name, area, temperature);
                TempData.Islands.Add(newIsland);

                await Application.Current.MainPage.DisplayAlert("Успех!", $"Остров {Name} успешно создан.", "OK");
                break;
            case "Mainland":
                if (TempData.Mainlands.Any(m => m.Name == Name))
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Материк с таким именем уже существует", "OK");
                    return;
                }
                // Создание нового материка и добавление в TempData
                Mainland newMainland = new Mainland(Name, area, temperature);
                TempData.Mainlands.Add(newMainland);

                await Application.Current.MainPage.DisplayAlert("Успех!", $"Материк {Name} успешно создан.", "OK");
                break;
            case "Ocean":
                if (TempData.Oceans.Any(m => m.Name == Name))
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Океан с таким именем уже существует", "OK");
                    return;
                }
                // Создание нового материка и добавление в TempData
                Ocean newOcean = new Ocean(Name, area, temperature);
                TempData.Oceans.Add(newOcean);

                await Application.Current.MainPage.DisplayAlert("Успех!", $"Океан {Name} успешно создан.", "OK");
                break;
            default:
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста перезагрузите страницу", "OK");
                return;
        }

        // Очистка полей ввода
        Name = string.Empty;
        Area = string.Empty;
        Temperature = string.Empty;

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
