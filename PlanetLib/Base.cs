namespace PlanetLib;
using System.ComponentModel;
public abstract class PlanetPart : IRandomDataGenerator, INotifyPropertyChanged
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
    public double Area { get; set; } // Площадь
    public double AverageTemperature { get; set; } // Средняя температура
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected PlanetPart(string name, double area, double averageTemperature)
    {
        Name = name;
        Area = area;
        AverageTemperature = averageTemperature;
    }

    protected PlanetPart()
    {
        Name = ((IRandomDataGenerator)this).GenerateRandomName(5);
        Area = ((IRandomDataGenerator)this).GenerateRandomNumber(1000.0, 100000.0);
        AverageTemperature = ((IRandomDataGenerator)this).GenerateRandomNumber(-30.0, 30.0);

    }

    public abstract void DisplayInfo();
}
