namespace PlanetLib;
public class Mainland : PlanetPart, IRandomDataGenerator
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