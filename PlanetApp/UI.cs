namespace PlanetApp.Model
{
    public class Card<T>
    {
        public string Name { get; set; }
        public double FirstDouble { get; set; }
        public double SecondDouble { get; set; }
        public T SelectedItem { get; set; }
    }
}


