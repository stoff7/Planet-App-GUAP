namespace PlanetLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// Главный класс Planet
public class Planet : IRandomDataGenerator
{
    [JsonInclude]
    [JsonConverter(typeof(SatelliteArrayConverter))]
    public Satellite[] satellites;
    public delegate void PlanetCreatedHandler(string message);
    public delegate void PlanetSavedHandler(bool success);
    public delegate void PlanetLoadedHandler(bool success);

    public event EventHandler<string>? PlanetCreated;
    public static event EventHandler<bool>? PlanetsSaved;
    public static event EventHandler<bool>? PlanetsLoaded;
    
    private static readonly int DefaultSize = 1; //Количество спутников по умолчанию

    public string Name { get; set; } 
    public string ImagePath{ get; set;}
    public List<Ocean> Oceans { get; set; }
    public List<Mainland> Mainlands { get; set; }
    public List<Island> Islands { get; set; }
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
        Oceans = new List<Ocean>();
        for (int i = 0;i < ((IRandomDataGenerator)this).GenerateRandomNumber(0,4); i++){
            Oceans.Add(new Ocean());
        }
        Mainlands = new List<Mainland>();
        for (int i = 0;i < ((IRandomDataGenerator)this).GenerateRandomNumber(0,4); i++){
            Mainlands.Add(new Mainland());
        }
        Islands = new List<Island>();
        for (int i = 0;i < ((IRandomDataGenerator)this).GenerateRandomNumber(0,4); i++){
            Islands.Add(new Island());
        }
        int size = ((IRandomDataGenerator)this).GenerateRandomNumber(0,4);
        satellites = new Satellite[size];
        for (int i = 0; i < size; i++)
        {
            satellites[i] = new Satellite();
        }
        ImagePath = "p" + ((IRandomDataGenerator)this).GenerateRandomNumber(1, 11).ToString() + ".png";
        PlanetCreated?.Invoke (this, $"Планета {Name} создана с океанами: {Oceans.Count}, материками: {Mainlands.Count}, островами: {Islands.Count}, спутниками: {satellites.Length}");
    }

    public Planet(string name, int number) //создание с определенным количеством спутников
    {
        if (number <= 0)
        {
            throw new ArgumentException("Number must be a positive integer.", nameof(number));
        }

        Name = name;
        Oceans = new List<Ocean>();
        Mainlands = new List<Mainland>();
        Islands = new List<Island>();
        satellites = new Satellite[number];
        for (int i = 0; i < number; i++)
        {
            satellites[i] = new Satellite();
        }
        ImagePath = "p" + ((IRandomDataGenerator)this).GenerateRandomNumber(1, 11).ToString() + ".png";
        PlanetCreated?.Invoke (this, $"Планета {Name} создана с океанами: {Oceans.Count}, материками: {Mainlands.Count}, островами: {Islands.Count}, спутниками: {satellites.Length}");
    }
    public Planet(string name) //создание только имени и спутника
    {

        Name = name;
        Oceans = new List<Ocean>();
        Mainlands = new List<Mainland>();
        Islands = new List<Island>();
        satellites = new Satellite[DefaultSize];
        satellites[0] = new Satellite();
        ImagePath = "p" + ((IRandomDataGenerator)this).GenerateRandomNumber(1, 11).ToString() + ".png";
        PlanetCreated?.Invoke (this, $"Планета {Name} создана с океанами: {Oceans.Count}, материками: {Mainlands.Count}, островами: {Islands.Count}, спутниками: {satellites.Length}");
    }
    public Satellite this[int index]
    {
        get => index >= 0 && index < satellites.Length
            ? satellites[index]
            : throw new IndexOutOfRangeException("Invalid satellite index.");
        set
        {
            if (index >= 0 && index < satellites.Length)
                satellites[index] = value;
            else
                throw new IndexOutOfRangeException("Invalid satellite index.");
        }
    }


    public Satellite? this[string name]
    {
        get
        {
            return satellites.FirstOrDefault(i => i.Name == name);
        }
    }

    public static List<Planet> LoadPlanetsFromJson(string filePath)
    {
        try
        {
            string jsonContent = File.ReadAllText(filePath);
            PlanetsLoaded?.Invoke(null, true);
            return JsonSerializer.Deserialize<List<Planet>>(jsonContent) ?? new List<Planet>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка загрузки данных: " + ex.Message);
            return new List<Planet>(); // Возвращаем пустой список в случае ошибки
        }
    }

    public static void SavePlanetsToJson(List<Planet> planets, string _filePath)
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

