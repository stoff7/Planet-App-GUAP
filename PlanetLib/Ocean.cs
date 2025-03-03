namespace PlanetLib;
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