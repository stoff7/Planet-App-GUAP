namespace PlanetLib;
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
        return random.Next(min, max + 1);
    }

    public double GenerateRandomNumber(double min, double max)
    {
        return random.Next((int)min, (int)max + 1) + random.NextDouble();
    }
}