// Класс Ocean
public class Ocean : PlanetPart, IRandomDataGenerator
{
    public Ocean(string name, double area, double averageTemperature)
        : base(name, area, averageTemperature) { }

    public Ocean()
        : base() { }

    public override bool Equals(object obj)
    {
        if (obj is Ocean otherOcean)
        {
            return this.Name == otherOcean.Name &&
                   this.Area == otherOcean.Area &&
                   this.AverageTemperature == otherOcean.AverageTemperature;
        }
        return false;
    }
    public override void DisplayInfo()
    {
        Console.WriteLine($"Ocean: {Name}, Area: {Area} sq km, Avg Temp: {AverageTemperature}°C");
    }
}

// Класс Mainland (материк)
public class Mainland : PlanetPart,IRandomDataGenerator 
{
    public Mainland(string name, double area, double averageTemperature)
        : base(name, area, averageTemperature) { }

    public Mainland()
        : base() { }

    public override bool Equals(object obj)
    {
        if (obj is Mainland otherMainland)
        {
            return this.Name == otherMainland.Name &&
                   this.Area == otherMainland.Area &&
                   this.AverageTemperature == otherMainland.AverageTemperature;
        }
        return false;
    }
    public override void DisplayInfo()
    {
        Console.WriteLine($"Mainland: {Name}, Area: {Area} sq km, Avg Temp: {AverageTemperature}°C");
    }
}

// Класс Island (остров)
public class Island : PlanetPart, IRandomDataGenerator
{
    public Island(string name, double area, double averageTemperature)
        : base(name, area, averageTemperature) { }
    
    public Island()
        : base() { }

    public override bool Equals(object obj)
    {
        if (obj is Island otherIsland)
        {
            return this.Name == otherIsland.Name &&
                   this.Area == otherIsland.Area &&
                   this.AverageTemperature == otherIsland.AverageTemperature;
        }
        return false;
    }
    public override void DisplayInfo()
    {
        Console.WriteLine($"Island: {Name}, Area: {Area} sq km, Avg Temp: {AverageTemperature}°C");
    }
}

// Абстрактный класс PlanetPart
public abstract class PlanetPart : IRandomDataGenerator
{
    public string Name { get; set; }
    public double Area { get; set; } // Площадь
    public double AverageTemperature { get; set; } // Средняя температура

    protected PlanetPart(string name, double area, double averageTemperature)
    {
        Name = name;
        Area = area;
        AverageTemperature = averageTemperature;
    }

    protected PlanetPart()
    {
        Name = ((IRandomDataGenerator)this).GenerateRandomName(5);
        Area = ((IRandomDataGenerator)this).GenerateRandomNumber(1000.0,100000.0);
        AverageTemperature = ((IRandomDataGenerator)this).GenerateRandomNumber(-30.0,30.0);

    }

    public abstract void DisplayInfo();
}


public class Satellite : IRandomDataGenerator
{

    public string Name { get; set; }
    public double Mass { get; set; }
    public override bool Equals(object obj)
    {
        if (obj is Satellite otherSatellite)
        {
            return this.Name == otherSatellite.Name &&
                   this.Mass == otherSatellite.Mass;
        }
        return false;
    }

    public Satellite()
    {
        Name = ((IRandomDataGenerator)this).GenerateRandomName(5);
        Mass = ((IRandomDataGenerator)this).GenerateRandomNumber(100, 5000);
    }
    public Satellite(string name,double mass)
    {
        Name = name;
        Mass = mass;
    }
}

public interface IRandomDataGenerator
{
    public static readonly Random random = new Random();

    // Метод для генерации случайного имени
    public string GenerateRandomName(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] buffer = new char[length];
        for (int i = 0; i < length; i++)
        {
            buffer[i] = chars[random.Next(chars.Length)];
        }
        return new string(buffer);
    }

    // Метод для генерации случайной массы
    public int GenerateRandomNumber(int min, int max)
    {
        return random.Next(min, max+1);
    }

    public double GenerateRandomNumber(double min, double max)
    {
        return random.Next((int)min, (int)max+1) + random.NextDouble();
    }
}


public class SatelliteArrayConverter : JsonConverter<Satellite[]>
{
    public override Satellite[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Чтение массива спутников из JSON
        return JsonSerializer.Deserialize<Satellite[]>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, Satellite[] value, JsonSerializerOptions options)
    {
        // Запись массива спутников в JSON
        JsonSerializer.Serialize(writer, value, options);
    }
}
