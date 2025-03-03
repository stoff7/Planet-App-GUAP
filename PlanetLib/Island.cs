namespace PlanetLib;
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
