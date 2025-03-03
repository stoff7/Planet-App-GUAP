namespace PlanetLib;
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
    public Satellite(string name, double mass)
    {
        Name = name;
        Mass = mass;
    }
}