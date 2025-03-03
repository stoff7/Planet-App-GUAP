using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PlanetLib;

public class Planet : IRandomDataGenerator, INotifyPropertyChanged
{

    public delegate void PlanetCreatedHandler(string message);
    public delegate void PlanetSavedHandler(bool success);
    public delegate void PlanetLoadedHandler(bool success);

    public event EventHandler<string>? PlanetCreated;
    public static event EventHandler<bool>? PlanetsSaved;
    public static event EventHandler<bool>? PlanetsLoaded;

    private static readonly int DefaultSize = 1; //Количество спутников по умолчанию

    public string Name { get; set; }
    private string imagePath;
    public string ImagePath
    {
        get => imagePath;
        set
        {
            if (imagePath != value)
            {
                imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public ObservableCollection<Ocean> Oceans { get; set; }
    public ObservableCollection<Mainland> Mainlands { get; set; }
    public ObservableCollection<Island> Islands { get; set; }
    public ObservableCollection<Satellite> Satellites { get; set; }
    public override bool Equals(object obj)
    {
        if (obj is Planet otherPlanet)
        {
            return this.Name == otherPlanet.Name &&
                   this.ImagePath == otherPlanet.ImagePath;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, ImagePath);
    }
    public Planet() //случайное создание
    {
        Name = ((IRandomDataGenerator)this).GenerateRandomName(5);
        Oceans = new ObservableCollection<Ocean>();
        for (int i = 0; i < ((IRandomDataGenerator)this).GenerateRandomNumber(0, 4); i++)
        {
            Oceans.Add(new Ocean());
        }
        Mainlands = new ObservableCollection<Mainland>();
        for (int i = 0; i < ((IRandomDataGenerator)this).GenerateRandomNumber(0, 4); i++)
        {
            Mainlands.Add(new Mainland());
        }
        Islands = new ObservableCollection<Island>();
        for (int i = 0; i < ((IRandomDataGenerator)this).GenerateRandomNumber(0, 4); i++)
        {
            Islands.Add(new Island());
        }
        Satellites = new ObservableCollection<Satellite>();
        for (int i = 0; i < ((IRandomDataGenerator)this).GenerateRandomNumber(0, 4); i++)
        {
            Satellites.Add(new Satellite());
        }
        ImagePath = "p" + ((IRandomDataGenerator)this).GenerateRandomNumber(1, 11).ToString() + ".png";
        PlanetCreated?.Invoke(this, $"Планета {Name} создана с океанами: {Oceans.Count}, материками: {Mainlands.Count}, островами: {Islands.Count}, спутниками: {Satellites.Count}");
    }

    public Planet(string name, int number) //создание с определенным количеством спутников
    {
        if (number <= 0)
        {
            throw new ArgumentException("Number must be a positive integer.", nameof(number));
        }
        if (name != "NoName")
        {
            Name = name;
        }
        Oceans = new ObservableCollection<Ocean>();
        Mainlands = new ObservableCollection<Mainland>();
        Islands = new ObservableCollection<Island>();
        Satellites = new ObservableCollection<Satellite>();
        for (int i = 0; i < number; i++)
        {
            Satellites.Add(new Satellite());
        }
        ImagePath = "p" + ((IRandomDataGenerator)this).GenerateRandomNumber(1, 11).ToString() + ".png";
        PlanetCreated?.Invoke(this, $"Планета {Name} создана с океанами: {Oceans.Count}, материками: {Mainlands.Count}, островами: {Islands.Count}, спутниками: {Satellites.Count}");
    }
    public Planet(string name) //создание только имени и спутника
    {

        Name = name;
        Oceans = new ObservableCollection<Ocean>();
        Mainlands = new ObservableCollection<Mainland>();
        Islands = new ObservableCollection<Island>();
        Satellites = new ObservableCollection<Satellite>();
        ImagePath = "p" + ((IRandomDataGenerator)this).GenerateRandomNumber(1, 11).ToString() + ".png";
        PlanetCreated?.Invoke(this, $"Планета {Name} создана с океанами: {Oceans.Count}, материками: {Mainlands.Count}, островами: {Islands.Count}, спутниками: {Satellites.Count}");
    }
    public static ObservableCollection<Planet> LoadPlanetsFromJson(string filePath)
    {
        try
        {
            string jsonContent = File.ReadAllText(filePath);
            PlanetsLoaded?.Invoke(null, true);
            return JsonSerializer.Deserialize<ObservableCollection<Planet>>(jsonContent) ?? new ObservableCollection<Planet>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка загрузки данных: " + ex.Message);
            return new ObservableCollection<Planet>(); // Возвращаем пустой список в случае ошибки
        }
    }

    public static void SavePlanetsToJson(ObservableCollection<Planet> planets, string _filePath)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            string json = JsonSerializer.Serialize(planets, options);
            File.WriteAllText(_filePath, json); // Сохраняем список планет в файл
            PlanetsSaved?.Invoke(null, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка сохранения данных: " + ex.Message);
        }
    }
}
