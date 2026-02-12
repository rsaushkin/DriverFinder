using System.Collections.Generic;

namespace DriverLocator.Models;

public interface INearestDriver
{
    List<(int driverId, double distance)> FindNearest(
        Driver registry, 
        int orderX, 
        int orderY, 
        int count = 5);
}