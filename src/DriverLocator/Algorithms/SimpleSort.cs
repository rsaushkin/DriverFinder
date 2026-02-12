using System;
using System.Collections.Generic;
using System.Linq;
using DriverLocator.Models;

namespace DriverLocator.Algorithms;

public class SimpleSort : INearestDriver
{
    public List<(int driverId, double distance)> FindNearest(
        Driver registry, int orderX, int orderY, int count = 5)
    {
        var drivers = registry.GetAllDrivers();
        if (drivers.Count == 0) return new();

        var list = drivers.Select(d => (
            driverId: d.Key,
            distance: Math.Sqrt(Math.Pow(d.Value.x - orderX, 2) + Math.Pow(d.Value.y - orderY, 2))
        )).ToList();

        list.Sort((a, b) =>
        {
            int cmp = a.distance.CompareTo(b.distance);
            return cmp != 0 ? cmp : a.driverId.CompareTo(b.driverId);
        });

        return list.Take(count).ToList();
    }
}