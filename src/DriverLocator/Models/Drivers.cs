namespace DriverLocator.Models;

public class Driver
{
    private readonly int _width;
    private readonly int _height;
    private readonly Dictionary<int, (int x, int y)> _drivers = new();

    public Driver(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public void AddOrUpdateDriver(int driverId, int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
            throw new ArgumentOutOfRangeException("Координаты вне границ карты");
        _drivers[driverId] = (x, y);
    }

    public IReadOnlyDictionary<int, (int x, int y)> GetAllDrivers() => _drivers;
    public int Width => _width;
    public int Height => _height;
}